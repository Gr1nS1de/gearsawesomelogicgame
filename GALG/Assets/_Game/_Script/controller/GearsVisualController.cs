using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GearsVisualController : Controller 
{
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return game.model.gearsFactoryModel.gearsDictionary; } }
	private CurrentGearModel				currentGearModel 			{ get { return game.model.currentGearModel; } }

	private Color							_lastGearStatusIndicatorColor;
	private float							_lastGearShadowColorAlpha;

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch (alias)
		{
			case N.GameOnStart:
				{
					OnStart ();

					break;
				}

			case N.OnInputGear___:
				{
					GameObject dragItem = (GameObject)data [0];
					Vector3 inputPoint = (Vector3)data [1];
					FingerMotionPhase gesturePhase = (FingerMotionPhase)data [2];

					switch (gesturePhase)
					{
						case FingerMotionPhase.Started:
							{
								//If just started
								if (dragItem != null)
								{
									SetHighlightCurrentGear (true);
								}

								break;
							}

						case FingerMotionPhase.Ended:
							{
								SetHighlightCurrentGear (false);
								break;
							}
					}

					break;
				}

			case N.GearsColliderTriggered_____:
				{
					UpdateCurrentGearIndicator ();
					break;
				}

		}
	}

	private void OnStart()
	{
	}

	private void UpdateCurrentGearIndicator()
	{
		if (game.model.currentGearModel.collisionsCount == 0)
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
					currentGearModel.gearModel.statusIndicator.DOColor(currentGearModel.gearModel.indicatorSelectedColor, 0.1f); 
					break;
				}

			case GearIndicatorStatus.ERROR:
				{
					currentGearModel.gearModel.statusIndicator.DOColor(currentGearModel.gearModel.indicatorErrorColor, 0.1f); 
					break;
				}
		}
	}

	private void SetHighlightCurrentGear(bool isEnable)
	{

		if (game.view.currentGearView == null)
		{
			Debug.LogError ("Can't highlight. Current gear view == null!");
			return;
		}

		var gearShadow = currentGearModel.gearModel.shadow;
		var shadowColor = gearShadow.color;

		if (isEnable)
		{
			//Setup position for light
			game.view.gearLightView.transform.SetParent (game.view.currentGearView.transform);
			game.view.gearLightView.transform.DOLocalMove (Vector3.zero, 0.2f);

			_lastGearShadowColorAlpha = shadowColor.a;
			_lastGearStatusIndicatorColor = currentGearModel.gearModel.statusIndicator.color;

			shadowColor.a = 0f;
			//gearShadow.transform.rotation = Quaternion.Euler (Vector3.zero);
			gearShadow.color = shadowColor;
			gearShadow.enabled = true;

			gearShadow.DOFade (_lastGearShadowColorAlpha, 0.1f);

			SetCurrentGearIndicator (GearIndicatorStatus.SELECTED);
		}
		else
		{
			currentGearModel.gearModel.shadow.DOFade (0f, 0.1f)
				.OnComplete (() =>
				{
					gearShadow.enabled = false;
					shadowColor.a = _lastGearShadowColorAlpha;
					gearShadow.color = shadowColor;
					gearShadow.transform.rotation = Quaternion.Euler (Vector3.zero);
				});

			//Restore prev indicator color
			currentGearModel.gearModel.statusIndicator.DOColor(_lastGearStatusIndicatorColor, 0.1f); 
		}
	}
}
