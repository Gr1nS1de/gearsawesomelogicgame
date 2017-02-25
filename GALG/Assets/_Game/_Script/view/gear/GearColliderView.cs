using UnityEngine;
using System.Collections;

public class GearColliderView : View
{
	public void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ( transform.name+ " triggered with "+other.transform.name);
		Notify (N.GearsColliderTriggered___, transform.name, other.transform.name);

	}

}
