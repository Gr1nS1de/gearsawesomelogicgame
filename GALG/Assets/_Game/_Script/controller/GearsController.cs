﻿using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GearsController : Controller
{
	private GearModel 	currentGearModel 		{ get { return game.model.playerModel; } }

	private GearView	_currentGearView; 
	private Vector3		_selectedPoint;
	private bool 		_isCanMoveFlag				= false;

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
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

					//Setup position for light
					game.view.gearLightView.transform.SetParent (selectedGear.transform);
					game.view.gearLightView.transform.DOLocalMove (Vector3.zero, 0.2f);

					_currentGearView = selectedGear;
					_currentGearView.gameObject.layer = LayerMask.NameToLayer ("SelectedGear");

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
					if (_currentGearView)
					{
						gearPosition.z = -1f;
						_currentGearView.transform.position = gearPosition;
						_currentGearView.gameObject.layer = LayerMask.NameToLayer ("PlayerGear");
						_currentGearView = null;
						DOTween.Kill ("START_MOVE");
					}
					break;
				}
		}

	}


	private void OnGameOver()
	{

	}
		
}