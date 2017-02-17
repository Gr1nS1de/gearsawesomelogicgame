using UnityEngine;
using System.Collections;

public class GameView : View
{
	public CameraView					cameraView					{ get { return _cameralView 				= SearchLocal<CameraView>(					_cameralView,				typeof(CameraView).Name ); } }
	public RoadView						currentRoadView				{ get { return _currentRoadView 			= game.model.roadFactoryModel.roadTemplates[(int)game.model.currentRoad - 1]; } }
	public ObstacleView					obstacleView				{ get { return _obstacleView 				= SearchLocal<ObstacleView>(				_obstacleView,				typeof(ObstacleView).Name); } }
	public PlayerView					playerView					{ get { return _playerView 					= SearchLocal<PlayerView>(					_playerView,				typeof(PlayerView).Name); } }
	public PlayerSpriteContainerView	playerSpriteContainerView	{ get { return _playerSpriteContainerView 	= SearchLocal<PlayerSpriteContainerView>(	_playerSpriteContainerView,	typeof(PlayerSpriteContainerView).Name ); } }
	public PlayerSpriteView				playerSpriteView			{ get { return _playerSpriteView 			= SearchLocal<PlayerSpriteView>(			_playerSpriteView,			typeof(PlayerSpriteView).Name); } }
	public PlayerTraceView				playerTraceView				{ get { return _playerTraceView 			= SearchLocal<PlayerTraceView>(				_playerTraceView,			typeof(PlayerTraceView).Name); } }
	public ObjectsPoolView				objectsPoolView				{ get { return _objectsPoolView				= SearchLocal<ObjectsPoolView>(				_objectsPoolView,			typeof(ObjectsPoolView).Name);}}
	//public RotatableComponent			rotatableComponent			{ get { return _rotatableComponent 			= SearchLocal<RotatableComponent>(			_rotatableComponent,		typeof(RotatableComponent).Name); } }

	private CameraView					_cameralView;
	private RoadView					_currentRoadView;
	private ObstacleView				_obstacleView;
	private PlayerView					_playerView;
	private PlayerSpriteContainerView	_playerSpriteContainerView;
	private PlayerSpriteView			_playerSpriteView;
	private PlayerTraceView				_playerTraceView;
	private ObjectsPoolView				_objectsPoolView;
	//private RotatableComponent        _rotatableComponent;

}
