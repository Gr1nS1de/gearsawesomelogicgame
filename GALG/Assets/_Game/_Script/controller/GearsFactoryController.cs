using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DG.Tweening;

public class GearsFactoryController : Controller
{
	private GearsFactoryModel 	_gearsFactoryModel	{ get { return game.model.gearsFactoryModel; } }

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
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
	/*
	public RoadModel InitRoads()
	{
		RoadModel currentGameRoadModelCopy = null;

		foreach(RoadView roadView in _gearsFactoryModel.roadTemplates)
		{
			RoadModel roadModel = roadView.GetComponent<RoadModel> ();

			if (roadModel.alias == game.model.currentRoad)
			{
				currentGameRoadModelCopy = roadModel.GetCopyOf<RoadModel> (roadModel);

				Destroy (roadModel);
			}

			var roadsContainerPosition = GM.instance.RoadContainer.transform.position;

			roadsContainerPosition.x = -(_gearsFactoryModel.roadsGapLength * ( (int)game.model.currentRoad - 1));

			GM.instance.RoadContainer.transform.position = roadsContainerPosition;

		}

		game.model.currentRoadModel.GetCopyOf<RoadModel>(currentGameRoadModelCopy);

		return currentGameRoadModelCopy;
	}
		*/
}
	