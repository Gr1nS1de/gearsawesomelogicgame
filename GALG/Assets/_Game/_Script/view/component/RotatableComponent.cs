using UnityEngine;
using System.Collections;

public class RotatableComponent : View
{
	public float m_Speed;

	void FixedUpdate ()
	{
		transform.Rotate(0f, 0f, m_Speed);
	}
}
