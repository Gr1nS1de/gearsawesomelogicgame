using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinksquirrel.Phys2D;
using Thinksquirre.Phys2DExamples;

public class GearsChainController : Controller
{
	private GearView 						currentGearView 			{ get { return game.view.currentGearView; } set { game.view.currentGearView = value; } }
	private GearModel 						currentGearModel 			{ get { return gearsDictionary[currentGearView]; } }
	private SelectedGearModel				selectedGearModel			{ get { return game.model.selectedGearModel;}}
	private GearsFactoryModel 				gearsFactoryModel 			{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsList					{ get { return gearsFactoryModel.instantiatedGearsList; } }
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return gearsFactoryModel.gearsDictionary; } }

	private int _checksMotorConnectedCount = 0;
	private bool _gearsStuckFlag = false;

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameOnStart:
				{
					OnStart();

					break;
				}

			case N.UpdateGearsChain:
				{
					//Debug.Log ("Update gear chain");
					//if(selectedGearModel.baseCollisionsCount == 0)
					StartCoroutine (StartUpdateCicle ());
					
					break;
				}

			case N.OnGearsChainStuck_:
				{
					_gearsStuckFlag = (bool)data[0];
					GearView motorGearView = gearsList.Find ((gearView ) => gearsDictionary [gearView].gearType == GearType.MOTOR_GEAR);
					GearModel motorGearModel = gearsDictionary[ motorGearView ];

					motorGearView.GetComponent<SinusoidalMotor> ().IsSinusoidal = _gearsStuckFlag;
					//motorGearView.GetComponent<HingeJoint2DExt> ().jointSpeed = 150f;

					break;
				}
		}

	}

	private void OnStart()
	{

	}

	private IEnumerator StartUpdateCicle()
	{
		yield return null; //Delay 1 frame for physix storing connections

		//Update gears connected_state for all gears
		UpdateConnectedGearsChain ();

		//Check for disconnected parts of chain
		//UpdateRedundantGearsChains ();

		//Update connected_state one more time for proper gears_state after destroying redundant gears.
		//UpdateConnectedGearsChain ();

		//Check if gears should stack
		if (IsChainStuck ())
			Notify (N.OnGearsChainStuck_, true);
		else 
			if (_gearsStuckFlag)
				Notify (N.OnGearsChainStuck_, false);
	}

	private void UpdateConnectedGearsChain()
	{
		int updateCount = 0;
		List<GearView> alreadyMotorConnectedGearsList = new List<GearView> ();

		for (int i = 0; i < gearsList.Count; i++)
		{
			if (updateCount++ > 100)
			{
				Debug.LogError ("Error. Updated gear more than "+updateCount +" times.");
				break;
			}

			GearView gearView = gearsList [i];
			GearModel gearModel = gearsDictionary [gearView];
			GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);
			List<GearView> alreadyConnectedGearsList = new List<GearView> (spinCollider.ConnectedGears);

			//If none trigger stay connection at all
			if (alreadyConnectedGearsList.Count <= 0 || (gearView == currentGearView && selectedGearModel.baseCollisionsCount != 0))
			{
				//Debug.Break ();
				ResetGearPositionState(gearView, true);
				continue;
			}

			int defaultConnectionsCount = 0;

			CheckRedundantGearJoints (gearView, alreadyConnectedGearsList);

			if (gearModel.gearType == GearType.MOTOR_GEAR)
				continue;

			//Debug.LogError ("--- Start update gear "+ gearView.name);

			foreach (var connectedGearView in alreadyConnectedGearsList)
			{
				GearModel connectedGearModel = gearsDictionary [connectedGearView];
				GearColliderView connectedSpinCollider = new List<GearColliderView> (connectedGearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);
				List<GearView> connectedGearConnectedGearsList = new List<GearView>( connectedSpinCollider.ConnectedGears);
				//Debug.Log ("Gear "+ gearView.name + " connected with "+connectedGearView.name);

				//Check connected gear position state for update current connection
				switch (connectedGearModel.gearPositionState)
				{
					case GearPositionState.DEFAULT:
						{
							_checksMotorConnectedCount = 0;
							if (IsGearConnectedToMotor (connectedGearView, connectedGearConnectedGearsList, alreadyMotorConnectedGearsList))
							{
								//Debug.LogError (connectedGearView.name + " connected to motor, but default!");
							}

							defaultConnectionsCount++;
							
							break;
						}

					case GearPositionState.CONNECTED:
						{
							if (connectedGearModel.gearType == GearType.MOTOR_GEAR && (!IsGearAlreadyConnected (gearView, connectedGearView) || gearModel.gearPositionState != GearPositionState.CONNECTED))
							{
								ConnectGears (gearView, connectedGearView);

								if(!alreadyMotorConnectedGearsList.Contains (gearView))
									alreadyMotorConnectedGearsList.Add (gearView);
								
								i = 0;
								break;
							}
							else if (connectedGearModel.gearType == GearType.MOTOR_GEAR)
							{
								
								break;
							}

							_checksMotorConnectedCount = 0;

							if (!IsGearConnectedToMotor (connectedGearView, connectedGearConnectedGearsList, alreadyMotorConnectedGearsList))
							{
								//Debug.LogError (connectedGearView.name+" gear position state connected but not connected to motor!");
								ResetGearPositionState (connectedGearView, true);

								if(alreadyMotorConnectedGearsList.Contains (connectedGearView))
									alreadyMotorConnectedGearsList.Remove (connectedGearView);

								i = 0;
								continue;
							}

							if(!IsGearAlreadyConnected(gearView, connectedGearView) && gearModel.gearPositionState == GearPositionState.DEFAULT)/// && gearsDictionary[gearView].gearPositionState == GearPositionState.DEFAULT || (IsGearAlreadyConnected(gearView, connectedGearView) && gearsDictionary[gearView].gearPositionState != GearPositionState.CONNECTED))
							{
								ConnectGears (gearView, connectedGearView);

								if(!alreadyMotorConnectedGearsList.Contains (gearView))
									alreadyMotorConnectedGearsList.Add (gearView);
								
								i = 0;
								continue;
							}

							break;
						}
				}
			}

			//If all connections are default - delete all gearJointComponents if have.
			if (defaultConnectionsCount == spinCollider.ConnectedGears.Count)
			{
				ResetGearPositionState (gearView);
			}
		}
	}
	/*
	private void UpdateRedundantGearsChains()
	{
		List<GearView> alreadyConnectedToMotorList = new List<GearView>();

		//Debug.LogError ("---------------");

		for (int i = 0; i < gearsList.Count; i++)
		{
			GearView gearView = gearsList [i];
			GearModel gearModel = gearsDictionary [gearView];
			GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);


			if (gearModel.gearType != GearType.MOTOR_GEAR)
			{
				_checksMotorConnectedCount = 0;
				//Debug.Log ("Start check gear "+ gearView.name);

				if(IsGearConnectedToMotor (gearView, spinCollider.ConnectedGears, alreadyConnectedToMotorList))
				{
					alreadyConnectedToMotorList.Add(gearView);
					//Debug.LogError ("Gear " + gearView.name + " connected to motor! +");
				}
				else
				{
					ResetGearPositionState (gearView, true);
					//Debug.LogError ("Gear " + gearView.name + " not connected to motor! -");
				}
			}
		}
	}
*/
	private bool IsChainStuck()
	{
		bool isChainStuck = false;

		for (int i = 0; i < gearsList.Count; i++)
		{
			GearView gearView = gearsList [i];
			GearModel gearModel = gearsDictionary [gearView];
			GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);
			GearView parentAttachedGear = spinCollider.ConnectedGears.Find((gear)=>new List<GearJoint2DExt>(gear.GetComponents<GearJoint2DExt>()).Find((gearJoint)=>gearJoint.connectedJoint == gearView.GetComponent<HingeJoint2DExt>())) ;

			foreach (var connectedGear in spinCollider.ConnectedGears)
			{
				List<GearJoint2DExt> connectedGearJoints = new List<GearJoint2DExt>(connectedGear.GetComponents<GearJoint2DExt> ());

				//If gear connected with another gear which connected with this gear parent gear joint. Chain stuck!
				if (parentAttachedGear != null && connectedGearJoints.Find (gearJoint => gearJoint.connectedJoint == parentAttachedGear.GetComponent<HingeJoint2DExt> ()) != null)
				{
					isChainStuck = true;
					i = gearsList.Count;
					break;
				}
			}
		}

		return isChainStuck;
	}

	private void ConnectGears (GearView triggerGear, GearView connectGear)
	{
		GearModel triggerGearModel = gearsDictionary[triggerGear];
		GearModel connectGearModel = gearsDictionary[connectGear];
		GearJoint2DExt connectGearJoint = !IsGearAlreadyConnected (triggerGear, connectGear) ? connectGear.gameObject.AddComponent<GearJoint2DExt> () : new List<GearJoint2DExt>(connectGear.GetComponents<GearJoint2DExt>()).Find(gearJoint=>gearJoint.connectedJoint == triggerGear.GetComponent<HingeJoint2DExt>());

		Debug.Log ("Connect gears: " + triggerGear.name + " to " + connectGear.name);

		triggerGearModel.gearPositionState = GearPositionState.CONNECTED;

		if (currentGearView != null && currentGearView == triggerGear)
		{
			Utils.SetGearLayer (currentGearView, GearLayer.SELECTED_CONNECTED);
		}
		else
			Utils.SetGearLayer (triggerGear, GearLayer.CONNECTED);

		connectGearJoint.localJoint = connectGearJoint.GetComponent<HingeJoint2DExt> ();
		connectGearJoint.connectedJoint = triggerGear.GetComponent<HingeJoint2DExt> ();
		connectGearJoint.gearRatio = (float)triggerGearModel.teethCount / connectGearModel.teethCount;
		connectGearJoint.collideConnected = true;
	}

	private void CheckRedundantGearJoints(GearView gearView, List<GearView> connectedGearsList)
	{
		GearModel gearModel = gearsDictionary [gearView];

		//Debug.Log ("Start check redundant gear joints");

		foreach(var gearJoint in gearView.GetComponents<GearJoint2DExt> ())
		{
			GearView connectedGearView = gearJoint.connectedJoint.GetComponent<GearView> ();

			//if gearView have gearJoint connected to another gear which not realy connected to gearView - destroy gear joint
			if (!connectedGearsList.Contains (connectedGearView))
			{
				Debug.Log ("Destroy redundant gear joint from "+gearJoint.name);
				Destroy (gearJoint);
			}
		}

		//Debug.Log ("End check redundant gear joints. Is connected to motor" + gearView.name + " " + isGearConnectedToMotor);
	}

	//Recursive searching the way to motor gear
	private bool IsGearConnectedToMotor(GearView gearView, List<GearView> connectedToInitGearList, List<GearView> alreadyMotorConnectedGearsList, GearView excludeGear = null)
	{
		if (_checksMotorConnectedCount++ >20)
		{
			Debug.LogError ("Error. Searching way to motor iterated more than "+_checksMotorConnectedCount+ " times.");
			_checksMotorConnectedCount = 0;
			return false;
		}

		//Debug.LogError ("Check if "+gearView.name + " connected to motor. Checks count = "+_checksMotorConnectedCount);
			
		bool isGearConnected = false;

		GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);
		List<GearView> connectedToSpin = new List<GearView> (spinCollider.ConnectedGears);
		//Debug.Log ("Start check gear "+gearView.name+" connected to motor." + _checksCount +( excludeGear != null ? "exclude "+ excludeGear.name : " none exclude" ) );

		foreach (var connectedGear in connectedToSpin)
		{
			if (connectedToInitGearList.Contains (connectedGear))
				connectedToInitGearList.Remove (connectedGear);

			//If connected to motor gear
			if(gearsDictionary[connectedGear].gearType == GearType.MOTOR_GEAR)
			{
				//Debug.LogError ("Gear " + gearView.name + " connected to motor!");
				isGearConnected = true;
				break;
			}

			if (excludeGear != null && connectedGear == excludeGear || selectedGearModel.baseCollisionsCount != 0)
			{
				//Debug.LogError ("Excluded gear = " + excludeGear.name + " selected gear base collision count = " + selectedGearModel.baseCollisionsCount);
				continue;
			}

			//If connected to gear which already connected to motor
			if (alreadyMotorConnectedGearsList.Contains (connectedGear))
			{
				//Debug.LogError (connectedGear.name + " already in connected list");
				isGearConnected = true;
				break;
			}
			
			GearColliderView connectedSpinCollider = new List<GearColliderView> (connectedGear.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);

			if (connectedSpinCollider.ConnectedGears.Count>1)
			{
				//Debug.LogError (connectedSpinCollider.transform.parent.name+ " have more than 1 connected gear - check is it connected to motor");
				//Debug.Log ("Gear "+gearView.name+ " have more then 0 GearJoints");
				return IsGearConnectedToMotor (connectedGear, connectedToInitGearList, alreadyMotorConnectedGearsList, gearView);
			}
		}

		//Debug.Log ("End check gear "+gearView.name + "connected to motor.");

		if (connectedToInitGearList.Count == 0 || isGearConnected)
			return isGearConnected;
		else
			return IsGearConnectedToMotor (connectedToInitGearList [0], connectedToInitGearList, alreadyMotorConnectedGearsList);
	}

	private void ResetGearPositionState(GearView gearView, bool isDeleteGearJoints = false, GearPositionState gearPositionState = GearPositionState.DEFAULT)
	{
		GearModel gearModel = gearsDictionary [gearView];

		switch(gearPositionState)
		{
			case GearPositionState.DEFAULT:
				{

					if (gearModel.gearType != GearType.MOTOR_GEAR)
					{
						gearModel.gearPositionState = GearPositionState.DEFAULT;

						if (selectedGearModel.baseCollisionsCount == 0)
						{
							if (currentGearView != null && currentGearView == gearView)
								Utils.SetGearLayer (currentGearView, GearLayer.SELECTED);
							else
								Utils.SetGearLayer (gearView, GearLayer.PLAYER);
						}
						else
						{
							if (currentGearView == null)
							{
								Debug.LogError (gearView.name + " - current gear view == null ");
								return;
							}

							//Debug.LogError (currentGearView.name + " base collisions cont > 0. It's wrong position.");
						}
							
					}
					
					break;
				}
		}

		if (isDeleteGearJoints)
		{
			DeleteGearJoints (gearView);
		}
	}

	private bool IsGearAlreadyConnected(GearView gearView, GearView connectGearView)
	{
		bool isConnected = false;

		GearJoint2DExt gearJoinComponent = new List<GearJoint2DExt>(connectGearView.GetComponents<GearJoint2DExt>()).Find(gearJoint=>gearJoint.connectedJoint == gearView.GetComponent<HingeJoint2DExt>());

		if (gearJoinComponent != null)
			isConnected = true;

		return isConnected;
	}

	public void DeleteGearJoints(GearView gearView)
	{
		bool isIncludeConnected = true;//gearView == currentGearView && selectedGearModel.baseCollisionsCount != 0 ? true : false;
		List<GearJoint2DExt> gearJoints = new List<GearJoint2DExt> (gearView.GetComponents<GearJoint2DExt> ());

		if (gearJoints.Count > 0)
		{
			Debug.Log ("Delete all gear joints from "+gearView.name);
			gearJoints.ForEach ((gearJoint ) => Destroy (gearJoint));
		}

		if (!isIncludeConnected)
			return;
		
		foreach(var gear in gearsList)
		{
			if (gear == gearView)
				continue;

			gearJoints = new List<GearJoint2DExt> (gear.GetComponents<GearJoint2DExt> ());

			if (gearJoints.Count > 0)
			{
				gearJoints.ForEach ((gearJoint ) =>
				{
					if(gearJoint.connectedJoint == gearView.GetComponent<HingeJoint2DExt>())
					{
						//Debug.Log("Delete gear joint from " + gearJoint.name + " to " + gearView.name);
						Destroy (gearJoint);
					}
				});
			}
		}
	}
}
