using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DG.Tweening;

public class GearsFactoryController : Controller
{
	private GearsFactoryModel 				gearsFactoryModel	{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsList			{ get { return game.model.gearsFactoryModel.loadedGears; } }
	private Dictionary<GearView, GearModel> gearsDictionary 	{ get { return game.model.gearsFactoryModel.gearsDictionary; } }

	private Vector2 						_screenSize;

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
		float screenHeight = Camera.main.orthographicSize * 2.0f;
		float screenWidth = screenHeight * Camera.main.aspect;
		_screenSize = new Vector2 (screenWidth, screenHeight);

		InitLevel (0f);
	}

	private void InitLevel(float difficulty)
	{
		float screenSquare = _screenSize.x * _screenSize.y;
		//Screen square without motor gear size
		float properScreenSquare = screenSquare - Utils.GetSquare(GetGearRendererSize(GearSizeType.MEDIUM, GearType.MOTOR_GEAR));
			
		Debug.Log ("Init level. screen size = " + _screenSize + ". S = "+screenSquare + " empty screen square = " + properScreenSquare);

		var sizesNamesArray = System.Enum.GetNames (typeof(GearSizeType));
		float sizesCount = sizesNamesArray.Length;

		foreach (var sizeName in sizesNamesArray)
		{
			Vector2 rendererSize = GetGearRendererSize ((GearSizeType)System.Enum.Parse (typeof(GearSizeType), sizeName));

			if (rendererSize == Vector2.zero)
				continue;

			Debug.Log ("- Maximum possible "+sizeName+" = " + (int)(properScreenSquare / Utils.GetSquare(rendererSize)));
		}

		//InstantiateGears ();
	}

	private void InstantiateGears(GearType gearType, GearSizeType gearSizeType, int count = 1)
	{
		Debug.Log ("Instantiate "+gearType + " sizeType " + gearSizeType + " count = " + count);

		foreach (GearView gearPrefab in gearsList)
		{
			GearModel gearModel = gearPrefab.GetComponent<GearModel> ();

			if (gearModel.gearType != gearType || gearModel.gearSizeType != gearSizeType)
				continue;

			for (int i = 0; i < count; i++)
			{
				gearModel = gearsFactoryModel.gameObject.AddComponent<GearModel>();
				GearView gearView = Instantiate (gearPrefab) as GearView;

				gearView.gameObject.SetActive (true);

				gearModel.GetCopyOf<GearModel> (gearView.GetComponent<GearModel> ());

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
	}

	private Vector2 GetGearRendererSize(GearSizeType gearSizeType, GearType gearType = GearType.PLAYER_GEAR)
	{
		Vector2 gearSize = Vector2.zero;

		var gearObj = gearsList.Find (gear => gear.GetComponent<GearModel> ().gearSizeType == gearSizeType && gear.GetComponent<GearModel>().gearType == gearType);

		if (gearObj == null)
			return gearSize;

		gearSize = gearObj.GetComponent<SpriteRenderer> ().bounds.size;

		return gearSize;
	}
}
	