using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TeethGenerator : MonoBehaviour
{
	public Color TeethColor;
	public Transform Teeth;
	public int TeethCount = 5;
	public float IncresingRadius = 0f;
	public float InitRadius;

	// Use this for initialization
	void OnEnable ()
	{
		float angleRadians;
		float searchRotationRadius = 0f;
		float incresingValue = TeethCount > 0 ? 360f / TeethCount : IncresingRadius;
		Vector3 instantiatePosition = transform.position;
		int i = 1;

		for (int c = 0; c < transform.childCount; c++)
			DestroyImmediate (transform.GetChild(c));

		do
		{
			angleRadians = searchRotationRadius * Mathf.Deg2Rad;//* Mathf.PI / 180.0f;

			// get the 2D dimensional coordinates
			instantiatePosition.x = transform.position.x + InitRadius * Mathf.Cos (angleRadians);
			instantiatePosition.y = transform.position.y + InitRadius * Mathf.Sin (angleRadians);

			Transform teeth = (Transform)Instantiate(Teeth);

			teeth.SetParent(transform);
			teeth.name = string.Format("Teeth_{0:00}",i);

			teeth.position = instantiatePosition;
			//teeth.transform.LookAt(teeth.transform.position - transform.position);
			teeth.eulerAngles = new Vector3(0f, 0f, searchRotationRadius - 90f);

			teeth.GetComponent<SpriteRenderer>().color = TeethColor;

			Debug.LogErrorFormat("instantiatePosition: {0}", instantiatePosition);

			searchRotationRadius += incresingValue ;

			i++;

		} while(searchRotationRadius < 360);

	}

}

