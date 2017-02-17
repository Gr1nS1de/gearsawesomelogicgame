using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GearsController : Controller
{
	private GearModel 	currentGearModel 		{ get { return game.model.playerModel; } }
	private GearView	currentGearView			{ get { return game.view.playerView; } } 

	private bool 		isCanMove				= true;

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
					Vector2 inputPoint = (Vector2)data [1];
					ContinuousGesturePhase gesturePhase = (ContinuousGesturePhase)data [2];

					GearView gearElement = dragItem.GetComponent<GearView> ();

					Debug.LogError ("Dragged " + dragItem.name+" input point " + inputPoint + " ");

					if(gearElement)
						OnDragGear (gearElement, inputPoint, gesturePhase);

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

	private void OnDragGear (GearView selectedGear, Vector2 inputPoint, ContinuousGesturePhase gesturePhase)
	{
		Debug.Log ("Drag gear = " + selectedGear.gameObject.name + " point " + inputPoint);

		Vector3 point = new Vector3 (inputPoint.x, inputPoint.y, 0f);
		switch (gesturePhase)
		{
			case ContinuousGesturePhase.Started:
				{
					isCanMove = false;

					selectedGear.transform.DOMove ((Vector3)inputPoint, 0.1f)
						.SetUpdate(UpdateType.Normal)
						.SetEase (Ease.InOutCubic)
						.OnComplete (() =>
						{
							isCanMove = true;
						});
					break;
				}

			case ContinuousGesturePhase.Updated:
				{
					if (isCanMove)
					{
						selectedGear.transform.position = inputPoint;
					}
					break;
				}

			case ContinuousGesturePhase.Ended:
				{
					isCanMove = false;
					break;
				}
		}

	}

	private void OnGameOver()
	{

	}
		
}