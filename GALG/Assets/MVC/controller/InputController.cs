using UnityEngine;
using System.Collections;

public class InputController : Controller
{
	private bool _isStationaryStarted = false;

	void OnTap( TapGesture gesture ) 
	{
		Debug.LogError ("Taped " + gesture.State);
		if(gesture.StartSelection != null && gesture.Raycast.Hit2D.rigidbody != null)
			Notify (N.InputOnDrag___, gesture.StartSelection, gesture.Raycast.Hit2D.centroid, gesture.State);
	}

	void OnFingerDown( FingerDownEvent e ) 
	{
		Debug.Log( e.Finger + " Down at " + e.Position + " on object:" + e.Selection + " ");

	}

	void OnFingerMove( FingerMotionEvent e ) 
	{
		float elapsed = e.ElapsedTime;

		if( e.Phase == FingerMotionPhase.Started )
			Debug.Log( e.Finger + " started moving at " + e.Position);
		else if( e.Phase == FingerMotionPhase.Updated )
			Debug.Log( e.Finger + " moving at " + e.Position );
		if( e.Phase == FingerMotionPhase.Ended )
			Debug.Log( e.Finger + " stopped moving at " + e.Position );
		
		if(e.Selection != null && e.Raycast.Hit2D.rigidbody != null)
			Notify (N.InputOnDrag___, e.Selection, e.Raycast.Hit2D.centroid, e.Phase);
	}

	void OnFingerStationary( FingerMotionEvent e ) 
	{
		float elapsed = e.ElapsedTime;

		if (e.Phase == FingerMotionPhase.Started)
		{
			Debug.Log (e.Finger + " started stationary state at " + e.Raycast.Hit2D.centroid);
			_isStationaryStarted = true;
		}
		else if( e.Phase == FingerMotionPhase.Updated )
			Debug.Log( e.Finger + " is still stationary at " + e.Position );
		else if( e.Phase == FingerMotionPhase.Ended )
			Debug.Log( e.Finger + " stopped being stationary at " + e.Position );


		if (_isStationaryStarted && e.Phase == FingerMotionPhase.Ended)
			return;

			if (e.Selection != null && e.Raycast.Hit2D.rigidbody != null)
				Notify (N.InputOnDrag___, e.Selection, e.Raycast.Hit2D.centroid, e.Phase);
		
	}

	void OnFingerUp( FingerUpEvent e ) 
	{
		if (_isStationaryStarted)
		{
			_isStationaryStarted = false;
			Notify (N.InputOnDrag___, e.Selection, e.Raycast.Hit2D.centroid, FingerMotionPhase.Ended);
		}
	}

	void OnDrag( DragGesture gesture ) 
	{
		// current gesture phase (Started/Updated/Ended)
		ContinuousGesturePhase phase = gesture.Phase;

		// Drag/displacement since last frame
		Vector2 deltaMove = gesture.DeltaMove;

		// Total drag motion from initial to current position
		Vector2 totalMove = gesture.TotalMove;

		if(gesture.StartSelection != null && gesture.Raycast.Hit2D.rigidbody != null)
			Notify (N.InputOnDrag___, gesture.StartSelection, gesture.Raycast.Hit2D.centroid, gesture.Phase);
	}
}