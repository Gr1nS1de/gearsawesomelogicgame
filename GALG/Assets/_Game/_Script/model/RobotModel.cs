using UnityEngine;
using System.Collections;

public enum RobotBodyType
{
	ROOT,
	HEAD,
	BODY,
	ARM_L,
	ARM_R,
	LEG_L,
	LEG_R
}

public enum ObstacleRecyclableState
{
	RECYCLABLE,
	NON_RECYCLABLE
}

public class RobotModel : Model
{

	public RobotModel(){}

	public RobotBodyType			bodyType			{ get { return _bodyType; } }
	public ObstacleRecyclableState 	recyclableState		{ get { return _recyclableState; } }
	public RobotView				obstacleView		{ get { return _obstacleView;} 	set { _obstacleView = value;}}
	public Vector3					spriteSize			{ get { return _spriteSize; } 	set { _spriteSize = value; } }
	public SpriteRenderer			spriteForVisible	{ get { return _spriteForVisible;} set { _spriteForVisible = value;}}

	[SerializeField]
	private RobotBodyType			_bodyType;
	[SerializeField]
	private ObstacleRecyclableState _recyclableState;
	private RobotView				_obstacleView;
	[SerializeField]
	private Vector3					_spriteSize;
	private SpriteRenderer			_spriteForVisible;

}
