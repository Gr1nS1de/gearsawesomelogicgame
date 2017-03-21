using UnityEngine;
using System.Collections;

public class GearView : View
{
	[SerializeField]private GearModel gearModel;

	void Start()
	{
		gearModel = game.model.gearsFactoryModel.gearsDictionary [this];
	}

}
