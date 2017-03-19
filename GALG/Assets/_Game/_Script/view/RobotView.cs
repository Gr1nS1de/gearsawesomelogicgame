using UnityEngine;
using System.Collections;
//using DG.Tweening;

public class RobotView : View
{
	public RobotBodyType	BodyType;
	//public bool 			isVisible				{ get { return _isVisible; } 		private set { _isVisible = value; } }
	//private RobotModel	robotModel 				{ get { return game.model.robotsFactoryModel.currentModelsDictionary[this]; } }
	//private Vector3			obstacleSpriteSize		{ get { return obstacleModel.spriteSize; } }
	//private Transform 		_obstacleWrapper;

	//[SerializeField]
	//private bool 			_isVisible 	= true;


	/*
	public void OnInit(Vector3 pathPosition, Quaternion rotation, bool isDownDirection)
	{
		_obstacleWrapper = transform.parent;
		_obstacleWrapper.gameObject.SetActive (true);
		gameObject.SetActive(true);

		PlaceObstacle(pathPosition, rotation, isDownDirection);
	}

	private void PlaceObstacle(Vector3 pathPosition, Quaternion rotation, bool isDownDirection)
	{
		_obstacleWrapper.rotation = rotation;

		pathPosition.z = 5f;
		_obstacleWrapper.position = pathPosition;

		if (game.model.currentRoadModel.width / 2f - obstacleSpriteSize.y < 0.0f)
		{
			Debug.LogError ("Obstacle [" + transform.name + "] sprite height is bigger than half of current road width.");
		}

		isVisible = true;

		//Reset position to outside.
		transform.localPosition += new Vector3 (0f, -obstacleSpriteSize.y * 2f, 0f );

		/*transform.DOLocalMoveY(-obstacleSpriteSize.y, 0.2f).OnComplete(() => {
			StopAllCoroutines();

			StartCoroutine(CheckVisibility());
		});
	}
		
	private IEnumerator CheckVisibility () 
	{
		while (true)
		{
			Vector3 screenPoint = Camera.main.WorldToViewportPoint (transform.position);

			if (obstacleModel.spriteForVisible.isVisible)
			{
				if (!isVisible)
					isVisible = true;
			}
			else
			{
				if (isVisible)
				{
					isVisible = false;

					yield return new WaitForSeconds(2f);

					Notify (N.ObstacleInvisible, this);

					break;
				}
			}

			yield return null;	
		}
	}
*/
}
