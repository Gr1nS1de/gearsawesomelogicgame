using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using Destructible2D;
//using DG.Tweening;

public enum GearType
{
	PLAYER_GEAR	= 0,
	MOTOR_GEAR	= 1,
	IDLE_GEAR	= 2
}

public enum GearSizeType
{
	VERY_LITTLE = 0,
	LITTLE		= 1,
	MEDIUM		= 2,
	BIG			= 3,
	VERY_BIG	= 4
}

public enum GearColliderType
{
	BASE,
	SPIN
}
	
public class GearModel : Model
{

	public GearType						gearType				{ get { return _gearType; } }
	public GearSizeType					gearSizeType			{ get { return _gearSizeType; } }	
	public SpriteRenderer				shadow					{ get { return _shadow; }  /*playerDestructible.ReplaceWith( _currentSprite );*/ }
	public SpriteRenderer				statusIndicator			{ get { return _statusIndicator; } }
	public Color						selectedIndicatorColor	{ get { return _selectedIndicatorColor; } }
	//public float						sfLightDuration			{ get { return m_LightDuration; } }
	//public float						deathDuration			{ get { return _deathDuration; } }
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
	private SpriteRenderer				_shadow;
	[SerializeField]
	private SpriteRenderer				_statusIndicator;
	[SerializeField]
	private Color						_selectedIndicatorColor;
//	private float						m_LightDuration;
	//[SerializeField]
	//private float						_deathDuration 	= 3f;
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