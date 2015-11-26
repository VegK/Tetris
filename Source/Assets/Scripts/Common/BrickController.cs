using UnityEngine;
using System.Collections;

public class BrickController : MonoBehaviour
{
	#region Properties
	#region Public
	public BrickType TypeBrick;

	public float SpeedFall { get; set; }
	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Start()
	{

	}

	private void Update()
	{
		Fall();
	}

	private void Fall()
	{
		var speed = Vector3.down;
		speed.y *= SpeedFall * Time.deltaTime;
		transform.Translate(speed);
	}
	#endregion
	#endregion
}