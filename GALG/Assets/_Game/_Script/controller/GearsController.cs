using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GearsController : Controller
{
	private GearModel 						currentGearModel 			{ get { return gearsDictionary[_currentGearView]; } }
	private GearsFactoryModel 				gearsFactoryModel 			{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsList					{ get { return game.model.gearsFactoryModel.loadedGears; } }
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return game.model.gearsFactoryModel.gearsDictionary; } }

	private GearView						_currentGearView; 
	private Vector3							_selectedPoint;
	private Vector3 						_lastBaseCorrectPoint;
	private bool 							_isCanMoveFlag				= false;
	private int 							_baseCollisionsCount		= 0;
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
					GearColliderType triggerColliderType = (GearColliderType)data [2];
					GearColliderType triggeredColliderType = (GearColliderType)data [3];
					bool isEnterCollision = (bool)data [4];

					if (triggerGear != _currentGearView)
					{
						Debug.LogError ("Trigger gear is not current selected!");
						return;
					}

					if (isEnterCollision)
						OnGearsEnterCollised (triggerGear, triggeredGear, triggerColliderType, triggeredColliderType);
					else
						OnGearsExitCollised (triggerGear, triggeredGear, triggerColliderType, triggeredColliderType);
					Debug.Log ("Collisions count = " + _baseCollisionsCount);
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
					
					if (_currentGearView)
						return;

					_isGearPositionCorrect = false;

					SelectGear (selectedGear);

					selectedGear.transform.DOMove (selectedPoint, 0.2f)
						.SetUpdate(UpdateType.Normal)
						.SetEase (Ease.InOutCubic)
						.OnComplete (() =>
						{
							_isCanMoveFlag = true;
						}).SetId(this);
					break;
				}

			case FingerMotionPhase.Updated:
				{
					if (_isCanMoveFlag)
					{
						_currentGearView.transform.DOMove(selectedPoint, 0.2f).SetId(this);
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

			foreach (var gearEventCollider in _currentGearView.GetComponentsInChildren<GearColliderView> ())
			{
				gearEventCollider.isSendNotification = false;
			}
				
			if (_baseCollisionsCount != 0)
			{
				//_lastBaseCorrectPoint = _lastBaseCorrectPoint;
				gearPosition  = _lastBaseCorrectPoint;
			}

			gearPosition.z = -1f;

			DOTween.Kill (this);

			_currentGearView.transform.DOMove (gearPosition, 0.1f)
				.OnComplete (() =>
			{
				ResetCurrentGear ();
			});
			
		}
	}

	private void ResetCurrentGear()
	{

		_currentGearView.gameObject.layer = LayerMask.NameToLayer ("PlayerGear");
		_currentGearView = null;

		_baseCollisionsCount = 0;

		_isGearPositionCorrect = true;
	}

	private void OnGearsEnterCollised(GearView triggerGear, GearView triggeredGear, GearColliderType triggerColliderType, GearColliderType triggeredColliderType)
	{
		switch (triggerColliderType)
		{
			case GearColliderType.BASE:
				{
					switch (triggeredColliderType)
					{
						case GearColliderType.BASE:
							break;

						case GearColliderType.SPIN:
							{
								_baseCollisionsCount++;
								_lastBaseCorrectPoint = triggerGear.transform.position + Vector3.ClampMagnitude (triggerGear.transform.position + new Vector3(0f,0f,1f) - triggeredGear.transform.position, 0.5f);
								Debug.DrawLine (Vector3.zero, _lastBaseCorrectPoint, Color.white, 1f);
								//Debug.Break ();
								break;
							}
					}
					break;
				}

			case GearColliderType.SPIN:
				{
					switch (triggeredColliderType)
					{
						case GearColliderType.BASE:
							break;

						case GearColliderType.SPIN:
							{
								
								break;
							}
					}

					break;
				}
		}
	}


	private void OnGearsExitCollised(GearView triggerGear, GearView triggeredGear, GearColliderType triggerColliderType, GearColliderType triggeredColliderType)
	{
		switch (triggerColliderType)
		{
			case GearColliderType.BASE:
				{
					switch (triggeredColliderType)
					{
						case GearColliderType.BASE:
							{

								break;
							}

						case GearColliderType.SPIN:
							{
								_baseCollisionsCount--;
								break;
							}
					}

					break;
				}

			case GearColliderType.SPIN:
				{
					switch (triggeredColliderType)
					{
						case GearColliderType.BASE:
								break;

						case GearColliderType.SPIN:
							{

								break;
							}
					}

					break;
				}
		}
	}


	private void OnGameOver()
	{

	}
		
}