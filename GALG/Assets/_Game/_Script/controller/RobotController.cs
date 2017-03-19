using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Destructible2D;

public class RobotController : Controller
{
	private RobotsFactoryModel robotsFactoryModel	{ get { return game.model.robotsFactoryModel; } }
	
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameOnStart:
				{
					OnStart ();

					break;
				}

				
		}
	}

	private void OnStart()
	{

	}



}
