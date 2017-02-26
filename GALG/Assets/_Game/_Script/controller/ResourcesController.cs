using UnityEngine;
using System.Collections.Generic;
//using Destructible2D;

public class ResourcesController : Controller
{
	private RCModel 				_RCModel				{ get { return game.model.RCModel; } }
	private GearModel 				_playerModel			{ get { return game.model.playerModel; } }
	private RobotsFactoryModel 		_robotsFactoryModel 	{ get { return game.model.robotsFactoryModel; } }
	private GearsFactoryModel 		_gearsFactoryModel 		{ get { return game.model.gearsFactoryModel; } }

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
