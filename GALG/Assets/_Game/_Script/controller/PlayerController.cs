using UnityEngine;
using System.Collections.Generic;
//using DG.Tweening;

public class PlayerController : Controller
{
	private PlayerModel 	playerModel 		{ get { return game.model.playerModel; } }
	private PlayerView		playerView			{ get { return game.view.playerView; } } 

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					OnStart();

					break;
				}


			case N.InputOnTouchDown:
				{
					//PlayerJump ();

					break;
				}

			case N.GameOver_:
				{
					Vector2 collisionPoint = (Vector2)data [0];

					OnGameOver ();

					break;
				}
		}
	}

	private void OnStart()
	{
	}

	private void OnGameOver()
	{

	}
		
}