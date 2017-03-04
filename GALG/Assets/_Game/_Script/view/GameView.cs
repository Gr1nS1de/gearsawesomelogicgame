using UnityEngine;
using System.Collections;

public class GameView : View
{
	public CameraView					cameraView					{ get { return _cameralView 				= SearchLocal<CameraView>(					_cameralView,				typeof(CameraView).Name ); } }
	public GameGearsContainerView		gameGearsContainer			{ get { return _gameGearsContainer 			= SearchLocal<GameGearsContainerView>(		_gameGearsContainer,		typeof(GameGearsContainerView).Name ); } }
	public PlayerGearsContainerView		playerGearsContainer		{ get { return _playerGearsContainer		= SearchLocal<PlayerGearsContainerView>(	_playerGearsContainer,		typeof(PlayerGearsContainerView).Name); } }
	public GearLightView				gearLightView				{ get { return _gearLightView 				= SearchLocal<GearLightView>(				_gearLightView,				typeof(GearLightView).Name ); } }
	//public GearColliderView			playerSpriteView			{ get { return _playerSpriteView 			= SearchLocal<GearColliderView>(			_playerSpriteView,			typeof(GearColliderView).Name); } }
	//public PlayerTraceView			playerTraceView				{ get { return _playerTraceView 			= SearchLocal<PlayerTraceView>(				_playerTraceView,			typeof(PlayerTraceView).Name); } }
	public ObjectsPoolView				objectsPoolView				{ get { return _objectsPoolView				= SearchLocal<ObjectsPoolView>(				_objectsPoolView,			typeof(ObjectsPoolView).Name);}}
	//public RotatableComponent			rotatableComponent			{ get { return _rotatableComponent 			= SearchLocal<RotatableComponent>(			_rotatableComponent,		typeof(RotatableComponent).Name); } }
	public GearView						currentGearView				{ get { return _currentGearView;}	set { _currentGearView = value;} }

	private CameraView					_cameralView;
	private GameGearsContainerView		_gameGearsContainer;
	private PlayerGearsContainerView	_playerGearsContainer;
	//private GearView					_playerView;
	private GearLightView				_gearLightView;
	//private GearColliderView			_playerSpriteView;
	private PlayerTraceView				_playerTraceView;
	private ObjectsPoolView				_objectsPoolView;
	//private RotatableComponent        _rotatableComponent;
	private GearView 					_currentGearView;	

}
