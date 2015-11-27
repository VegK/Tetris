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
	private float _speedUpFall = 1;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void DetachBoxes()
	{
		var childs = GetBoxes();
		foreach (GameObject obj in childs)
		{
			obj.transform.SetParent(transform.parent);

			var collider = obj.GetComponent<BoxCollider>();
			if (collider != null)
				collider.enabled = true;
		}

		transform.DetachChildren();
		Destroy(gameObject);
	}

	public GameObject[] GetBoxes()
	{
		var count = transform.childCount;
		var res = new GameObject[count];

		for (int i = 0; i < count; i++)
			res[i] = transform.GetChild(i).gameObject;

		return res;
	}

	public void MoveLeft()
	{
		var pos = transform.localPosition;
		pos.x -= 1;
		transform.localPosition = pos;
	}

	public void MoveRight()
	{
		var pos = transform.localPosition;
		pos.x += 1;
		transform.localPosition = pos;
	}

	public void SpeedUpFall()
	{
		_speedUpFall = 3;
	}
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
		speed.y *= SpeedFall * _speedUpFall * Time.deltaTime;
		transform.Translate(speed);
		_speedUpFall = 1;
	}
	#endregion
	#endregion
}