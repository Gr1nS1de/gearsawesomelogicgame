using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRotationFixer : MonoBehaviour 
{
	public Transform	RelativeTransform;

	private Quaternion 	_initRotation;

	// Use this for initialization
	void Start () {
		_initRotation = transform.rotation;	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(RelativeTransform == null)
		{
			transform.rotation = _initRotation;	
		}else
		{
			transform.position = new Vector3(RelativeTransform.position.x + 0.05f, RelativeTransform.position.y - 0.03f, transform.position.z);
			transform.rotation = RelativeTransform.rotation;
		}
	}
}
