﻿using UnityEngine;
using System.Collections.Generic;
//using DG.Tweening;
using UnityEngine.SceneManagement;
//using Destructible2D;

public class GameController : Controller
{
	#region Declare controllers reference
	public CameraController					cameraController				{ get { return _cameraController 			= SearchLocal<CameraController>(			_cameraController,				typeof(CameraController).Name ); } }
	public RoadController					roadController					{ get { return _roadController 				= SearchLocal<RoadController>(				_roadController,				typeof(RoadController).Name ); } }
	public GearsFactoryController			gearsFactoryController			{ get { return _gearsFactoryController 		= SearchLocal<GearsFactoryController>(		_gearsFactoryController,		typeof(GearsFactoryController).Name ); } }
	public GearsInputController				gearsInputController			{ get { return _gearsInputController 		= SearchLocal<GearsInputController>(		_gearsInputController,			typeof(GearsInputController).Name ); } }
	public GearsCollisionController			gearsCollisionController		{ get { return _gearsCollisionController 	= SearchLocal<GearsCollisionController>(	_gearsCollisionController,		typeof(GearsCollisionController).Name ); } }
	public GearsChainController				gearsChainController			{ get { return _gearsChainController 		= SearchLocal<GearsChainController>(		_gearsChainController,			typeof(GearsChainController).Name ); } }
	public GearsVisualController			gearsVisualController			{ get { return _gearsVisualController 		= SearchLocal<GearsVisualController>(		_gearsVisualController,			typeof(GearsVisualController).Name ); } }
	public RobotController					obstacleController				{ get { return _obstacleController			= SearchLocal<RobotController>(			_obstacleController,			typeof(RobotController).Name ); } }
	public RobotsFactoryController			robotsFactoryController			{ get { return _robotsFactoryController 	= SearchLocal<RobotsFactoryController>(		_robotsFactoryController,		typeof(RobotsFactoryController).Name ); } }
	//public DestructibleController			destructibleController			{ get { return _destructibleController 		= SearchLocal<DestructibleController>(		_destructibleController,		typeof(DestructibleController).Name ); } }
	public GearsInputController				playerController				{ get { return _playerController 			= SearchLocal<GearsInputController>(		_playerController,				typeof(GearsInputController).Name ); } }
	public GameSoundController				gameSoundController				{ get { return _gameSoundController			= SearchLocal<GameSoundController>(			_gameSoundController,			typeof(GameSoundController).Name ); } }
	public ResourcesController				resourcesController				{ get { return _resourcesController 		= SearchLocal<ResourcesController>(			_resourcesController,			typeof(ResourcesController).Name ); } }
	public ObjectsPoolController			objectsPoolController			{ get { return _objectsPoolController 		= SearchLocal<ObjectsPoolController> (		_objectsPoolController, 		typeof(ObjectsPoolController).Name);}}

	private CameraController				_cameraController;
	private RoadController					_roadController;
	private GearsFactoryController 			_gearsFactoryController;
	private GearsInputController 			_gearsInputController;
	private GearsCollisionController 		_gearsCollisionController;
	private GearsChainController 			_gearsChainController;
	private GearsVisualController 			_gearsVisualController;
	private RobotController					_obstacleController;
	private RobotsFactoryController 		_robotsFactoryController;
	//private DestructibleController		_destructibleController;
	private GearsInputController			_playerController;
	private GameSoundController				_gameSoundController;
	private ResourcesController				_resourcesController;
	private ObjectsPoolController 			_objectsPoolController;
	#endregion

	private GearModel 					playerModel	{ get { return game.model.playerModel;}}

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameOnStart:
				{
					//PlayerPrefs.DeleteAll ();
					game.model.gameState = GameState.MAIN_MENU;
					OnStart();
					break;
				}

			case N.GamePlayLevel_:
				{
					game.model.gameState = GameState.PLAYING;
					break;
				}


			case N.GearsColliderTriggered_____:
				{
					//var obstacleView = (ObstacleView)data [0];
					//var collisionPoint = (Vector2)data [1];

					break;
				}

			case N.GameOver:
				{
					var collisionPoint = (Vector2)data [0];

					GameOver (collisionPoint);

					game.model.gameState = GameState.GAMEOVER;

					break;
				}
		}
	}

	private void OnStart()
	{
		SetNewGame ();
	}

	void SetNewGame()
	{
		game.model.currentScore = 0;

		//m_PointText.text = _pointScore.ToString();

	}

	public void Add1Score()
	{
		game.model.currentScore++;

		//Notify (N.GameAddScore, 1);

		//m_PointText.text = _pointScore.ToString();

		//_soundManager.PlayTouch();

		//FindObjectOfType<Circle>().DOParticle();
	}
	/*
	public void OnImpactObstacleByPlayer(RobotView obstacleView, Vector2 collisionPoint)
	{
		var obstacleModel = game.model.robotsFactoryModel.currentModelsDictionary[obstacleView];

		if (!obstacleModel)
		{
			Debug.LogError ("Cant find model");
			return;
		}
			
		switch (obstacleModel.bodyType)
		{
			case RobotBodyType.HEAD:
				{
					//obstacleRenderObject.GetComponent<Rigidbody2D> ().isKinematic = true;
					Notify(N.GameOver, collisionPoint);

					break;
				}

			case RobotBodyType.BODY:
				{
					/*
					var obstacleDestructible = obstacleView.GetComponent<D2dDestructible> ();

					Add1Score ();

					obstacleView.gameObject.layer = LayerMask.NameToLayer (GM.instance.destructibleObstaclePieceLayerName);

					Notify (N.DestructibleBreakEntity___, obstacleDestructible, game.model.destructibleModel.destructibleObstacleFractureCount, collisionPoint);

					break;
				}
			default:
				break;
		}
	}
*/
	private void GameOver( Vector2 collisionPoint )
	{
		if (game.model.gameState == GameState.GAMEOVER)
			return;

		//ReportScoreToLeaderboard(point);

		//_player.DesactivateTouchControl();

		//DOTween.KillAll();
		StopAllCoroutines();

		Utils.SetLastScore(game.model.currentScore);

		//playerModel.particleTrace.Stop ();

		//ShowAds();

		//_soundManager.PlayFail();

		ui.controller.OnGameOver(() =>
		{
			ReloadScene();
		});
	}

	private void ReloadScene()
	{

		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadSceneAsync( 0, LoadSceneMode.Single );
		#else
		Application.LoadLevel(Application.loadedLevel);
		#endif
	}



}
