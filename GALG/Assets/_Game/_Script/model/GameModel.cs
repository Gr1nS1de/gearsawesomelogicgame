using UnityEngine;
using System.Collections;
//using DG.Tweening;

public enum GameState
{
	MAIN_MENU,
	PLAYING,
	GAMEOVER
}

public class GameModel : Model
{

	#region Game model
	public GameState					gameState				{ get { return _gameState; } 		set { _gameState 	= value; } }
	public int							currentScore			{ get { return _currentScore; } 	set { _currentScore = value; } }
	public Road							currentRoad				{ get { return _currentRoad;}		set { _currentRoad = value;} }
	public Road							prevRoad				{ get { return _prevRoad;}}

	[SerializeField]
	private GameState					_gameState 				= GameState.MAIN_MENU;
	[SerializeField]
	private int 						_currentScore;
	[SerializeField]
	private Road						_currentRoad;
	private Road 						_prevRoad;
	#endregion

	#region Declare models reference
	public CameraModel					cameraModel				{ get { return _cameraModel 				= SearchLocal<CameraModel>(					_cameraModel,				typeof(CameraModel).Name); } }
	public GearsFactoryModel			gearsFactoryModel		{ get { return _gearsFactoryModel			= SearchLocal<GearsFactoryModel>(			_gearsFactoryModel,			typeof(GearsFactoryModel).Name ); } }
	public RobotsFactoryModel			robotsFactoryModel		{ get { return _robotsFactoryModel 			= SearchLocal<RobotsFactoryModel>(			_robotsFactoryModel,		typeof(RobotsFactoryModel).Name ); } }
	public DestructibleModel			destructibleModel		{ get { return _destructibleModel 			= SearchLocal<DestructibleModel>( 			_destructibleModel, 		typeof(DestructibleModel).Name ); } }
	public GearModel					playerModel				{ get { return _playerModel 				= SearchLocal<GearModel>(					_playerModel,				typeof(GearModel).Name ); } }
	public GameSoundModel				soundModel				{ get { return _soundModel 					= SearchLocal<GameSoundModel>(				_soundModel,				typeof(GameSoundModel).Name ); } }
	public RCModel						RCModel					{ get { return _RCModel 					= SearchLocal<RCModel>(						_RCModel,					typeof(RCModel).Name ); } }
	public ObjectsPoolModel				objectsPoolModel		{ get { return _objectsPoolModel			= SearchLocal<ObjectsPoolModel>(			_objectsPoolModel,			typeof(ObjectsPoolModel).Name );}}
	public CurrentGearModel				currentGearModel		{ get { return _currentGearModel 			= SearchLocal<CurrentGearModel>(			_currentGearModel,			typeof(CurrentGearModel).Name); } }


	private CameraModel					_cameraModel;
	private GearsFactoryModel			_gearsFactoryModel;
	private RobotsFactoryModel			_robotsFactoryModel;
	private DestructibleModel   		_destructibleModel;
	private GearModel					_playerModel;
	private GameSoundModel				_soundModel;
	private RCModel						_RCModel;
	private ObjectsPoolModel			_objectsPoolModel;
	private CurrentGearModel			_currentGearModel;
	#endregion
}
	