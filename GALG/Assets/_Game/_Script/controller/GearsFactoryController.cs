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
		var sizesNamesArray = System.Enum.GetNames (typeof(GearSizeType));
		int gearsInstantiateCount = 0;

		Debug.Log ("Init level. screen size = " + _screenSize + ". S = "+screenSquare + " empty screen square = " + properScreenSquare);

		foreach (var sizeName in sizesNamesArray)
		{
			GearSizeType gearSizeType = (GearSizeType)System.Enum.Parse (typeof(GearSizeType), sizeName);
			Vector2 rendererSize = GetGearRendererSize (gearSizeType);

			if (rendererSize == Vector2.zero)
				continue;

			Debug.Log ("- Maximum possible "+sizeName+" = " + (int)(properScreenSquare / Utils.GetSquare(rendererSize)));

			gearsInstantiateCount  = (int)(properScreenSquare / Utils.GetSquare(rendererSize));

			InstantiateGear (GearType.PLAYER_GEAR, gearSizeType, gearsInstantiateCount );
		}

		InstantiateGear (GearType.MOTOR_GEAR, GearSizeType.MEDIUM, 1);

		InstantiateGear (GearType.IDLE_GEAR, GearSizeType.MEDIUM, 2);
	}

	private void InstantiateGear(GearType gearType, GearSizeType gearSizeType, int count = 1)
	{
		Debug.Log ("Instantiate "+gearType + " sizeType " + gearSizeType + " count = " + count);

		foreach (GearView gearPrefab in gearsList)
		{
			GearModel gearModel = gearPrefab.GetComponent<GearModel> ();

			//If types is not equal
			if (gearModel.gearType != gearType || gearModel.gearSizeType != gearSizeType)
				continue;

			//Counting gears for instantiate everyone
			for (int i = 0; i < count; i++)
			{
				//Add new model component to factory model game object
				gearModel = gearsFactoryModel.gameObject.AddComponent<GearModel>();

				GearView gearView = Instantiate (gearPrefab) as GearView;

				//Set active if prefab inactive
				gearView.gameObject.SetActive (true);

				//Copy model to factory model game object on scene
				gearModel.GetCopyOf<GearModel> (gearView.GetComponent<GearModel> ());

				//Destroy model on gear game object
				Destroy (gearView.GetComponent<GearModel> ());

				switch (gearModel.gearType)
				{
					case GearType.PLAYER_GEAR:
						{
							gearView.transform.name = "PlayerGear_0" + i;
							gearView.transform.SetParent (game.view.playerGearsContainer.transform);
							break;
						}

					case GearType.IDLE_GEAR:
						{
							gearView.transform.name = "IdleGear_0" + i;
							gearView.transform.SetParent (game.view.gameGearsContainer.transform);
							break;
						}

					case GearType.MOTOR_GEAR:
						{
							gearView.transform.name = "MotorGear_0" + i;
							gearView.transform.SetParent (game.view.gameGearsContainer.transform);
							break;
						}
				}

				gearView.transform.position = CalculateGearPosition (gearModel.gearSizeType, gearModel.gearType);

				gearsDictionary.Add (gearView, gearModel);
			}
				
		}
	}

	private Vector3 CalculateGearPosition(GearSizeType gearSizeType, GearType gearType, bool isInCameraViewField = true)
	{
		float gearRadius = GetGearRendererSize (gearSizeType, gearType).x / 2f;
		Vector3 position = Vector3.zero;
		float halfScreenWidth = _screenSize.x / 2f; 
		float halfScreenHeight = _screenSize.y / 2f;

		Debug.Log ("Calculate gear radius = " + gearRadius);

		int i = 0;

		while (true && i < 100)
		{
			Vector3 randomScreenPosition = new Vector3 (Random.Range(-halfScreenWidth, halfScreenWidth), Random.Range(-halfScreenHeight, halfScreenHeight), -1f);

			//Debug.LogError ("Random screen position = " + randomScreenPosition);

			//If no overlap
			if (Utils.IsCorrectGearBasePosition(randomScreenPosition, gearRadius, false, "GearBaseCollider"))
			{
				position = randomScreenPosition;
				break;
			}
				
			i++;
		}

		return position;
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
	