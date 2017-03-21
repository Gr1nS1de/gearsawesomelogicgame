using UnityEngine;
using System.Collections;

public class UIView : View
{
	public RobotView[]					robotViews					{ get { return _robotViews 					= SearchLocal<RobotView>(					_robotViews,				typeof(RobotView).Name); } }

	private RobotView[]					_robotViews;


}

