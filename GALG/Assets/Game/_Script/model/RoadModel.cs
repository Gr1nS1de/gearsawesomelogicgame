using UnityEngine;
using System.Collections;
//using DG.Tweening;

public class RoadModel : Model
{
	public Road					alias				{ get { return _alias; } }
	public float				radius				{ get { return _radius; } }
	public float				width				{ get { return _width;}}
	public int					segments			{ get { return _segments;}}
	public int					scoreToFinish		{ get { return _scoreToFinish;}}
	public ItemsScoreData[]		itemsScoreData		{ get { return _itemsScoreData;}}
	//public DOTweenPath			roadTweenPath		{ get { return _roadTweenPath;}}
	public float				pathDuration		{ get { return _pathDuration;}}

	[SerializeField]
	private Road				_alias;
	[SerializeField]
	private float       		_radius;
	[SerializeField]
	private float       		_width;
	[SerializeField]
	private int					_segments;
	[SerializeField]
	private int					_scoreToFinish;
	[SerializeField]
	private ItemsScoreData[]	_itemsScoreData = new ItemsScoreData[5];
	//[SerializeField]
	//private DOTweenPath			_roadTweenPath;
	[SerializeField]
	private float 				_pathDuration;
}
	
[System.Serializable]
public struct ItemsScoreData
{
	public int scoreCount;
	public Sprite sprite;
}
