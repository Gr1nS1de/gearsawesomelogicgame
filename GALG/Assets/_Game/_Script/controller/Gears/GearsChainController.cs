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
	List<GearView> _motorConnectedGearsList = new List<GearView> ();

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

		//Check if gears should stack
		if (IsChainStuck ())
			Notify (N.OnGearsChainStuck_, true);
		else 
			if (_gearsStuckFlag)
				Notify (N.OnGearsChainStuck_, false);
	}

	private void UpdateConnectedGearsChain()
	{
		GearView motorGear = gearsList.Find ((gear ) => gearsDictionary [gear].gearType == GearType.MOTOR_GEAR);

		_motorConnectedGearsList.Clear ();

		ConnectMotorChain (motorGear);

		gearsList.ForEach ((gear ) =>
		{
			if(!_motorConnectedGearsList.Contains(gear))
				ResetGearPositionState(gear, true);
		});
	}

	private void ConnectMotorChain(GearView gearView)
	{
		GearModel gearModel = gearsDictionary [gearView];
		GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);
		List<GearView> attachedGearsList = new List<GearView> (spinCollider.ConnectedGears);


		if (gearView == currentGearView && selectedGearModel.isError)
		{
			if(currentGearModel.gearPositionState == GearPositionState.CONNECTED)
				ResetGearPositionState (gearView, true);
			
			return;
		}

		//Debug.LogError ("Start connected gears for "+ gearView.name);

		foreach(var attachedGear in attachedGearsList)
		{
			GearModel attachedGearModel = gearsDictionary[attachedGear];

			if (attachedGear == currentGearView && selectedGearModel.isError)
				continue;

			if (attachedGearModel.gearPositionState == GearPositionState.CONNECTED)
			{
				if (!_motorConnectedGearsList.Contains (attachedGear))
				{
					//Debug.LogError ("Check gear for "+gearView.name+ " - " + attachedGear.name  + " connected already!");
					_motorConnectedGearsList.Add (attachedGear);
					ConnectMotorChain (attachedGear);
				}
				continue;
			}

			//Debug.LogError ("Connect "+gearView.name + " with " + attachedGear.name);

			attachedGearModel.isRotateRight = !gearModel.isRotateRight;
			
			ConnectGears (attachedGear, gearView);

			ConnectMotorChain (attachedGear);
		}

		//CheckRedundantGearJoints (gearView);
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
			int leftRotatedGears = 0;
			int rightRotatedGears = 0;

			foreach (var connectedGear in spinCollider.ConnectedGears)
			{
				GearModel connectedGearModel = gearsDictionary[connectedGear];

				if (connectedGearModel.isRotateRight)
					rightRotatedGears++;
				else
					leftRotatedGears++;
			}

			if (rightRotatedGears > 0 && leftRotatedGears > 0)
			{
				isChainStuck = true;
				break;
			}
		}

		return isChainStuck;
	}

	private void ConnectGears (GearView triggerGear, GearView connectGear)
	{
		GearModel triggerGearModel = gearsDictionary[triggerGear];
		GearModel connectGearModel = gearsDictionary[connectGear];
		GearJoint2DExt connectGearJoint = !IsGearAlreadyConnected (triggerGear, connectGear) ? connectGear.gameObject.AddComponent<GearJoint2DExt> () : new List<GearJoint2DExt>(connectGear.GetComponents<GearJoint2DExt>()).Find(gearJoint=>gearJoint.connectedJoint == triggerGear.GetComponent<HingeJoint2DExt>());

		//Debug.Log ("Connect gears: " + triggerGear.name + " to " + connectGear.name);

		_motorConnectedGearsList.Add (triggerGear);

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
	/*
	private void CheckRedundantGearJoints(GearView gearView)
	{
		GearModel gearModel = gearsDictionary [gearView];
		GearColliderView spinCollider = new List<GearColliderView> (gearView.GetComponentsInChildren<GearColliderView> ()).Find (gearCollider => gearCollider.ColliderType == GearColliderType.SPIN);
		List<GearView> attachedGearsList = new List<GearView> (spinCollider.ConnectedGears);

		//Debug.Log ("Start check redundant gear joints");

		foreach(var gearJoint in gearView.GetComponents<GearJoint2DExt> ())
		{
			GearView connectedGearView = gearJoint.connectedJoint.GetComponent<GearView> ();

			//if gearView have gearJoint connected to another gear which not realy connected to gearView - destroy gear joint
			if (!attachedGearsList.Contains (connectedGearView))
			{
				Debug.Log ("Destroy redundant gear joint from "+gearJoint.name);
				Destroy (gearJoint);
			}
		}

		//Debug.Log ("End check redundant gear joints. Is connected to motor" + gearView.name + " " + isGearConnectedToMotor);
	}*/

	public void DeleteGearJoints(GearView gearView)
	{
		bool isIncludeConnected = true;//gearView == currentGearView && selectedGearModel.baseCollisionsCount != 0 ? true : false;
		List<GearJoint2DExt> gearJoints = new List<GearJoint2DExt> (gearView.GetComponents<GearJoint2DExt> ());

		if (gearJoints.Count > 0)
		{
			//Debug.Log ("Delete all gear joints from "+gearView.name);
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
