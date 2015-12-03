using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
	#region Properties
	#region Public

	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Brick")
		{
			var box = other as BoxCollider;
			var posY = Mathf.RoundToInt(other.transform.position.y + box.center.y);
			if (posY > transform.position.y)
			{
				var ctrl = other.GetComponent<BrickController>();
				if (ctrl != null)
					FieldController.Instance.FixBrick(ctrl);
			}
		}
	}
	#endregion
	#endregion
}