using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RobotPart
{
	ROOT,
	HEAD,
	BODY,
	HAND_L,
	HAND_R,
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

	public Dictionary<RobotPart, RobotView>			robotPartsDictionary	{ get { return _robotPartsDictionary; } }
	public ObstacleRecyclableState 					recyclableState			{ get { return _recyclableState; } }
	public RobotView								obstacleView			{ get { return _obstacleView;} 	set { _obstacleView = value;}}
	public Vector3									spriteSize				{ get { return _spriteSize; } 	set { _spriteSize = value; } }
	public SpriteRenderer							spriteForVisible		{ get { return _spriteForVisible;} set { _spriteForVisible = value;}}

	[SerializeField]
	private Dictionary<RobotPart, RobotView>		_robotPartsDictionary 	= new Dictionary<RobotPart, RobotView>();
	[SerializeField]
	private ObstacleRecyclableState 				_recyclableState;
	private RobotView								_obstacleView;
	[SerializeField]
	private Vector3									_spriteSize;
	private SpriteRenderer							_spriteForVisible;

}
