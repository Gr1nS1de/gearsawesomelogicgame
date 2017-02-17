using UnityEngine;
using System.Collections.Generic;
//using DG.Tweening;

public class GearsController : Controller
{
	private GearModel 	currentGearModel 		{ get { return game.model.playerModel; } }
	private GearView	currentGearView			{ get { return game.view.playerView; } } 

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();

					break;
				}


			case N.InputOnDrag__:
				{
					GameObject dragItem = (GameObject)data [0];
					Vector2 inputPoint = (Vector2)data [1];
					GearView gearElement = dragItem.GetComponent<GearView> ();

					if(gearElement)
						OnDragGear (gearElement, inputPoint);

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

	private void OnDragGear (GearView dragItem, Vector2 inputPoint)
	{

	}

	private void OnGameOver()
	{

	}
		
}