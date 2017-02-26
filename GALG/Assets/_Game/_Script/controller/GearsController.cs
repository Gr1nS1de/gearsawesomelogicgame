using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GearsController : Controller
{
	private GearModel 			currentGearModel 		{ get { return game.model.playerModel; } }
	private GearsFactoryModel 	gearsFactoryModel 		{ get { return game.model.gearsFactoryModel; } }

	private GearView	_currentGearView; 
	private Vector3		_selectedPoint;
	private bool 		_isCanMoveFlag				= false;

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameOnStart:
				{
					OnStart();

					break;
				}


			case N.InputOnDrag___:
				{
					GameObject dragItem = (GameObject)data [0];
					Vector3 inputPoint = (Vector3)data [1];
					FingerMotionPhase gesturePhase = (FingerMotionPhase)data [2];

					if (dragItem != null)
					{
					
						GearView gearElement = dragItem.GetComponent<GearView> ();

						if (gearElement)
							OnDragGear (gearElement, inputPoint, gesturePhase);
					}
					else if (_currentGearView)
					{
						OnDragGear (_currentGearView, inputPoint, gesturePhase);
					}

					break;
				}

			case N.GearsColliderTriggered______:
				{
					GearView triggerGear = (GearView)data [0];
					GearView triggeredGear = (GearView)data [1];
					Vector3 collisionPoint = (Vector3)data [2];
					GearColliderType triggerColliderType = (GearColliderType)data [3];
					GearColliderType triggeredColliderType = (GearColliderType)data [4];
					bool isEnterCollision = (bool)data [5];

					if (triggerGear != _currentGearView)
					{
						Debug.LogError ("Trigger gear is not current selected!");
						return;
					}

					if (isEnterCollision)
						OnGearsEnterCollised (triggerGear, triggeredGear, collisionPoint, triggerColliderType, triggeredColliderType);
					else
						OnGearsExitCollised (triggerGear, triggeredGear, collisionPoint, triggerColliderType, triggeredColliderType);
					
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
					if (_currentGearView)
						return;

					SelectGear (selectedGear);

					selectedGear.transform.DOMove (selectedPoint, 0.2f)
						.SetUpdate(UpdateType.Normal)
						.SetEase (Ease.InOutCubic)
						.OnComplete (() =>
						{
							_isCanMoveFlag = true;
						}).SetId("START_MOVE");
					break;
				}

			case FingerMotionPhase.Updated:
				{
					if (_isCanMoveFlag)
					{
						_currentGearView.transform.DOMove(selectedPoint, 0.2f);
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

	private void SelectGear(GearView gear)
	{
		//Setup position for light
		game.view.gearLightView.transform.SetParent (gear.transform);
		game.view.gearLightView.transform.DOLocalMove (Vector3.zero, 0.2f);

		foreach (var gearEventCollider in gear.GetComponentsInChildren<GearColliderView> ())
		{
			gearEventCollider.isSendNotification = true;
		}

		_currentGearView = gear;
		_currentGearView.gameObject.layer = LayerMask.NameToLayer ("SelectedGear");
	}

	private void DeselectCurrentGear()
	{
		if (_currentGearView)
		{
			Vector3 gearPosition = _currentGearView.transform.position;

			gearPosition.z = -1f;

			foreach (var gearEventCollider in _currentGearView.GetComponentsInChildren<GearColliderView> ())
			{
				gearEventCollider.isSendNotification = false;
			}

			_currentGearView.transform.position = gearPosition;
			_currentGearView.gameObject.layer = LayerMask.NameToLayer ("PlayerGear");
			_currentGearView = null;

			DOTween.Kill ("START_MOVE");
		}
	}

	private void OnGearsEnterCollised(GearView triggerGear, GearView triggeredGear, Vector3 collisionPoint, GearColliderType triggerColliderType, GearColliderType triggeredColliderType)
	{


	}


	private void OnGearsExitCollised(GearView triggerGear, GearView triggeredGear, Vector3 collisionPoint, GearColliderType triggerColliderType, GearColliderType triggeredColliderType)
	{


	}


	private void OnGameOver()
	{

	}
		
}