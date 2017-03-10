using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GearColliderView : View
{
	private Dictionary<GearView, GearModel> gearsDictionary {get {return game.model.gearsFactoryModel.gearsDictionary; } }

	public GearColliderType ColliderType;
	[HideInInspector]
	public float			ColliderRadius;
	public bool 			isSendEntryNotification = false;
	public List<GearView>	ConnectedGears = new List<GearView>();

	private GearModel 		_gearModel;

	void Start()
	{
		ColliderRadius = GetComponent<CircleCollider2D> ().radius;
		_gearModel = gearsDictionary[transform.parent.GetComponent<GearView>()];
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		GearView otherGearView = other.transform.parent.GetComponent<GearView> ();

		if (!ConnectedGears.Contains (otherGearView))
			ConnectedGears.Add (otherGearView);

		if (isSendEntryNotification)
		{
			SendEntryNotification (true, other);
			//Debug.LogError ("Triggered "+transform.parent.name + " with "+ other.transform.parent.name + " " + other.GetComponent<GearColliderView>().ColliderType);
		}
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (ColliderType != GearColliderType.SPIN || other.GetComponent<GearColliderView>().ColliderType != GearColliderType.SPIN)
			return;

		GearView otherGearView = other.transform.parent.GetComponent<GearView> ();
		//GearModel otherGearModel = gearsDictionary[otherGearView];
		//GearColliderView otherSpinCollider = other.GetComponent<GearColliderView> ();

		if (!ConnectedGears.Contains (otherGearView))
			ConnectedGears.Add (otherGearView);

		/*
		switch (otherGearModel.gearPositionState)
		{
			case GearPositionState.DEFAULT:
				{
					if (ConnectedGears.Contains (otherGearView))
					{
						Notify (N.OnDisconnectGears__, transform.parent.GetComponent<GearView> (), otherGearView);

						ConnectedGears.Remove (otherGearView);
						otherSpinCollider.ConnectedGears.Remove (otherGearView);
					}
					else if (_gearModel.gearPositionState == GearPositionState.CONNECTED)
					{
						Notify (N.OnConnectGears__, otherGearView, transform.parent.GetComponent<GearView> ());

						ConnectedGears.Add (otherGearView);
						otherSpinCollider.ConnectedGears.Add(transform.parent.GetComponent<GearView>());
					}
					break;
				}

			case GearPositionState.CONNECTED:
				{
					//If already connected - return
					if (ConnectedGears.Contains (otherGearView))
						return;

					Notify (N.OnConnectGears__, transform.parent.GetComponent<GearView> (), otherGearView);

					ConnectedGears.Add (otherGearView);
					otherSpinCollider.ConnectedGears.Add (transform.parent.GetComponent<GearView>());

					break;
				}
		}*/
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		GearView otherGearView = other.transform.parent.GetComponent<GearView> ();

		if (ConnectedGears.Contains (otherGearView))
			ConnectedGears.Remove (otherGearView);

		if (isSendEntryNotification)
		{
			SendEntryNotification (false, other);
			/*
			if (otherGearView != null && ConnectedGears.Contains (otherGearView))
			{
				Notify (N.OnDisconnectGears__, otherGearView, transform.parent.GetComponent<GearView> ());

				ConnectedGears.Remove (otherGearView);
				other.GetComponent<GearColliderView> ().ConnectedGears.Remove (transform.parent.GetComponent<GearView> ());
			}*/
		}
	}

	private void SendEntryNotification(bool isEnter, Collider2D other)
	{
		//Debug.Log ( transform.parent.name+ " triggered with "+other.transform.parent.name + " type = " + ColliderType + " isEnter = " + isEnter);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, other.transform.position);
		Vector3 collisionPoint = hit.point;

		Notify (N.GearsColliderTriggered_____, transform.parent.GetComponent<GearView>(), other.transform.parent.GetComponent<GearView>(),  this, other.GetComponent<GearColliderView>(), isEnter);
	}

}
