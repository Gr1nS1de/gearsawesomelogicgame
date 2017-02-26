using UnityEngine;
using System.Collections;


public class GearColliderView : View
{
	public GearColliderType ColliderType;

	public void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log ( transform.parent.name+ " triggered with "+other.transform.parent.name + " type = " + ColliderType);
		RaycastHit hit;
		Vector3 collisionPoint = Vector3.zero;


		if (Physics.Raycast(transform.position, transform.forward, out hit))
		{
			Debug.Log("Point of contact: "+hit.point);
			collisionPoint = hit.point;
		}

		Notify (N.GearsColliderTriggered____, transform.parent.GetComponent<GearView>(), other.transform.parent.GetComponent<GearView>(), collisionPoint,  ColliderType);
	}

}
