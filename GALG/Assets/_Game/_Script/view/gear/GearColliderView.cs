using UnityEngine;
using System.Collections;


public class GearColliderView : View
{
	public GearColliderType ColliderType;
	public bool 			isSendNotification = false;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(isSendNotification)
			SendNotification(true, other);
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if(isSendNotification)
			SendNotification(false, other);
	}

	private void SendNotification(bool isEnter, Collider2D other)
	{
		Debug.Log ( transform.parent.name+ " triggered with "+other.transform.parent.name + " type = " + ColliderType + " isEnter = " + isEnter);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, other.transform.position);
		Vector3 collisionPoint = hit.point;

		Notify (N.GearsColliderTriggered______, transform.parent.GetComponent<GearView>(), other.transform.parent.GetComponent<GearView>(),  ColliderType, other.GetComponent<GearColliderView>().ColliderType, isEnter);
	}

}
