using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObstacleBundle
{
	public Road 			roadAlias;
	public ObstacleView[]	obstacleTemplates;
}

public class RobotsFactoryModel : Model
{
	public ObstacleView[]									obstacleTemplates				{ get { return _obstacleTemplates = System.Array.Find(obstacleBundles, o => o.roadAlias == game.model.currentRoad).obstacleTemplates; } }
	public ObstacleBundle[]									obstacleBundles					{ get { return _obstacleBundles; } }
	public Dictionary<ObstacleView, ObstacleModel>			currentModelsDictionary			{ get { return _currentModelsDictionary; } }		
	public ObstacleView[]									hardObstacleTemplates			{ get { return System.Array.FindAll(obstacleTemplates, o => o.GetComponent<ObstacleModel>().state == ObstacleState.HARD);}}
	public ObstacleView[]									destructibleObstacleTemplates	{ get { return System.Array.FindAll(obstacleTemplates, o => o.GetComponent<ObstacleModel>().state == ObstacleState.DESTRUCTIBLE);}}
	public Dictionary<ObstacleState, ObstacleView[]>  		templatesByStateDictionary 		{ get { if(!InitTemplatesDictionaryFlag)InitTemplatesDictionary ();  return _templatesByStateDictionary ;} }
	public GameObject										obstaclesDynamicContainer		{ get { return _obstaclesDynamicContainer = _obstaclesDynamicContainer ? _obstaclesDynamicContainer : new GameObject(); } }
	public Dictionary<ObstacleState, List<ObstacleView>>	recyclableObstaclesDictionary 	{ get { if (!InitRecyclableDictionaryFlag)InitRecyclableDictionary (); return _recyclableObstaclesDictionary; } }

	[SerializeField]
	private ObstacleView[]									_obstacleTemplates;
	[SerializeField]
	private ObstacleBundle[]								_obstacleBundles 				= new ObstacleBundle[System.Enum.GetNames(typeof(Road)).Length];
	private Dictionary<ObstacleView, ObstacleModel> 		_currentModelsDictionary 		= new Dictionary<ObstacleView, ObstacleModel>();
	private Dictionary<ObstacleState, ObstacleView[]>		_templatesByStateDictionary		= new Dictionary<ObstacleState, ObstacleView[]>();
	private GameObject										_obstaclesDynamicContainer;
	private Dictionary<ObstacleState, List<ObstacleView>>	_recyclableObstaclesDictionary	= new Dictionary<ObstacleState, List<ObstacleView>>();

	private bool InitTemplatesDictionaryFlag = false;
	private bool InitRecyclableDictionaryFlag = false;

	private void InitTemplatesDictionary()
	{
		
		if(!_templatesByStateDictionary.ContainsKey(ObstacleState.HARD))
			_templatesByStateDictionary.Add (ObstacleState.HARD, hardObstacleTemplates); 

		if(!_templatesByStateDictionary.ContainsKey(ObstacleState.DESTRUCTIBLE))
			_templatesByStateDictionary.Add (ObstacleState.DESTRUCTIBLE, destructibleObstacleTemplates);  

		InitTemplatesDictionaryFlag = true;
	}

	private void InitRecyclableDictionary()
	{
		foreach(ObstacleState obstacleState in System.Enum.GetValues(typeof(ObstacleState)))
		{
			if (!_recyclableObstaclesDictionary.ContainsKey (obstacleState))
				_recyclableObstaclesDictionary.Add (obstacleState, new List<ObstacleView>());
		}

		InitRecyclableDictionaryFlag = true;
	}
}	
