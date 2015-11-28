using UnityEngine;

public class BrickZController : BrickController
{
	#region Properties
	#region Public

	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public
	public override void Rotate()
	{
		var ang = transform.rotation.eulerAngles;
		ang.z = (Mathf.RoundToInt(ang.z) == 0) ? 90 : 0;

		if (!AvailableRotateInField(ang))
			return;

		var rot = transform.rotation;
		rot.eulerAngles = ang;
		transform.rotation = rot;

		CalcLastBoxes();
		var trend = ExitBeyondField(0);
		if (trend != 0)
		{
			var pos = transform.localPosition;
			var angZ = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
			if (angZ == 0)
				pos.x += trend;
			else if (angZ == 90)
				pos.y += trend;
			transform.localPosition = pos;
		}
	}
	#endregion
	#region Private

	#endregion
	#endregion
}