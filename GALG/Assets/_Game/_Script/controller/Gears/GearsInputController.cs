using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Thinksquirrel.Phys2D;

public class GearsInputController : Controller
{
	private GearModel 						currentGearModel 			{ get { return gearsDictionary[game.view.currentGearView]; } }
	private GearsFactoryModel 				gearsFactoryModel 			{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsList					{ get { return gearsFactoryModel.themeGearsPrefabsList; } }
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
					else if (game.view.currentGearView)
					{
						OnDragGear (game.view.currentGearView, inputPoint, gesturePhase);
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

		Vector3 selectedPoint = new Vector3 (inputPoint.x, inputPoint.y, -2f);
		Vector3 gearPosition = selectedGear.transform.position;

		switch (gesturePhase)
		{
			case FingerMotionPhase.Started:
				{
					if (!_isGearPositionCorrect)
						return;
					
					if (game.view.currentGearView)
						return;

					_isGearPositionCorrect = false;

					//Init vars for control gear and visual effects
					SelectGear (selectedGear);

					//Move forward to camera
					gearPosition.z = -2;

					_selectedPointDelta = gearPosition - selectedPoint;

					selectedGear.transform.position = gearPosition;

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
		foreach (var gearEventCollider in gear.GetComponentsInChildren<GearColliderView> ())
		{
			gearEventCollider.isSendEntryNotification = true;
		}

		game.view.currentGearView = gear;
		game.view.currentGearView.gameObject.layer = LayerMask.NameToLayer ("SelectedGear");
		game.model.currentGearModel.lastCorrectPosition = gear.transform.position;

		DetachCurrentGear ();
	}

	private void MoveCurrentGear(Vector3 selectedPoint)
	{
		Vector3 position = selectedPoint + _selectedPointDelta;

		position.z = -2f;

		game.view.currentGearView.transform.DOMove(position, 0.2f).SetId(this);
	}

	private void DeselectCurrentGear()
	{
		if (game.view.currentGearView)
		{
			Vector3 gearPosition = game.view.currentGearView.transform.position;

			foreach (var gearEventCollider in game.view.currentGearView.GetComponentsInChildren<GearColliderView> ())
			{
				gearEventCollider.isSendEntryNotification = false;
			}
				
			if (game.model.currentGearModel.collisionsCount != 0)
			{
				//_lastBaseCorrectPoint = _lastBaseCorrectPoint;
				gearPosition  = game.model.currentGearModel.lastCorrectPosition;
			}

			gearPosition.z = -1f;

			DOTween.Kill (this);

			game.view.currentGearView.transform.DOMove (gearPosition, 0.2f)
				.OnComplete (() =>
			{
				ResetCurrentGear ();
			});
			
		}
	}

	#endregion Gear input control methods

	private void ResetCurrentGear()
	{
		AttachCurrentGear ();

		game.view.currentGearView.gameObject.layer = LayerMask.NameToLayer ("PlayerGear");
		game.view.currentGearView = null;

		game.model.currentGearModel.collisionsCount = 0;

		_isGearPositionCorrect = true;

		Notify (N.UpdateGearsChain);
	}

	private void DetachCurrentGear()
	{
		game.view.currentGearView.GetComponent<HingeJoint2DExt> ().enabled = false;
	}
		
	private void AttachCurrentGear()
	{
		game.view.currentGearView.GetComponent<HingeJoint2DExt> ().connectedAnchor = (Vector2)game.view.currentGearView.transform.position;
		game.view.currentGearView.GetComponent<HingeJoint2DExt> ().enabled = true;
	}

	private void OnGameOver()
	{

	}
		
}