using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using Destructible2D;
//using DG.Tweening;

public enum GearType
{
	PLAYER_GEAR		= 0,
	MOTOR_GEAR		= 1,
	CHECKPOINT_GEAR	= 2,
	TEST 			= 3
}

public enum GearSizeType
{
	VERY_LITTLE = 0,
	LITTLE		= 1,
	MEDIUM		= 2,
	BIG			= 3,
	VERY_BIG	= 4
}

public enum GearPositionState
{
	DEFAULT,
	CONNECTED
}

public enum GearIndicatorState
{
	DEFAULT		= 0,
	SELECTED	= 1,
	ERROR		= 2
}

public enum GearColliderType
{
	BASE,
	SPIN
}

public enum GearLayer
{
	PLAYER,
	SELECTED,
	SELECTED_CONNECTED,
	CONNECTED,
	ERROR
}
	
public class GearModel : Model
{
	public GearType						gearType				{ get { return _gearType; } }
	public GearSizeType					gearSizeType			{ get { return _gearSizeType; } }	
	public GearPositionState			gearPositionState		{ get { return _gearPositionState; } set { _gearPositionState = value;} }	
	public GearIndicatorState			gearIndicatorState		{ get { return _gearIndicatorState; } set { _gearIndicatorState = value;} }	
	public SpriteRenderer				shadow					{ get { return _shadow; }  /*playerDestructible.ReplaceWith( _currentSprite );*/ }
	public SpriteRenderer				statusIndicator			{ get { return _statusIndicator; } }
	public Color						indicatorDefaultColor	{ get { return _indicatorDefaultColor; } }
	public Color						indicatorSelectedColor	{ get { return _indicatorSelectedColor; } }
	public Color						indicatorErrorColor		{ get { return _indicatorErrorColor; } }
	public int							teethCount				{ get { return _teethCount; } }
	public bool 						isRotateRight			{ get; set; }	
	//public PlayerPositionState		positionState			{ get { return _positionState; } 		set { _positionState = value; } }
	//public float 						jumpWidth				{ get { return _jumpWidth 				= game.model.currentRoadModel.width / 2f - currentSprite.bounds.size.x * 0.5f * game.view.playerSpriteView.transform.localScale.x; } }
	//public float						jumpDuration			{ get { return _jumpDuration;} 			set { _jumpDuration = value; }}
	//public float						pathDuration			{ get { return game.model.currentRoadModel.pathDuration; } }
	//public float						breakForce				{ get { return _breakForce; } }
	//public ParticleSystem				particleTrace			{ get { return game.view.playerTraceView.GetComponent<ParticleSystem> ();}}
	//public Tweener					playerPath				{ get { return _playerPath;} 			set { _playerPath = value;}}
	//public int							playerPathWPIndex		{ get { return 	_playerPathWPIndex;} 	set { _playerPathWPIndex = value; }}
	//public D2dDestructible			playerDestructible		{ get { return game.view.playerSpriteView.GetComponent<D2dDestructible> ();}}

	[SerializeField]
	private GearType					_gearType;
	[SerializeField]
	private GearSizeType				_gearSizeType;
	[SerializeField]
	private GearPositionState			_gearPositionState;
	[SerializeField]
	private GearIndicatorState			_gearIndicatorState;
	[SerializeField]
	private SpriteRenderer				_shadow;
	[SerializeField]
	private SpriteRenderer				_statusIndicator;
	[SerializeField]
	private Color						_indicatorDefaultColor;
	[SerializeField]
	private Color						_indicatorSelectedColor;
	[SerializeField]
	private Color						_indicatorErrorColor;
	[SerializeField]
	private int							_teethCount;
	//[SerializeField]
	//private PlayerPositionState 		_positionState;
	//private float						_jumpWidth;
	//[SerializeField]
	//private float						_jumpDuration	= 0.2f;
	//[SerializeField]
	//private float						_pathDuration	= 10f;
	//[SerializeField]
	//private float						_breakForce		= 100f;
	//private Tweener 					_playerPath;
	//private int							_playerPathWPIndex;
}
	
/*
public enum PlayerPositionState
{
	ON_CIRCLE,
	OUT_CIRCLE
}*/