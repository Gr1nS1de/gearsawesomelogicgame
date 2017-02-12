using UnityEngine;
using System.Collections;
//using DG.Tweening;

public enum GameState
{
	READY,
	PLAYING,
	GAMEOVER
}

public class GameModel : Model
{

	#region Game model
	public GameState					gameState				{ get { return _gameState; } 		set { _gameState 	= value; } }
	public int							currentScore			{ get { return _currentScore; } 	set { _currentScore = value; } }
	public Road							currentRoad				{ get { return _currentRoad;}		set { _prevRoad = _currentRoad; Notify(N.RCResetRoadModelTemplate, _currentRoad); _currentRoad = value; Notify (N.GameRoadChangeStart__, _prevRoad, _currentRoad); } }
	public Road							prevRoad				{ get { return _prevRoad;}}
	public RoadModel					currentRoadModel		{ get { return _currentRoadModel			= SearchLocal<RoadModel>(					_currentRoadModel,			typeof(RoadModel).Name); } }
//	public Vector3[]					currentRoadWaypoints	{ get { return game.model.currentRoadModel.roadTweenPath.GetTween ().PathGetDrawPoints ();}}

	[SerializeField]
	private GameState					_gameState 				= GameState.READY;
	[SerializeField]
	private int 						_currentScore;
	[SerializeField]
	private Road						_currentRoad;
	private RoadModel 					_currentRoadModel;	
	private Road 						_prevRoad;
	#endregion

	#region Declare models reference
	public CameraModel					cameraModel				{ get { return _cameraModel 				= SearchLocal<CameraModel>(					_cameraModel,				typeof(CameraModel).Name); } }
	public RoadFactoryModel				roadFactoryModel		{ get { return _roadFactoryModel			= SearchLocal<RoadFactoryModel>(			_roadFactoryModel,			typeof(RoadFactoryModel).Name ); } }
	public ObstacleFactoryModel			obstacleFactoryModel	{ get { return _obstacleFactoryModel 		= SearchLocal<ObstacleFactoryModel>(		_obstacleFactoryModel,		typeof(ObstacleFactoryModel).Name ); } }
	public DestructibleModel			destructibleModel		{ get { return _destructibleModel 			= SearchLocal<DestructibleModel>( 			_destructibleModel, 		typeof(DestructibleModel).Name ); } }
	public PlayerModel					playerModel				{ get { return _playerModel 				= SearchLocal<PlayerModel>(					_playerModel,				typeof(PlayerModel).Name ); } }
	public GameSoundModel				soundModel				{ get { return _soundModel 					= SearchLocal<GameSoundModel>(				_soundModel,				typeof(GameSoundModel).Name ); } }
	public RCModel						RCModel					{ get { return _RCModel 					= SearchLocal<RCModel>(						_RCModel,					typeof(RCModel).Name ); } }
	public ObjectsPoolModel				objectsPoolModel		{ get { return _objectsPoolModel			= SearchLocal<ObjectsPoolModel>(			_objectsPoolModel,			typeof(ObjectsPoolModel).Name );}}

	private CameraModel					_cameraModel;
	private RoadFactoryModel			_roadFactoryModel;
	private ObstacleFactoryModel		_obstacleFactoryModel;
	private DestructibleModel   		_destructibleModel;
	private PlayerModel					_playerModel;
	private GameSoundModel				_soundModel;
	private RCModel						_RCModel;
	private ObjectsPoolModel			_objectsPoolModel;
	#endregion
}
	