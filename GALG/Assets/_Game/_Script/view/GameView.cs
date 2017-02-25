using UnityEngine;
using System.Collections;

public class GameView : View
{
	public CameraView					cameraView					{ get { return _cameralView 				= SearchLocal<CameraView>(					_cameralView,				typeof(CameraView).Name ); } }
	public RoadView						currentRoadView				{ get { return _currentRoadView 			= game.model.gearsFactoryModel.roadTemplates[(int)game.model.currentRoad - 1]; } }
	public ObstacleView					obstacleView				{ get { return _obstacleView 				= SearchLocal<ObstacleView>(				_obstacleView,				typeof(ObstacleView).Name); } }
	public GearView						playerView					{ get { return _playerView 					= SearchLocal<GearView>(					_playerView,				typeof(GearView).Name); } }
	public GearLightView				gearLightView				{ get { return _gearLightView 	= SearchLocal<GearLightView>(	_gearLightView,	typeof(GearLightView).Name ); } }
	//public GearColliderView				playerSpriteView			{ get { return _playerSpriteView 			= SearchLocal<GearColliderView>(			_playerSpriteView,			typeof(GearColliderView).Name); } }
	public PlayerTraceView				playerTraceView				{ get { return _playerTraceView 			= SearchLocal<PlayerTraceView>(				_playerTraceView,			typeof(PlayerTraceView).Name); } }
	public ObjectsPoolView				objectsPoolView				{ get { return _objectsPoolView				= SearchLocal<ObjectsPoolView>(				_objectsPoolView,			typeof(ObjectsPoolView).Name);}}
	//public RotatableComponent			rotatableComponent			{ get { return _rotatableComponent 			= SearchLocal<RotatableComponent>(			_rotatableComponent,		typeof(RotatableComponent).Name); } }

	private CameraView					_cameralView;
	private RoadView					_currentRoadView;
	private ObstacleView				_obstacleView;
	private GearView					_playerView;
	private GearLightView				_gearLightView;
	//private GearColliderView			_playerSpriteView;
	private PlayerTraceView				_playerTraceView;
	private ObjectsPoolView				_objectsPoolView;
	//private RotatableComponent        _rotatableComponent;

}
