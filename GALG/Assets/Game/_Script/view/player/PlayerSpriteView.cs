using UnityEngine;
using System.Collections;

public class PlayerSpriteView : View<Game>
{
	private void Update()
	{
		//transform.Rotate (new Vector3(0f, 0f, 10f));
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		//Debug.Log (other.transform.name + " " + game.model.obstacleFactoryModel.obstacleModelsDictionary[other.transform.GetComponentInParent<ObstacleView>()]);
		if (other.transform.GetComponent<ObstacleView> ())
			Notify(N.GamePlayerImpactObstacle__, other.transform.GetComponent<ObstacleView>(), other.contacts[0].point);
		else
			if(other.transform.parent)
				if(other.transform.parent.GetComponent<ObstacleView>())
					Notify(N.GamePlayerImpactObstacle__, other.transform.GetComponentInParent<ObstacleView>(), other.contacts[0].point);
	}

}
