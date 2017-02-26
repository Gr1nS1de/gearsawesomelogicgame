using UnityEngine;
using System.Collections.Generic;

public enum Road
{
	GINGERBREAD_MAN = 1,
	SNOWMAN			= 2
}

public class GearsFactoryModel : Model
{
	public RoadView[]						roadTemplates			{ get { return _roadTemplates; } set { _roadTemplates = value;} }
	public float							roadsGapLength			{ get { return _roadsGapLength; } }
	public Dictionary<GearView, GearModel> 	currentGearsDictionary 	{ get { return _currentGearsDictionary; } }

	[SerializeField]
	private RoadView[]						_roadTemplates 			= new RoadView[System.Enum.GetNames(typeof(Road)).Length];
	[SerializeField]
	private float							_roadsGapLength;
	public Dictionary<GearView, GearModel> 	_currentGearsDictionary = new Dictionary<GearView, GearModel>();
}
