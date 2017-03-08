using UnityEngine;
using System.Collections;

public class GearView : View
{
	public GearModel gearModel;

	void Start()
	{
		gearModel = game.model.gearsFactoryModel.gearsDictionary [this];
	}

}
