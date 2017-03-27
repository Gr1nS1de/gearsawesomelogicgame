using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DG.Tweening;

public class GearsFactoryController : Controller
{
	private GearsFactoryModel 				gearsFactoryModel	{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsPrefabsList	{ get { return game.model.gearsFactoryModel.themeGearsPrefabsList; } }
	private List<GearView>					gearsList			{ get { return game.model.gearsFactoryModel.instantiatedGearsList; } }
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

			case N.StartGenerateLevel:
				{
					ClearCurrentLevel ();
					InitLevel_Old (0f);
					break;
				}
		}
	}

	private void OnStart()
	{
		float screenHeight = Camera.main.orthographicSize * 2.0f;
		float screenWidth = screenHeight * Camera.main.aspect;
		_screenSize = new Vector2 (screenWidth, screenHeight);

	}

	private void InitLevel(float difficulty)
	{

	}

	private void InitLevel_Old(float difficulty)
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
			{
				Debug.LogError (gearSizeType.ToString()+ " have renderer size = zero vector");
				continue;
			}

			Debug.Log ("- Maximum possible "+sizeName+" = " + (int)(properScreenSquare / Utils.GetSquare(rendererSize)));

			gearsInstantiateCount  = (int)(properScreenSquare / Utils.GetSquare(rendererSize));

			InstantiateGear (GearType.PLAYER_GEAR, gearSizeType, gearsInstantiateCount - 1 );
		}

		InstantiateGear (GearType.MOTOR_GEAR, GearSizeType.MEDIUM, 1);

		InstantiateGear (GearType.CHECKPOINT_GEAR, GearSizeType.MEDIUM, 2);
	}

	private void InstantiateGear(GearType gearType, GearSizeType gearSizeType, int count = 1)
	{
		Debug.Log ("Instantiate "+gearType + " sizeType " + gearSizeType + " count = " + count);

		foreach (GearView gearPrefab in gearsPrefabsList)
		{
			GearModel gearModel = gearPrefab.GetComponent<GearModel> ();

			//If types is not equal
			if (gearModel.gearType != gearType || gearModel.gearSizeType != gearSizeType)
				continue;

			//Counting gears for instantiate everyone
			for (int i = 0; i < count; i++)
			{
				//Debug.Log ("Start create object "+gearModel.gearType);
				GameObject gearModelObject = new GameObject ();

				gearModelObject.transform.SetParent (gearsFactoryModel.transform);

				//Add new model component to factory model game object
				gearModel = gearModelObject.AddComponent<GearModel>();

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
							gearView.transform.name = gearModelObject.name = "PlayerGear_0" + i;
							gearView.transform.SetParent (game.view.playerGearsContainer.transform);
							break;
						}

					case GearType.CHECKPOINT_GEAR:
						{
							gearView.transform.name = gearModelObject.name = "IdleGear_0" + i;
							gearView.transform.SetParent (game.view.gameGearsContainer.transform);
							break;
						}

					case GearType.MOTOR_GEAR:
						{
							gearView.transform.name = gearModelObject.name = "MotorGear_0" + i;
							gearView.transform.SetParent (game.view.gameGearsContainer.transform);
							gearModel.isRotateRight = true;
							break;
						}
				}

				gearView.transform.position = GetGearRandomScreenPosition (gearModel.gearSizeType, gearModel.gearType);

				gearsList.Add (gearView);
				gearsDictionary.Add (gearView, gearModel);
			}
				
		}
	}

	private Vector3 GetGearRandomScreenPosition(GearSizeType gearSizeType, GearType gearType, bool isInCameraViewField = true)
	{
		float gearRadius = GetGearRendererSize (gearSizeType, gearType).x / 2f;
		Vector3 position = Vector3.zero;
		float halfScreenWidth = _screenSize.x / 2f; 
		float halfScreenHeight = _screenSize.y / 2f;

		int i = 0;

		while (true)
		{
			Vector3 randomScreenPosition = new Vector3 (Random.Range(-halfScreenWidth, halfScreenWidth), Random.Range(-halfScreenHeight, halfScreenHeight), -1f);

			//Debug.LogError ("Random screen position = " + randomScreenPosition);

			//If no overlap
			if (Utils.IsCorrectGearPosition(randomScreenPosition, gearRadius, false, "GearBaseCollider"))
			{
				position = randomScreenPosition;
				break;
			}
				
			if (i++ > 100)
			{
				Debug.LogError ("Random screen position searched more than 100 times");
				break;
			}
		}

		return position;
	}

	private Vector3 GetGearRandomPositionOutCircle(Vector3 centerPosition, float radius)
	{
		Vector3 gearProperPosition;
		float randomX = 0;
		float randomY = 0;
		float angleRadians = 0;
		float halfScreenWidth = _screenSize.x / 2f; 
		float halfScreenHeight = _screenSize.y / 2f;
		int searchRotationRadius = 0;

		do
		{
			angleRadians = searchRotationRadius * Mathf.Deg2Rad;//* Mathf.PI / 180.0f;

			// get the 2D dimensional coordinates
			randomX = radius * Mathf.Cos (angleRadians);
			randomY = radius * Mathf.Sin (angleRadians);

			searchRotationRadius += 20;

			if(searchRotationRadius > 380)
			{
				Debug.LogError("Searching position on circle over 380 deg");
				break;
			}
			//If no overlap


		} while(randomX > halfScreenWidth || randomX < -halfScreenWidth || randomY > halfScreenHeight || randomY < -halfScreenHeight);

		gearProperPosition = new Vector3 (randomX, randomY, centerPosition.z);
		/*
		if (Utils.IsCorrectGearPosition(gearProperPosition, radius, false, "GearBaseCollider"))
		{
			return GetGearRandomPositionOutCircle(centerPosition, radius);
		}*/

		// return the vector info
		return gearProperPosition;
	}

	private void ClearCurrentLevel()
	{
		gearsList.ForEach ((gearView ) =>
		{
			Destroy(gearsDictionary[gearView].gameObject);
			Destroy(gearView.gameObject);
		});

		gearsDictionary.Clear ();
		gearsList.Clear ();
	}

	private Vector2 GetGearRendererSize(GearSizeType gearSizeType, GearType gearType = GearType.PLAYER_GEAR)
	{
		Vector2 gearSize = Vector2.zero;

		var gearObj = gearsPrefabsList.Find (gear => gear.GetComponent<GearModel> ().gearSizeType == gearSizeType && gear.GetComponent<GearModel>().gearType == gearType);

		if (gearObj == null)
			return gearSize;

		gearSize = gearObj.GetComponent<SpriteRenderer> ().bounds.size;

		return gearSize;
	}
}
	