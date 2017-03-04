using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearsCollisionController : Controller 
{

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

	private void OnStart()
	{
	}

	private void OnGameOver()
	{

	}
}
