﻿using UnityEngine;
using System.Collections;

public class N : MonoBehaviour
{
	#region Player notifications
	public const string PlayerJumpStart				= "player.jump.start";
	public const string PlayerJumpFinish			= "player.jump.finish";
	public const string PlayerDie					= "player.die";
	#endregion

	#region Obstacle notifications
	public const string ObstacleInvisible			= "obstacle.invisible";
	#endregion

	#region Game notifications
	public const string GameOnStart					= "game.onstart";
	public const string GamePlayLevel_				= "game.play.level";
	public const string GamePause					= "game.pause";
	public const string GameOver					= "game.over";

	public const string GearsColliderTriggered___	= "gears_collider.triggered";
	/*
	public const string GamePlayerGetScoreItem		= "game.player.get.score_item";
	public const string GamePlayerPlacedOnRoad		= "game.player.placed_on_road";

	public const string GameAddScore				= "game.add.score";
	public const string GameRoadChangeStart__		= "game.road.change.start";
	public const string GameRoadChangeEnd 			= "game.road.change.end";
	public const string GameRoadsPlaced				= "game.roads.placed"*/
	#endregion

	#region UI notifications
	public const string UIClickedStart 				= "ui.clicked.start";
	#endregion

	#region Destructible notifications
	public const string DestructibleBreakEntity___ 	= "destructible.break.entity";
	#endregion

	#region Input controller notification
	public const string InputOnDrag___				= "input.on.drag";
	#endregion

	#region ResourcesController notifications
	public const string RCStartLoad					= "rc.start.load";
	public const string RCResetRoadModelTemplate	= "rc.reset.road.model.template";
	#endregion
}
