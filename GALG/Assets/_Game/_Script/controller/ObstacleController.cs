using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Destructible2D;

public class ObstacleController : Controller
{
	private RobotsFactoryModel _obstacleFactoryModel	{ get { return game.model.robotsFactoryModel; } }
	
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameOnStart:
				{
					OnStart ();

					break;
				}

			case N.GearsColliderTriggered____:
				{
					var obstacleView = (ObstacleView)data [0];
					var collisionPoint = (Vector2)data [1];

					BreakObstacle (obstacleView, collisionPoint);
					break;
				}


			case N.ObstacleInvisible:
				{
					ObstacleView obstacle = (ObstacleView)data [0];

					CheckRecycleObstacle (obstacle);

					break;
				}
		}
	}

	private void OnStart()
	{

	}

	public void BreakObstacle(ObstacleView obstacleView, Vector2 collisionPoint)
	{
		var obstacleModel = game.model.robotsFactoryModel.currentModelsDictionary[obstacleView];

		if (!obstacleModel)
		{
			Debug.LogError ("Cant find model");
			return;
		}

		switch (obstacleModel.state)
		{
			case ObstacleState.HARD:
				{
					Notify(N.GameOver, collisionPoint);

					break;
				}

			case ObstacleState.DESTRUCTIBLE:
				{
					//var obstacleDestructible = obstacleView.GetComponent<D2dDestructible> ();

					obstacleView.gameObject.layer = LayerMask.NameToLayer (GM.instance.destructibleObstaclePieceLayerName);

					//Notify (N.DestructibleBreakEntity___, obstacleDestructible, game.model.destructibleModel.destructibleObstacleFractureCount, collisionPoint);

					break;
				}
			default:
				break;
		}
	}

	private void CheckRecycleObstacle(ObstacleView obstacleView)
	{
		ObstacleModel obstacleModel = _obstacleFactoryModel.currentModelsDictionary [obstacleView];

		switch (obstacleModel.recyclableState)
		{
			case ObstacleRecyclableState.RECYCLABLE:
				{
					StoreObstacleForRecycle (obstacleView);
					break;
				}

			case ObstacleRecyclableState.NON_RECYCLABLE:
				{
					DeleteObstacle (obstacleView);
					break;
				}
		}
	}

	private void StoreObstacleForRecycle(ObstacleView obstacleView)
	{
		ObstacleModel obstacleModel = _obstacleFactoryModel.currentModelsDictionary [obstacleView];
		var recyclableDictionary = _obstacleFactoryModel.recyclableObstaclesDictionary;

		recyclableDictionary[obstacleModel.state].Add (obstacleView);

		obstacleView.transform.parent.gameObject.SetActive (false);
	}

	private void DeleteObstacle(ObstacleView obstacleView)
	{
		//Destroy model copy component from factory
		Destroy( _obstacleFactoryModel.currentModelsDictionary[obstacleView] );

		//Delete view from dictionary
		_obstacleFactoryModel.currentModelsDictionary.Remove (obstacleView);

		//Destroy obstacle wrapper
		DestroyImmediate (obstacleView.transform.parent.gameObject);
	}
}
