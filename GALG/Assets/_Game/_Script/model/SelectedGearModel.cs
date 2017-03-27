using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedGearModel : Model 
{
	public GearModel 						gearModel 					{ get { return game.model.gearsFactoryModel.gearsDictionary[game.view.currentGearView]; } }
	public Vector3							lastCorrectPosition			{ get { return _lastCorrectPosition; } 	set { _lastCorrectPosition = value; } }
	public int								baseCollisionsCount			{ get { return _collisionsCount; } 	set { _collisionsCount = value; } }
	public bool								isError						{ get { return baseCollisionsCount != 0; } }


	[SerializeField]
	private Vector3 						_lastCorrectPosition;
	[SerializeField]
	private int 							_collisionsCount 			= 0;
	[SerializeField]
	private bool 							_isError					= false;
}
