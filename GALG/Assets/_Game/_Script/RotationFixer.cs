using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFixer : MonoBehaviour 
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
			transform.rotation = RelativeTransform.rotation;
		}
	}
}
