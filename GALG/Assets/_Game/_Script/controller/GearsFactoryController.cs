using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DG.Tweening;

public class GearsFactoryController : Controller
{
	private GearsFactoryModel 				gearsFactoryModel	{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsList			{ get { return game.model.gearsFactoryModel.loadedGears; } }
	private Dictionary<GearView, GearModel> gearsDictionary 	{ get { return game.model.gearsFactoryModel.gearsDictionary; } }

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
		InitLevel ();
	}

	private void InitLevel()
	{
		float screenHeight = Camera.main.orthographicSize * 2.0f;
		float screenWidth = screenHeight * Camera.main.aspect;
		Vector2 screenSize = new Vector2 (screenWidth, screenHeight);

		Debug.Log ("Init level. screen size = " + screenSize);

		InstantiateGears ();
	}

	private void InstantiateGears()
	{
		Debug.Log ("Instantiate "+gearsList.Count + " gears.");
		foreach (GearView gearPrefab in gearsList)
		{
			GearView gearView = null;
			GearModel gearModel = gearsFactoryModel.gameObject.AddComponent<GearModel>();

			gearModel.GetCopyOf<GearModel> (gearPrefab.GetComponent<GearModel> ());

			gearView = Instantiate (gearPrefab) as GearView;

			Destroy (gearView.GetComponent<GearModel> ());

			switch (gearModel.gearType)
			{
				case GearType.PLAYER_GEAR:
					{
						gearView.transform.SetParent (game.view.playerGearsContainer.transform);
						break;
					}

				case GearType.IDLE_GEAR:
					{
						gearView.transform.SetParent (game.view.gameGearsContainer.transform);
						break;
					}

				case GearType.MOTOR_GEAR:
					{
						gearView.transform.SetParent (game.view.gameGearsContainer.transform);
						break;
					}
			}

			gearsDictionary.Add (gearView, gearModel);
		}
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
	