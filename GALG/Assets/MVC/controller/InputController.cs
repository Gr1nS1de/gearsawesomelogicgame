using UnityEngine;
using System.Collections;

public class InputController : Controller
{

	void OnDrag( DragGesture gesture ) 
	{
		// current gesture phase (Started/Updated/Ended)
		ContinuousGesturePhase phase = gesture.Phase;

		// Drag/displacement since last frame
		Vector2 deltaMove = gesture.DeltaMove;

		// Total drag motion from initial to current position
		Vector2 totalMove = gesture.TotalMove;

		if(gesture.Selection != null)
			Notify (N.InputOnDrag__, gesture.Selection, gesture.Raycast.Hit2D.point);
	}
}