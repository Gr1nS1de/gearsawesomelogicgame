using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObstacleBundle
{
	public Road 			roadAlias;
	public RobotView[]	obstacleTemplates;
}

public class RobotsFactoryModel : Model
{
	/*public RobotView[]									obstacleTemplates				{ get { return _obstacleTemplates = System.Array.Find(obstacleBundles, o => o.roadAlias == game.model.currentRoad).obstacleTemplates; } }
	public ObstacleBundle[]									obstacleBundles					{ get { return _obstacleBundles; } }
	public Dictionary<RobotView, RobotModel>			currentModelsDictionary			{ get { return _currentModelsDictionary; } }		
	public RobotView[]									hardObstacleTemplates			{ get { return System.Array.FindAll(obstacleTemplates, o => o.GetComponent<RobotModel>().bodyType == RobotBodyType.HEAD);}}
	public RobotView[]									destructibleObstacleTemplates	{ get { return System.Array.FindAll(obstacleTemplates, o => o.GetComponent<RobotModel>().bodyType == RobotBodyType.BODY);}}
	public Dictionary<RobotBodyType, RobotView[]>  		templatesByStateDictionary 		{ get { if(!InitTemplatesDictionaryFlag)InitTemplatesDictionary ();  return _templatesByStateDictionary ;} }
	public GameObject										obstaclesDynamicContainer		{ get { return _obstaclesDynamicContainer = _obstaclesDynamicContainer ? _obstaclesDynamicContainer : new GameObject(); } }
	public Dictionary<RobotBodyType, List<RobotView>>	recyclableObstaclesDictionary 	{ get { if (!InitRecyclableDictionaryFlag)InitRecyclableDictionary (); return _recyclableObstaclesDictionary; } }

	[SerializeField]
	private RobotView[]									_obstacleTemplates;
	[SerializeField]
	private ObstacleBundle[]								_obstacleBundles 				= new ObstacleBundle[System.Enum.GetNames(typeof(Road)).Length];
	private Dictionary<RobotView, RobotModel> 		_currentModelsDictionary 		= new Dictionary<RobotView, RobotModel>();
	private Dictionary<RobotBodyType, RobotView[]>		_templatesByStateDictionary		= new Dictionary<RobotBodyType, RobotView[]>();
	private GameObject										_obstaclesDynamicContainer;
	private Dictionary<RobotBodyType, List<RobotView>>	_recyclableObstaclesDictionary	= new Dictionary<RobotBodyType, List<RobotView>>();

	private bool InitTemplatesDictionaryFlag = false;
	private bool InitRecyclableDictionaryFlag = false;

	private void InitTemplatesDictionary()
	{
		
		if(!_templatesByStateDictionary.ContainsKey(RobotBodyType.HEAD))
			_templatesByStateDictionary.Add (RobotBodyType.HEAD, hardObstacleTemplates); 

		if(!_templatesByStateDictionary.ContainsKey(RobotBodyType.BODY))
			_templatesByStateDictionary.Add (RobotBodyType.BODY, destructibleObstacleTemplates);  

		InitTemplatesDictionaryFlag = true;
	}

	private void InitRecyclableDictionary()
	{
		foreach(RobotBodyType obstacleState in System.Enum.GetValues(typeof(RobotBodyType)))
		{
			if (!_recyclableObstaclesDictionary.ContainsKey (obstacleState))
				_recyclableObstaclesDictionary.Add (obstacleState, new List<RobotView>());
		}

		InitRecyclableDictionaryFlag = true;
	}*/
}	
