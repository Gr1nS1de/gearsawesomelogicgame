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
		UpdateRedundantGearsChains ();

		//Update connected_state one more time for proper gears_state after destroying redundant gears.
		UpdateConnectedGearsChain ();

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
		for (int i = 0; i < gearsList.Count; i++)
		{
			if (updateCount++ > 50)
			{
				Debug.LogError ("Error. Updated gear more than "+updateCount +" times.");
				break;
			}

			GearView gearView = gearsList [i];
			GearModel gearModel = gearsDictionary [gearView];
			GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);

			//If none trigger stay connection at all
			if (spinCollider.ConnectedGears.Count <= 0)
			{
				//Debug.Break ();
				ResetGearPositionState(gearView, true);
				continue;
			}

			int defaultConnectionsCount = 0;

			CheckRedundantGearJoints (gearView, spinCollider.ConnectedGears);

			foreach (var connectedGearView in spinCollider.ConnectedGears)
			{
				GearModel connectedGearModel = gearsDictionary [connectedGearView];

				//Debug.Log ("Gear "+ gearView.name + " connected with "+connectedGearView.name);

				//Check connected gear position state for update current connection
				switch (connectedGearModel.gearPositionState)
				{
					case GearPositionState.DEFAULT:
						{
							defaultConnectionsCount++;
							break;
						}

					case GearPositionState.CONNECTED:
						{
							if(!IsGearAlreadyConnected(gearView, connectedGearView) && gearsDictionary[gearView].gearPositionState == GearPositionState.DEFAULT || (IsGearAlreadyConnected(gearView, connectedGearView) && gearsDictionary[gearView].gearPositionState != GearPositionState.CONNECTED))
							{
								ConnectGears (gearView, connectedGearView);

								i = 0;
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

	private void UpdateRedundantGearsChains()
	{
		List<GearView> alreadyConnectedGearsList = new List<GearView>();

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

				if(IsGearConnectedToMotor (gearView, alreadyConnectedGearsList))
				{
					alreadyConnectedGearsList.Add(gearView);
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
			}//else
			//if(IsGearAlreadyConnected(gearView,	connectedGearView))
			//{
			//	gearModel.gearPositionState = GearPositionState.CONNECTED;
			//	Debug.Log ("Gear "+gearView.name + " already connected to "+ connectedGearView);
			//}
		}

		//Debug.Log ("End check redundant gear joints. Is connected to motor" + gearView.name + " " + isGearConnectedToMotor);
	}

	private bool IsGearConnectedToMotor(GearView gearView, List<GearView> alreadyConnectedGearsList, GearView excludeGear = null)
	{
		if (_checksMotorConnectedCount++ > 50)
		{
			Debug.LogError ("Error. Searching way to motor iterated more than "+_checksMotorConnectedCount+ " times.");
			return false;
		}

		bool isGearConnected = false;
		GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);

		//Debug.Log ("Start check gear "+gearView.name+" connected to motor." + _checksCount +( excludeGear != null ? "exclude "+ excludeGear.name : " none exclude" ) );

		foreach (var connectedGear in spinCollider.ConnectedGears)
		{
			if (excludeGear != null && connectedGear == excludeGear)
				continue;

			//If connected to motor gear = fire action and return
			if(gearsDictionary[connectedGear].gearType == GearType.MOTOR_GEAR)
			{
				//Debug.Log ("Gear " + gearView.name + " connected to motor!");
				isGearConnected = true;
				break;
			}

			//If connected to gear which already connected to motor
			if (alreadyConnectedGearsList.Contains (connectedGear))
			{
				isGearConnected = true;
				break;
			}

			GearColliderView connectedSpinCollider = new List<GearColliderView> (connectedGear.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);

			if (connectedSpinCollider.ConnectedGears.Count>1)
			{
				//Debug.Log ("Gear "+gearView.name+ " have more then 0 GearJoints");
				return IsGearConnectedToMotor (connectedGear, alreadyConnectedGearsList, gearView);
			}
		}

		//Debug.Log ("End check gear "+gearView.name + "connected to motor.");

		return isGearConnected;
	}

	private void ResetGearPositionState(GearView gearView, bool isDeleteGearJoints = false, GearPositionState gearPositionState = GearPositionState.DEFAULT)
	{
		GearModel gearModel = gearsDictionary [gearView];
		List<GearJoint2DExt> gearJoints = new List<GearJoint2DExt> (gearView.GetComponents<GearJoint2DExt> ());

		switch(gearPositionState)
		{
			case GearPositionState.DEFAULT:
				{

					if (gearModel.gearType != GearType.MOTOR_GEAR)
						gearModel.gearPositionState = GearPositionState.DEFAULT;
					
					break;
				}
		}

		if (isDeleteGearJoints && gearJoints.Count > 0)
		{
			Debug.Log ("Delete all gear joints from "+gearView.name);
			gearJoints.ForEach ((gearJoint ) => Destroy (gearJoint));
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
}
