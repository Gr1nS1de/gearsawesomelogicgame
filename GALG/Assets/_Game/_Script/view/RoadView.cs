using UnityEngine;
using System.Collections;
//using DG.Tweening;

public class RoadView : View<GameActivity>
{
	private LineRenderer 	_line;
	private RoadModel 		_roadModel;

	public void OnInit (RoadModel roadModel)
	{
		_roadModel = roadModel;

		gameObject.SetActive (true);

		_line = GetComponent<LineRenderer>();
		_line.useWorldSpace = true;

		//_roadModel.roadTweenPath.DOPlay ();

		DrawRoad ();
	}

	public void DrawRoad()
	{/*
		Vector3[] drawPoints = _roadModel.roadTweenPath.GetTween().PathGetDrawPoints(); 
		int secondVertexForPerfectClampedLR = 1;
		int pointsCount = drawPoints.Length + secondVertexForPerfectClampedLR ;

		_line.SetWidth(_roadModel.width,_roadModel.width);
		_line.SetVertexCount(pointsCount);

		for (int i = 0; i < pointsCount; ++i)
			if (i == pointsCount - 1)
				_line.SetPosition (i, drawPoints [1]);
			else
				_line.SetPosition(i, drawPoints[i]);
*/
	}
}
