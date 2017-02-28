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
	private Color							_lastGearStatusIndicatorColor;
	private float							_lastGearShadowColorAlpha;

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

			case N.GearsColliderTriggered_____:
				{
					GearView triggerGear = (GearView)data [0];
					GearView triggeredGear = (GearView)data [1];
					GearColliderView triggerColliderView = (GearColliderView)data [2];
					GearColliderView triggeredColliderView = (GearColliderView)data [3];
					bool isEnterCollision = (bool)data [4];

					if (triggerGear != _currentGearView)
					{
						Debug.LogError ("Trigger gear is not current selected!");
						return;
					}

					if (isEnterCollision)
						OnGearsEnterCollised (triggerGear, triggeredGear, triggerColliderView, triggeredColliderView);
					else
						OnGearsExitCollised (triggerGear, triggeredGear, triggerColliderView, triggeredColliderView);

					UpdateCurrentGearIndicator ();
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

					gearPosition.z = -2;

					selectedGear.transform.position = gearPosition;

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

		SetEnableSelectedGearHighlight (true);
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

			_currentGearView.transform.DOMove (gearPosition, 0.2f)
				.OnComplete (() =>
			{
				ResetCurrentGear ();
			});

			SetEnableSelectedGearHighlight (false);
			
		}
	}

	private void UpdateCurrentGearIndicator()
	{
		if (_baseCollisionsCount == 0)
		{
			SetCurrentGearIndicator (GearIndicatorStatus.SELECTED);
		}
		else
		{
			SetCurrentGearIndicator (GearIndicatorStatus.ERROR);
		}
	}

	private void SetCurrentGearIndicator(GearIndicatorStatus status)
	{
		switch (status)
		{
			case GearIndicatorStatus.SELECTED:
				{
					currentGearModel.statusIndicator.DOColor(currentGearModel.indicatorSelectedColor, 0.1f); 
					break;
				}

			case GearIndicatorStatus.ERROR:
				{
					currentGearModel.statusIndicator.DOColor(currentGearModel.indicatorErrorColor, 0.1f); 
					break;
				}
		}
	}

	private void ResetCurrentGear()
	{
		_currentGearView.gameObject.layer = LayerMask.NameToLayer ("PlayerGear");
		_currentGearView = null;

		_baseCollisionsCount = 0;

		_isGearPositionCorrect = true;
	}

	private void SetEnableSelectedGearHighlight(bool isEnable)
	{
		var shadowColor = currentGearModel.shadow.color;

		if (isEnable)
		{
			_lastGearShadowColorAlpha = shadowColor.a;
			_lastGearStatusIndicatorColor = currentGearModel.statusIndicator.color;

			shadowColor.a = 0f;
			currentGearModel.shadow.color = shadowColor;
			currentGearModel.shadow.enabled = true;

			currentGearModel.shadow.DOFade (_lastGearShadowColorAlpha, 0.1f);

			SetCurrentGearIndicator (GearIndicatorStatus.SELECTED);
		}
		else
		{
			currentGearModel.shadow.DOFade (0f, 0.1f)
				.OnComplete (() =>
			{
					currentGearModel.shadow.enabled = false;
					shadowColor.a = _lastGearShadowColorAlpha;
					currentGearModel.shadow.color = shadowColor;
			});
			
			currentGearModel.statusIndicator.DOColor(_lastGearStatusIndicatorColor, 0.1f); 
		}
	}

	private void OnGearsEnterCollised(GearView triggerGear, GearView triggeredGear, GearColliderView triggerColliderView, GearColliderView triggeredColliderView)
	{
		switch (triggerColliderView.ColliderType)
		{
			case GearColliderType.BASE:
				{
					switch (triggeredColliderView.ColliderType)
					{
						case GearColliderType.BASE:
							break;

						case GearColliderType.SPIN:
							{
								float triggerGearGap = triggerColliderView.ColliderRadius * triggerGear.transform.localScale.x;
								float triggeredGearGap = triggeredColliderView.ColliderRadius * triggeredGear.transform.localScale.x;
								float baseGap = triggerGearGap + triggeredGearGap + 0.01f;

								_baseCollisionsCount++;
								_lastBaseCorrectPoint = triggeredGear.transform.position - Vector3.ClampMagnitude( ( triggeredGear.transform.position - (triggerGear.transform.position + new Vector3(0f, 0f, 1f))) * 100f, baseGap);

								break;
							}
					}
					break;
				}

			case GearColliderType.SPIN:
				{
					switch (triggeredColliderView.ColliderType)
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


	private void OnGearsExitCollised(GearView triggerGear, GearView triggeredGear, GearColliderView triggerColliderView, GearColliderView triggeredColliderView)
	{
		switch (triggerColliderView.ColliderType)
		{
			case GearColliderType.BASE:
				{
					switch (triggeredColliderView.ColliderType)
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
					switch (triggeredColliderView.ColliderType)
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