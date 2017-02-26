using UnityEngine;
using System.Collections.Generic;

public enum Road
{
	GINGERBREAD_MAN = 1,
	SNOWMAN			= 2
}

public class GearsFactoryModel : Model
{
	//public RoadView[]						roadTemplates			{ get { return _roadTemplates; } set { _roadTemplates = value;} }
	public List<GearView>					loadedGears				{ get { return _loadedGears; } }
	public Dictionary<GearView, GearModel> 	gearsDictionary 		{ get { return _gearsDictionary; } }

	//[SerializeField]
	//private RoadView[]					_roadTemplates 			= new RoadView[System.Enum.GetNames(typeof(Road)).Length];
	[SerializeField]
	private List<GearView>					_loadedGears			= new List<GearView>();
	public Dictionary<GearView, GearModel> 	_gearsDictionary		= new Dictionary<GearView, GearModel>();
}
