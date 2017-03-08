using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinksquirrel.Phys2D;

public class GearsCollisionController : Controller 
{
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return game.model.gearsFactoryModel.gearsDictionary; } }

	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameOnStart:
				{
					OnStart();

					break;
				}
					

			case N.GearsColliderTriggered_____:
				{
					GearView triggerGear = (GearView)data [0];
					GearView triggeredGear = (GearView)data [1];
					GearColliderView triggerColliderView = (GearColliderView)data [2];
					GearColliderView triggeredColliderView = (GearColliderView)data [3];
					bool isEnterCollision = (bool)data [4];

					if (triggerGear != game.view.currentGearView)
					{
						Debug.LogError ("Trigger gear is not current selected!");
						return;
					}

					if (isEnterCollision)
						OnGearsEnterCollised (triggerGear, triggeredGear, triggerColliderView, triggeredColliderView);
					else
						OnGearsExitCollised (triggerGear, triggeredGear, triggerColliderView, triggeredColliderView);

					break;
				}

			case N.OnConnectGears__:
				{
					GearView triggerGear = (GearView)data [0];
					GearView connectedGear = (GearView)data [1];

					ConnectGears (triggerGear, connectedGear);
					break;
				}

			case N.OnDisconnectGears__:
				{
					GearView triggerGear = (GearView)data [0];
					GearView connectedGear = (GearView)data [1];

					DisconnectGears (triggerGear, connectedGear);
					break;
				}

			case N.GameOver:
				{
					OnGameOver ();

					break;
				}
		}
	}

	private void OnStart()
	{
	}

	private void OnGearsEnterCollised(GearView triggerGear, GearView triggeredGear, GearColliderView triggerColliderView, GearColliderView triggeredColliderView)
	{
		switch (triggerColliderView.ColliderType)
		{
			case GearColliderType.BASE:
				{
					switch (triggeredColliderView.ColliderType)
					{
						case GearColliderType.BASE:
							break;

						case GearColliderType.SPIN:
							{
								float offsetBeetwenGears = 0.01f;
								float triggerGearRadius = triggerColliderView.ColliderRadius * triggerGear.transform.localScale.x;
								float triggeredGearRadius = triggeredColliderView.ColliderRadius * triggeredGear.transform.localScale.x;
								float baseGap = triggerGearRadius + triggeredGearRadius + offsetBeetwenGears;
								Vector3 beforeTriggerPosition = triggeredGear.transform.position - Vector3.ClampMagnitude( ( triggeredGear.transform.position - (triggerGear.transform.position + new Vector3(0f, 0f, 1f))) * 100f, baseGap);

								game.model.currentGearModel.collisionsCount++;

								//Debug.LogError (_baseCollisionsCount + " beforeTrigPos = "+ beforeTriggerPosition + " raius = " + triggerColliderView.ColliderRadius  + " " + triggerGearRadius);

								//Check for triggered saved position is empty for current gear size. 
								if (Utils.IsCorrectGearBasePosition (beforeTriggerPosition, triggerGearRadius, true, "GearSpinCollider"))
								{
									game.model.currentGearModel.lastCorrectPosition = beforeTriggerPosition;
								}

								break;
							}
					}
					break;
				}

			case GearColliderType.SPIN:
				{
					switch (triggeredColliderView.ColliderType)
					{
						case GearColliderType.BASE:
							break;

						case GearColliderType.SPIN:
							{

								break;
							}
					}

					break;
				}
		}
	}


	private void OnGearsExitCollised(GearView triggerGear, GearView triggeredGear, GearColliderView triggerColliderView, GearColliderView triggeredColliderView)
	{
		switch (triggerColliderView.ColliderType)
		{
			case GearColliderType.BASE:
				{
					switch (triggeredColliderView.ColliderType)
					{
						case GearColliderType.BASE:
							{

								break;
							}

						case GearColliderType.SPIN:
							{
								game.model.currentGearModel.collisionsCount--;
								break;
							}
					}

					break;
				}

			case GearColliderType.SPIN:
				{
					switch (triggeredColliderView.ColliderType)
					{
						case GearColliderType.BASE:
							break;

						case GearColliderType.SPIN:
							{

								break;
							}
					}

					break;
				}
		}
	}

	private void ConnectGears (GearView triggerGear, GearView connectedGear)
	{
		GearModel triggerGearModel = gearsDictionary[triggerGear];
		GearModel connectedGearModel = gearsDictionary[connectedGear];
		GearJoint2DExt connectedGearJoint = connectedGear.gameObject.AddComponent<GearJoint2DExt> ();

		triggerGearModel.gearPositionState = GearPositionState.CONNECTED;

		connectedGearJoint.localJoint = connectedGearJoint.GetComponent<HingeJoint2DExt> ();
		connectedGearJoint.connectedJoint = triggerGear.GetComponent<HingeJoint2DExt> ();
		connectedGearJoint.gearRatio = (float)triggerGearModel.teethCount / connectedGearModel.teethCount;
		connectedGearJoint.collideConnected = true;


	}

	private void DisconnectGears (GearView triggerGear, GearView connectedGear)
	{
		GearModel triggerGearModel = gearsDictionary[triggerGear];
		GearModel connectedGearModel = gearsDictionary[connectedGear];

		triggerGearModel.gearPositionState = GearPositionState.DEFAULT;

		foreach (var gearJoint in connectedGear.GetComponents<GearJoint2DExt>())
		{
			if (gearJoint.connectedJoint == triggerGear.GetComponent<HingeJoint2DExt> ())
				Destroy (gearJoint);
		}
	}

	private void OnGameOver()
	{

	}
}
