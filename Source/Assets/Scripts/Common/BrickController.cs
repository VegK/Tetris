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