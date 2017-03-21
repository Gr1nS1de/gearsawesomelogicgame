using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Thinksquirrel.Phys2D;


public class GearsInputController : Controller
{
	private GearView 						currentGearView 			{ get { return game.view.currentGearView; } set { game.view.currentGearView = value; } }
	private GearModel 						currentGearModel 			{ get { return gearsDictionary[currentGearView]; } }
	private SelectedGearModel				selectedGearModel			{ get { return game.model.selectedGearModel;}}
	private GearsFactoryModel 				gearsFactoryModel 			{ get { return gearsFactoryModel; } }
	private List<GearView>					gearsList					{ get { return gearsFactoryModel.instantiatedGearsList; } }
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return gearsFactoryModel.gearsDictionary; } }

	private Vector3							_selectedPoint;
	private Vector3							_selectedPointDelta;
	private bool 							_isCanMoveFlag				= false;
	private bool 							_isGearPositionCorrect 		= true;

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameOnStart:
				{
					OnStart();

					break;
				}


			case N.OnInputGear___:
				{
					GameObject dragItem = (GameObject)data [0];
					Vector3 inputPoint = (Vector3)data [1];
					FingerMotionPhase gesturePhase = (FingerMotionPhase)data [2];

					//If just start drag with new gear
					if (dragItem != null)
					{
					
						//Get item gear view
						GearView gearElement = dragItem.GetComponent<GearView> ();

						if (gearElement)
							OnDragGear (gearElement, inputPoint, gesturePhase);
						else
							Debug.LogError ("Input gear element == null. ");
					}
					else if (currentGearView)
					{
						OnDragGear (currentGearView, inputPoint, gesturePhase);
					}

					break;
				}

			case N.GameOver:
				{
					OnGameOver ();

					break;
				}
		}
	}

	private void OnStart()
	{
	}

	private void OnDragGear (GearView selectedGear, Vector3 inputPoint, FingerMotionPhase gesturePhase)
	{
		//Debug.Log ("Drag gear = " + selectedGear.gameObject.name + " point " + inputPoint);

		Vector3 selectedPoint = new Vector3 (inputPoint.x, inputPoint.y, -2f);// Z = -2 bcs move object forward to camera
		Vector3 gearPosition = selectedGear.transform.position;

		switch (gesturePhase)
		{
			case FingerMotionPhase.Started:
				{
					if (!_isGearPositionCorrect)
						return;
					
					if (currentGearView != null)
						return;

					_isGearPositionCorrect = false;

					//Init vars for control gear and visual effects
					SelectGear (selectedGear);

					//Get delta between touch point and gear center position - for proper gear drag
					_selectedPointDelta = new Vector3(gearPosition.x, gearPosition.y, selectedPoint.z) - selectedPoint;

					_isCanMoveFlag = true;
					break;
				}

			case FingerMotionPhase.Updated:
				{
					if (_isCanMoveFlag)
					{
						MoveCurrentGear (selectedPoint);
					}
					break;
				}

			case FingerMotionPhase.Ended:
				{
					_isCanMoveFlag = false;

					DeselectCurrentGear ();
					break;
				}
		}

	}

	#region Gear input control methods
	private void SelectGear(GearView gear)
	{
		int currentGearLayer = gear.gameObject.layer;

		Debug.Log ("Select gear " + gear.name);
		foreach (var gearEventCollider in gear.GetComponentsInChildren<GearColliderView> ())
		{
			gearEventCollider.isSendEntryNotification = true;
		}

		currentGearView = gear;
		currentGearView.gameObject.layer = currentGearLayer == LayerMask.NameToLayer ("ConnectedGear") ? LayerMask.NameToLayer ("SelectedConnectedGear") : LayerMask.NameToLayer ("SelectedGear");
		selectedGearModel.lastCorrectPosition = gear.transform.position;

		DetachCurrentGear ();
	}

	private void MoveCurrentGear(Vector3 selectedPoint)
	{
		Vector3 position = selectedPoint + _selectedPointDelta;

		position.z = -2f;

		currentGearView.transform.DOMove(position, 0.1f).SetId(this);
		currentGearView.GetComponent<HingeJoint2DExt> ().connectedAnchor = (Vector2)currentGearView.transform.position;

	}

	private void DeselectCurrentGear()
	{
		if (currentGearView && !DOTween.IsTweening("TWEEN_RESETING_CURRENT_GEAR"))
		{
			Vector3 gearCurrentPosition = currentGearView.transform.position;

			//Stop send trigger events from Base & Spin colliders
			foreach (var gearEventCollider in currentGearView.GetComponentsInChildren<GearColliderView> ())
			{
				gearEventCollider.isSendEntryNotification = false;
			}
				
			//If wrong position - set current position as last saved correct position
			if (selectedGearModel.baseCollisionsCount != 0)
			{
				//_lastBaseCorrectPoint = _lastBaseCorrectPoint;
				gearCurrentPosition  = game.model.selectedGearModel.lastCorrectPosition;
			}

			//Push gear back to normal Z for deselected player gear
			gearCurrentPosition.z = -1f;

			DOTween.Kill (this);

			currentGearView.transform.DOMove (gearCurrentPosition, 0.2f)
				.OnComplete (() =>
				{
					ResetCurrentGear ();
				})
				.SetId("TWEEN_RESETING_CURRENT_GEAR");
			
		}
	}

	#endregion Gear input control methods

	private void ResetCurrentGear()
	{
		Debug.Log ("Reset current gear "+ currentGearView.name);

		AttachCurrentGear ();

		//currentGearView.gameObject.layer = LayerMask.NameToLayer ("PlayerGear");

		selectedGearModel.baseCollisionsCount = 0;

		_isGearPositionCorrect = true;

		GearView storedCurrentGear = game.view.currentGearView;

		//Bugfix. Fixing stuck of chain by overlaping on gear reset.
		storedCurrentGear.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;

		DOVirtual.DelayedCall (0.05f, () =>
		{
			Notify (N.UpdateGearsChain);
			storedCurrentGear.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
		});

		if (selectedGearModel.gearModel.gearPositionState == GearPositionState.DEFAULT)
			Utils.SetGearLayer (currentGearView, GearLayer.PLAYER);
		else if(selectedGearModel.gearModel.gearPositionState == GearPositionState.CONNECTED)
			Utils.SetGearLayer (currentGearView, GearLayer.CONNECTED);

		currentGearView = null;
	}

	private void DetachCurrentGear()
	{
		currentGearView.GetComponent<HingeJoint2DExt> ().enabled = false;
	}
		
	private void AttachCurrentGear()
	{
		currentGearView.GetComponent<HingeJoint2DExt> ().connectedAnchor = (Vector2)currentGearView.transform.position;
		currentGearView.GetComponent<HingeJoint2DExt> ().enabled = true;
	}

	private void OnGameOver()
	{

	}
		
}