using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GearsVisualController : Controller 
{
	private GearView 						currentGearView 			{ get { return game.view.currentGearView; } set { game.view.currentGearView = value; } }
	private GearModel 						currentGearModel 			{ get { return gearsDictionary[currentGearView]; } }
	private SelectedGearModel				selectedGearModel			{ get { return game.model.selectedGearModel;}}
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return game.model.gearsFactoryModel.gearsDictionary; } }

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

			case N.OnGearSelected_:
				{
					GearView gearView = (GearView)data [0];

					SetHighlightGear (gearView, true);

					break;
				}


			case N.OnGearDeselected_:
				{
					GearView gearView = (GearView)data [0];

					SetHighlightGear (gearView, false);
					
					break;
				}

			case N.OnCurrentGearError_:
				{
					bool isError = (bool)data [0];

					UpdateCurrentGearIndicator (isError);
					UpdateGearLayer (isError);

					break;
				}

		}
	}

	private void OnStart()
	{
	}

	private void UpdateCurrentGearIndicator(bool isError)
	{
		if (!isError)
		{
			SetCurrentGearIndicatorState (GearIndicatorState.SELECTED);
		}
		else
		{
			SetCurrentGearIndicatorState (GearIndicatorState.ERROR);
		}
	}

	private void UpdateGearLayer(bool isError)
	{
		if (!isError)
		{

		}else
		Utils.SetGearLayer (currentGearView, GearLayer.ERROR);
	}

	private void SetCurrentGearIndicatorState(GearIndicatorState state)
	{

		switch (state)
		{
			case GearIndicatorState.DEFAULT:
				{
					selectedGearModel.gearModel.statusIndicator.DOColor(selectedGearModel.gearModel.indicatorDefaultColor, 0.1f); 
					break;
				}

			case GearIndicatorState.SELECTED:
				{
					selectedGearModel.gearModel.statusIndicator.DOColor(selectedGearModel.gearModel.indicatorSelectedColor, 0.1f); 
					break;
				}

			case GearIndicatorState.ERROR:
				{
					selectedGearModel.gearModel.statusIndicator.DOColor(selectedGearModel.gearModel.indicatorErrorColor, 0.1f); 
					break;
				}
		}

		currentGearModel.gearIndicatorState = state;
	}

	private void SetHighlightGear( GearView currentGear, bool isEnable)
	{
		
		if (currentGear == null)
		{
			Debug.LogError ("Can't highlight. Current gear view == null! isEnable = " + isEnable);
			return;
		}

		var gearShadow = selectedGearModel.gearModel.shadow;
		var shadowColor = gearShadow.color;

		if (isEnable)
		{
			//Setup position for light
			//game.view.gearLightView.transform.SetParent (currentGearView.transform);
			//game.view.gearLightView.transform.DOLocalMove (Vector3.zero, 0.2f);

			_lastGearShadowColorAlpha = shadowColor.a;
			_lastGearStatusIndicatorColor = selectedGearModel.gearModel.statusIndicator.color;

			shadowColor.a = 0f;
			//gearShadow.transform.rotation = Quaternion.Euler (Vector3.zero);
			gearShadow.color = shadowColor;
			gearShadow.enabled = true;

			gearShadow.DOFade (_lastGearShadowColorAlpha, 0.1f);

			SetCurrentGearIndicatorState (GearIndicatorState.SELECTED);
		}
		else
		{
			selectedGearModel.gearModel.shadow.DOFade (0f, 0.1f)
				.OnComplete (() =>
				{
					gearShadow.enabled = false;
					shadowColor.a = _lastGearShadowColorAlpha;
					gearShadow.color = shadowColor;
					gearShadow.transform.rotation = Quaternion.Euler (Vector3.zero);
				});

			//Restore default indicator color
			SetCurrentGearIndicatorState (GearIndicatorState.DEFAULT);
		}
	}
}
