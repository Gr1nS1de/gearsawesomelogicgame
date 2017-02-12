using UnityEngine;
using System.Collections;

public enum ObstacleState
{
	HARD,
	DESTRUCTIBLE
}

public enum ObstacleRecyclableState
{
	RECYCLABLE,
	NON_RECYCLABLE
}

public class ObstacleModel : Model
{

	public ObstacleModel(){}

	public ObstacleState			state				{ get { return _state; } }
	public ObstacleRecyclableState 	recyclableState		{ get { return _recyclableState; } }
	public ObstacleView				obstacleView		{ get { return _obstacleView;} 	set { _obstacleView = value;}}
	public Vector3					spriteSize			{ get { return _spriteSize; } 	set { _spriteSize = value; } }
	public SpriteRenderer			spriteForVisible	{ get { return _spriteForVisible;} set { _spriteForVisible = value;}}

	[SerializeField]
	private ObstacleState			_state;
	[SerializeField]
	private ObstacleRecyclableState _recyclableState;
	private ObstacleView			_obstacleView;
	[SerializeField]
	private Vector3					_spriteSize;
	private SpriteRenderer			_spriteForVisible;

}
