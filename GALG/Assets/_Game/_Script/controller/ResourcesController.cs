using UnityEngine;
using System.Collections.Generic;
//using Destructible2D;

public class ResourcesController : Controller
{
	private RCModel 				_RCModel				{ get { return game.model.RCModel; } }
	private GearModel 			_playerModel			{ get { return game.model.playerModel; } }
	private ObstacleFactoryModel 	_obstacleFactoryModel 	{ get { return game.model.obstacleFactoryModel; } }
	private RoadFactoryModel 		_roadFactoryModel 		{ get { return game.model.roadFactoryModel; } }

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.RCStartLoad:
				{

					break;
				}


		}
	}

}
