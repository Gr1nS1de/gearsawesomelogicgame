using UnityEngine;
using System.Collections.Generic;
//using DG.Tweening;
using UnityEngine.SceneManagement;
//using Destructible2D;

public class GameController : Controller
{
	#region Declare controllers reference
	public CameraController					cameraController				{ get { return _cameraController 			= SearchLocal<CameraController>(			_cameraController,				typeof(CameraController).Name ); } }
	public RoadController					roadController					{ get { return _roadController 				= SearchLocal<RoadController>(				_roadController,				typeof(RoadController).Name ); } }
	public RoadFactoryController			roadFactoryController			{ get { return _roadFactoryController 		= SearchLocal<RoadFactoryController>(		_roadFactoryController,			typeof(RoadFactoryController).Name ); } }
	public ObstacleController				obstacleController				{ get { return _obstacleController			= SearchLocal<ObstacleController>(			_obstacleController,			typeof(ObstacleController).Name ); } }
	public ObstacleFactoryController		obstacleFactoryController		{ get { return _obstacleFactoryController 	= SearchLocal<ObstacleFactoryController>(	_obstacleFactoryController,		typeof(ObstacleFactoryController).Name ); } }
	//public DestructibleController			destructibleController			{ get { return _destructibleController 		= SearchLocal<DestructibleController>(		_destructibleController,		typeof(DestructibleController).Name ); } }
	public GearsController					playerController				{ get { return _playerController 			= SearchLocal<GearsController>(			_playerController,				typeof(GearsController).Name ); } }
	public GameSoundController				gameSoundController				{ get { return _gameSoundController			= SearchLocal<GameSoundController>(			_gameSoundController,			typeof(GameSoundController).Name ); } }
	public ResourcesController				resourcesController				{ get { return _resourcesController 		= SearchLocal<ResourcesController>(			_resourcesController,			typeof(ResourcesController).Name ); } }
	public ObjectsPoolController			objectsPoolController			{ get { return _objectsPoolController 		= SearchLocal<ObjectsPoolController> (		_objectsPoolController, 		typeof(ObjectsPoolController).Name);}}

	private CameraController				_cameraController;
	private RoadController					_roadController;
	private RoadFactoryController 			_roadFactoryController;
	private ObstacleController				_obstacleController;
	private ObstacleFactoryController 		_obstacleFactoryController;
	//private DestructibleController			_destructibleController;
	private GearsController				_playerController;
	private GameSoundController				_gameSoundController;
	private ResourcesController				_resourcesController;
	private ObjectsPoolController 			_objectsPoolController;
	#endregion

	private GearModel 					playerModel	{ get { return game.model.playerModel;}}

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameStart:
				{
					//PlayerPrefs.DeleteAll ();
					game.model.gameState = GameState.READY;
					OnStart();
					break;
				}

			case N.GamePlay:
				{
					game.model.gameState = GameState.PLAYING;
					break;
				}


			case N.GamePlayerImpactObstacle__:
				{
					var obstacleView = (ObstacleView)data [0];
					var collisionPoint = (Vector2)data [1];

					OnImpactObstacleByPlayer (obstacleView, collisionPoint);
					break;
				}

			case N.GameRoadChangeStart__:
				{
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

		Notify (N.GameAddScore, 1);

		//m_PointText.text = _pointScore.ToString();

		//_soundManager.PlayTouch();

		//FindObjectOfType<Circle>().DOParticle();
	}

	public void OnImpactObstacleByPlayer(ObstacleView obstacleView, Vector2 collisionPoint)
	{
		var obstacleModel = game.model.obstacleFactoryModel.currentModelsDictionary[obstacleView];

		if (!obstacleModel)
		{
			Debug.LogError ("Cant find model");
			return;
		}
			
		switch (obstacleModel.state)
		{
			case ObstacleState.HARD:
				{
					//obstacleRenderObject.GetComponent<Rigidbody2D> ().isKinematic = true;
					Notify(N.GameOver, collisionPoint);

					break;
				}

			case ObstacleState.DESTRUCTIBLE:
				{
					/*
					var obstacleDestructible = obstacleView.GetComponent<D2dDestructible> ();

					Add1Score ();

					obstacleView.gameObject.layer = LayerMask.NameToLayer (GM.instance.destructibleObstaclePieceLayerName);

					Notify (N.DestructibleBreakEntity___, obstacleDestructible, game.model.destructibleModel.destructibleObstacleFractureCount, collisionPoint);
*/
					break;
				}
			default:
				break;
		}
	}

	private void GameOver( Vector2 collisionPoint )
	{
		if (game.model.gameState == GameState.GAMEOVER)
			return;

		//ReportScoreToLeaderboard(point);

		//_player.DesactivateTouchControl();

		//DOTween.KillAll();
		StopAllCoroutines();

		Utils.SetLastScore(game.model.currentScore);

		playerModel.particleTrace.Stop ();

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
