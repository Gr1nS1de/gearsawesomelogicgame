using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinksquirrel.Phys2D;

public class GearsCollisionController : Controller 
{
	private GearView 						currentGearView 			{ get { return game.view.currentGearView; } set { game.view.currentGearView = value; } }
	private GearModel 						currentGearModel 			{ get { return gearsDictionary[currentGearView]; } }
	private SelectedGearModel				selectedGearModel			{ get { return game.model.selectedGearModel;}}
	private GearsFactoryModel 				gearsFactoryModel 			{ get { return game.model.gearsFactoryModel; } }
	private List<GearView>					gearsList					{ get { return gearsFactoryModel.instantiatedGearsList; } }
	private Dictionary<GearView, GearModel> gearsDictionary 			{ get { return gearsFactoryModel.gearsDictionary; } }

	private GearPositionState				_storedGearPositionState;

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
		//Debug.Log ("Gear enter collised "+ triggerGear.name + " to "+ triggeredGear.name + " trigger collider type = "+ triggerColliderView.ColliderType + " triggered collider type = "+ triggeredColliderView.ColliderType);

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
								float offsetBeetwenGears = 0.03f;
								float triggerGearRadius = triggerColliderView.ColliderRadius * triggerGear.transform.localScale.x;
								float triggeredGearRadius = triggeredColliderView.ColliderRadius * triggeredGear.transform.localScale.x;
								float baseGap = triggerGearRadius + triggeredGearRadius + offsetBeetwenGears;
								Vector3 beforeTriggerPosition = triggeredGear.transform.position - Vector3.ClampMagnitude( ( triggeredGear.transform.position - (triggerGear.transform.position + new Vector3(0f, 0f, 1f))) * 100f, baseGap);

								selectedGearModel.baseCollisionsCount++;


								//Debug.LogError (_baseCollisionsCount + " beforeTrigPos = "+ beforeTriggerPosition + " raius = " + triggerColliderView.ColliderRadius  + " " + triggerGearRadius);

								//Check for triggered saved position is empty for current gear size. 
								if (Utils.IsCorrectGearPosition (beforeTriggerPosition, triggerGearRadius, true, "GearSpinCollider"))
								{
									selectedGearModel.lastCorrectPosition = beforeTriggerPosition;
									Notify (N.UpdateGearsChain);
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
								Notify (N.UpdateGearsChain);
								break;
							}
					}

					break;
				}
		}
	}


	private void OnGearsExitCollised(GearView triggerGear, GearView triggeredGear, GearColliderView triggerColliderView, GearColliderView triggeredColliderView)
	{
		//Debug.Log ("Gear exit collised "+ triggerGear.name + " to "+ triggeredGear.name + " trigger collider type = "+ triggerColliderView.ColliderType + " triggered collider type = "+ triggeredColliderView.ColliderType);
		
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
								selectedGearModel.baseCollisionsCount--;


								Notify (N.UpdateGearsChain);

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
								Notify (N.UpdateGearsChain);
								break;
							}
					}

					break;
				}
		}
	}



	private void OnGameOver()
	{

	}
}
