using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Utility class.
/// </summary>
public static class Utils
{
	private const string LastScoreKey = "LAST_SCORE";
	private const string BestScoreKey = "BEST_SCORE";

	public static float GetSquare(Vector2 size)
	{
		return size.x * size.y;
	}

	public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve,float smoothness){
		List<Vector3> points;
		List<Vector3> curvedPoints;
		int pointsLength = 0;
		int curvedLength = 0;

		if(smoothness < 1.0f) smoothness = 1.0f;

		pointsLength = arrayToCurve.Length;

		curvedLength = (pointsLength*Mathf.RoundToInt(smoothness))-1;
		curvedPoints = new List<Vector3>(curvedLength);

		float t = 0.0f;
		for(int pointInTimeOnCurve = 0;pointInTimeOnCurve < curvedLength+1;pointInTimeOnCurve++){
			t = Mathf.InverseLerp(0,curvedLength,pointInTimeOnCurve);

			points = new List<Vector3>(arrayToCurve);

			for(int j = pointsLength-1; j > 0; j--){
				for (int i = 0; i < j; i++){
					points[i] = (1-t)*points[i] + t*points[i+1];
				}
			}

			curvedPoints.Add(points[0]);
		}

		return(curvedPoints.ToArray());
	}

	public static void ActivateTransformChildrens(Transform obj, bool isActivate)
	{
		if (!obj)
			Debug.LogError ("Try to activate null Transform");
		
		for(int i = 0; i < obj.transform.childCount; i++)
		{
			if(!obj.transform.GetChild (i).gameObject.activeInHierarchy)
				obj.transform.GetChild (i).gameObject.SetActive(isActivate);
		}
	}

	public static void AddRoadScore(Road road, int score)
	{
		int currentScore = GetRoadScore (road);

		PlayerPrefs.SetInt (road.ToString(), currentScore + score);

		PlayerPrefs.Save();
	}

	public static void SetLastScore(int score)
	{
		PlayerPrefs.SetInt(LastScoreKey,score);

		SetBestScore(score);

		PlayerPrefs.Save();
	}

	public static void SetBestScore(int score)
	{
		int b = GetBestScore();

		if(score > b)
			PlayerPrefs.SetInt(BestScoreKey,score);
	}

	public static int GetRoadScore(Road road)
	{
		return PlayerPrefs.GetInt (road.ToString(), 0);
	}

	public static int GetBestScore()
	{
		return PlayerPrefs.GetInt(BestScoreKey, 0);
	}

	public static int GetLastScore()
	{
		return PlayerPrefs.GetInt(LastScoreKey, 0);
	}
		
	public static bool IsCorrectGearPosition(Vector3 gearPosition, float gearRadius, bool isIncludeSelf, string layerName)
	{
		List<Collider2D> overlapList = new List<Collider2D>( Physics2D.OverlapCircleAll ((Vector2)gearPosition, gearRadius, 1<<LayerMask.NameToLayer (layerName)));
		bool isCorrectPosition = false;

		//Check for no overlap with other gear
		if (overlapList.Count == 1 && isIncludeSelf || overlapList.Count == 0 && !isIncludeSelf )
		{
			isCorrectPosition = true;
		}

		return isCorrectPosition;
	}


	public static void SetGearLayer(GearView gearView, GearLayer gearLayer)
	{
		int gearViewLayer = gearView.gameObject.layer;
		string layerName = "";

		switch (gearLayer)
		{
			case GearLayer.CONNECTED:
				{
					layerName = "ConnectedGear";
					break;
				}

			case GearLayer.PLAYER:
				{
					layerName = "PlayerGear";
					break;
				}

			case GearLayer.SELECTED:
				{
					layerName = "SelectedGear";
					break;
				}

			case GearLayer.SELECTED_CONNECTED:
				{
					layerName = "SelectedConnectedGear";
					break;
				}

			case GearLayer.ERROR:
				{
					layerName = "ErrorGear";
					break;
				}
		}

		if (gearViewLayer == LayerMask.NameToLayer ("CheckpointGear"))
		{
			layerName = "CheckpointGear";
		}
		//Debug.LogError (gearView.name + " set layer " + layerName);
				
		gearView.gameObject.layer = LayerMask.NameToLayer (layerName);
	}


}
