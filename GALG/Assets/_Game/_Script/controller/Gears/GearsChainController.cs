using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinksquirrel.Phys2D;

public class GearsChainController : Controller
{
	private GearModel 						currentGearModel 			{ get { return gearsDictionary[game.view.currentGearView]; } }
	private GearsFactoryModel 				gearsFactoryModel 			{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsList					{ get { return gearsFactoryModel.instantiatedGearsList; } }
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return gearsFactoryModel.gearsDictionary; } }

	private List<GearJoint2DExt>			_gearJointsDestroyList		= new List<GearJoint2DExt>();

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
					UpdateConnectedGearsChain ();

					UpdateRedundantGearsChains ();

					UpdateConnectedGearsChain ();
					break;
				}
		}

	}

	private void OnStart()
	{

	}

	private void ConnectGears (GearView triggerGear, GearView connectGear)
	{
		GearModel triggerGearModel = gearsDictionary[triggerGear];
		GearModel connectGearModel = gearsDictionary[connectGear];
		GearJoint2DExt connectGearJoint = connectGear.gameObject.AddComponent<GearJoint2DExt> ();

		Debug.Log ("Connect gears: " + triggerGear.name + " to " + connectGear.name);

		triggerGearModel.gearPositionState = GearPositionState.CONNECTED;

		connectGearJoint.localJoint = connectGearJoint.GetComponent<HingeJoint2DExt> ();
		connectGearJoint.connectedJoint = triggerGear.GetComponent<HingeJoint2DExt> ();
		connectGearJoint.gearRatio = (float)triggerGearModel.teethCount / connectGearModel.teethCount;
		connectGearJoint.collideConnected = true;
	}

	private void DisconnectGears (GearView triggerGear, GearView connectedGear)
	{
		GearModel triggerGearModel = gearsDictionary[triggerGear];
		GearModel connectedGearModel = gearsDictionary[connectedGear];

		if(triggerGearModel.gearType != GearType.MOTOR_GEAR)
			triggerGearModel.gearPositionState = GearPositionState.DEFAULT;

		Debug.Log ("Disconnect gears: " + triggerGear.name + " to " + connectedGear.name);

		foreach (var gearJoint in connectedGear.GetComponents<GearJoint2DExt>())
		{
			if (gearJoint.connectedJoint == triggerGear.GetComponent<HingeJoint2DExt> ())
				Destroy (gearJoint);
		}

		foreach (var gearJoint in triggerGear.GetComponents<GearJoint2DExt>())
		{
			if (gearJoint.connectedJoint == connectedGear.GetComponent<HingeJoint2DExt> ())
				Destroy (gearJoint);
		}

	}

	private void UpdateConnectedGearsChain()
	{
		int a = 0;
		for (int i = 0; i < gearsList.Count; i++)
		{
			if(a++>100)break;

			GearView gearView = gearsList [i];
			GearModel gearModel = gearsDictionary [gearView];
			GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);

			//If none trigger stay connection at all
			if (spinCollider.ConnectedGears.Count <= 0)
			{
				ResetGearPositionState(gearView, true);
				continue;
			}

			int defaultConnectionCount = 0;

			CheckRedundantGearJoints (gearView, spinCollider.ConnectedGears);

			foreach (var connectedGearView in spinCollider.ConnectedGears)
			{
				GearModel connectedGearModel = gearsDictionary [connectedGearView];
				//var gearJoinComponent = new List<GearJoint2DExt>(gearView.GetComponents<GearJoint2DExt>()).Find(gearJoint=>gearJoint.connectedJoint == connectedGear.GetComponent<HingeJoint2DExt>());

				//Check connected gear position state for update current connection
				switch (connectedGearModel.gearPositionState)
				{
					case GearPositionState.DEFAULT:
						{
							defaultConnectionCount++;
							break;
						}

					case GearPositionState.CONNECTED:
						{
							if (gearModel.gearPositionState == GearPositionState.DEFAULT)
							{
								if (!IsGearAlreadyConnected (gearView, connectedGearView))
								{
									ConnectGears (gearView, connectedGearView);

									i = 0;
								}
							}
							break;
						}
				}
			}

			//If all connections are default - delete all gearJointComponents if have.
			if (defaultConnectionCount == spinCollider.ConnectedGears.Count)
			{
				ResetGearPositionState (gearView);
			}

		}
	}

	private void UpdateRedundantGearsChains()
	{
		List<GearView> alreadyConnectedGearsList = new List<GearView>();


		Debug.LogError ("---------------");

		for (int i = 0; i < gearsList.Count; i++)
		{
			GearView gearView = gearsList [i];
			GearModel gearModel = gearsDictionary [gearView];
			GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);


			if (gearModel.gearType != GearType.MOTOR_GEAR)
			{
				_checksCount = 0;
				Debug.Log ("Start check gear "+ gearView.name);

				if(IsGearConnectedToMotor (gearView, alreadyConnectedGearsList))
				{
					alreadyConnectedGearsList.Add(gearView);
					Debug.LogError ("Gear " + gearView.name + " connected to motor! +");
				}
				else
				{
					ResetGearPositionState (gearView, true);
					Debug.LogError ("Gear " + gearView.name + " not connected to motor! -");
				}
			}
		}
	}

	private void CheckRedundantGearJoints(GearView gearView, List<GearView> connectedGearsList)
	{
		//Debug.Log ("Start check redundant gear joints");

		foreach(var gearJoint in gearView.GetComponents<GearJoint2DExt> ())
		{
			//if gearView have gearJoint connected to another gear which not realy connected to gearView - destroy gear joint
			if(!connectedGearsList.Contains(gearJoint.connectedJoint.GetComponent<GearView>()))
				Destroy(gearJoint);
		}

		//Debug.Log ("End check redundant gear joints. Is connected to motor" + gearView.name + " " + isGearConnectedToMotor);
	}
	private int _checksCount = 0;

	private bool IsGearConnectedToMotor(GearView gearView, List<GearView> alreadyConnectedGearsList, GearView excludeGear = null)
	{
		if (_checksCount++ > 50)
		{
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
				//actionIsGearConnected(true);
				isGearConnected = true;
				break;
			}

			//If connected to gear which already connected to motor
			if (alreadyConnectedGearsList.Contains (connectedGear))
			{
				isGearConnected = true;
				break;
			}

			//GearJoint2DExt gearJoint = connectedGear.GetComponent<GearJoint2DExt> ();
			GearColliderView connectedSpinCollider = new List<GearColliderView> (connectedGear.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);

			if (connectedSpinCollider.ConnectedGears.Count>1)//.connectedJoint.GetComponents<GearJoint2DExt> ().Length > 0)
			{
				//Debug.Log ("Gear "+gearView.name+ " have more then 0 GearJoints");
				return IsGearConnectedToMotor (connectedGear, alreadyConnectedGearsList, gearView);
			}

			//_gearJointsDestroyList.Add (gearJoint);
		}

		//Debug.Log ("End check gear "+gearView.name + "connected to motor.");

		return isGearConnected;
	}

	private void ResetGearPositionState(GearView gearView, bool isDeleteGearJoints = false, GearPositionState gearPositionState = GearPositionState.DEFAULT)
	{
		GearModel gearModel = gearsDictionary [gearView];

		switch(gearPositionState)
		{
			case GearPositionState.DEFAULT:
				{
					if(isDeleteGearJoints)
						new List<GearJoint2DExt> (gearView.GetComponents<GearJoint2DExt> ()).ForEach ((gearJoin ) => Destroy (gearJoin));

					if (gearModel.gearType != GearType.MOTOR_GEAR)
						gearModel.gearPositionState = GearPositionState.DEFAULT;
					
					break;
				}
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
