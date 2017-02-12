using UnityEngine;
using System.Collections;

public class InputController : Controller
{
	public delegate void OnTouchDown(TouchDirection td);
	public static event OnTouchDown OnTouchedDown;

	public delegate void OnTouchUp();
	public static event OnTouchUp OnTouchedUp;

	private void Update()
	{
		/*
		if(!Application.isMobilePlatform)
		{

			if (Input.GetKeyDown (KeyCode.LeftArrow))
			{
				if(OnTouchedDown!=null)
					OnTouchedDown(TouchDirection.left);

				return;
			} 
			else if (Input.GetKeyDown (KeyCode.RightArrow))
			{
				if(OnTouchedDown!=null)
					OnTouchedDown(TouchDirection.right);

				return;
			}
			else if (Input.GetKeyUp (KeyCode.LeftArrow))
			{
				if(OnTouchedUp!=null)
					OnTouchedUp();

				return;
			}
			else if (Input.GetKeyUp (KeyCode.RightArrow))
			{
				if(OnTouchedUp!=null)
					OnTouchedUp();

				return;
			}

			if(Input.anyKeyDown)
			{
				if(OnTouchedDown!=null)
					OnTouchedDown(TouchDirection.none);
			}

			return;
		}*/

		#if UNITY_TVOS

		float h = Input.GetAxis("Horizontal");

		if(h < 0)
		{
		if(OnTouched!=null)
		OnTouched(TouchDirection.left);
		}
		else if(h > 0)
		{
		if(OnTouched!=null)
		OnTouched(TouchDirection.right);
		}

		#endif

		//#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_TVOS 

		int nbTouches = Input.touchCount;
		if (nbTouches > 0) 
		{
			Touch touch = Input.GetTouch (0);

			TouchPhase phase = touch.phase;

			if (phase == TouchPhase.Began) 
			{
				Notify(N.InputOnTouchDown);
				
				if (touch.position.x < Screen.width / 2f)
				{
					if(OnTouchedDown!=null)
						OnTouchedDown(TouchDirection.left);
				}
				else
				{
					if(OnTouchedDown!=null)
						OnTouchedDown(TouchDirection.right);
				}
			}

			if (phase == TouchPhase.Ended)
			{
				Notify(N.InputOnTouchUp);

				if(OnTouchedUp!=null)
					OnTouchedUp();
			}
		}
		else 
		if (Input.GetMouseButtonDown (0))
		{
			Notify (N.InputOnTouchDown);
		}




		//#endif
	}
}