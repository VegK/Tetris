using UnityEngine;
using System.Linq;

public class BrickController : MonoBehaviour
{
	#region Properties
	#region Public
	public BrickType TypeBrick;

	public float SpeedFall { get; set; }
	#endregion
	#region Private
	private GameObject[] _bufferBoxes;
	private GameObject _lastLeftBox;
	private GameObject _lastRightBox;
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
		if (_bufferBoxes != null)
			return _bufferBoxes;

		var count = transform.childCount;

		_bufferBoxes = new GameObject[count];
		for (int i = 0; i < count; i++)
		{
			var child = transform.GetChild(i);
			if (child.tag == "Box")
				_bufferBoxes[i] = child.gameObject;
		}

		return _bufferBoxes;
	}

	public void MoveLeft()
	{
		if (ExitBeyondField(-1) != 0)
			return;

		var pos = transform.localPosition;
		pos.x -= 1;
		transform.localPosition = pos;
	}

	public void MoveRight()
	{
		if (ExitBeyondField(1) != 0)
			return;

		var pos = transform.localPosition;
		pos.x += 1;
		transform.localPosition = pos;
	}

	public void SpeedUpFall()
	{
		_speedUpFall = 3;
	}

	public void Rotate()
	{
		var ang = transform.rotation.eulerAngles;
		ang.z += 90;

		var rot = transform.rotation;
		rot.eulerAngles = ang;
		transform.rotation = rot;

		CalcLastBoxes();
		var trend = ExitBeyondField(0);
		if (trend != 0)
		{
			var pos = transform.localPosition;
			var angZ = (int)transform.rotation.eulerAngles.z;
			if (angZ == 0)
				pos.x += trend;
			else if (angZ == 90)
				pos.y += trend;
			else if (angZ == 180)
				pos.x -= trend;
			else if (angZ == 270)
				pos.y -= trend;
			transform.localPosition = pos;
		}
	}
	#endregion
	#region Private
	private void Awake()
	{
		CalcLastBoxes();
	}

	private void Start()
	{

	}

	private void Update()
	{
		Fall();
	}

	private void Fall()
	{
		var speed = SpeedFall * _speedUpFall * Time.deltaTime;

		var direction = Vector3.zero;
		var angZ = (int)transform.rotation.eulerAngles.z;
		if (angZ == 0)
		{
			direction = Vector3.down;
			direction.y *= speed;
		}
		else if (angZ == 90)
		{
			direction = Vector3.left;
			direction.x *= speed;
		}
		else if (angZ == 180)
		{
			direction = Vector3.up;
			direction.y *= speed;
		}
		else if (angZ == 270)
		{
			direction = Vector3.right;
			direction.x *= speed;
		}

		transform.Translate(direction);
		_speedUpFall = 1;
	}

	/// <summary>
	/// Check output brick beyond field. Return 0 not output, -1 if output of left, 1 if output of right.
	/// </summary>
	/// <param name="diff">Difference</param>
	/// <returns>0 not output, -1 output of left, 1 output of right</returns>
	private int ExitBeyondField(int diff)
	{
		if (_lastRightBox.transform.position.x + diff >= FieldController.Instance.Size.x)
			return 1;
		if (_lastLeftBox.transform.position.x + diff < 0)
			return -1;
		return 0;
	}

	private void CalcLastBoxes()
	{
		var boxes = GetBoxes();
		var angZ = (int)transform.rotation.eulerAngles.z;

		if (angZ == 0)
		{
			_lastLeftBox = boxes.Aggregate((a, b) => a.transform.localPosition.x < b.transform.localPosition.x ? a : b);
			_lastRightBox = boxes.Aggregate((a, b) => a.transform.localPosition.x > b.transform.localPosition.x ? a : b);
		}
		else if (angZ == 90)
		{
			_lastLeftBox = boxes.Aggregate((a, b) => a.transform.localPosition.y > b.transform.localPosition.y ? a : b);
			_lastRightBox = boxes.Aggregate((a, b) => a.transform.localPosition.y < b.transform.localPosition.y ? a : b);
		}
		else if (angZ == 180)
		{
			_lastLeftBox = boxes.Aggregate((a, b) => a.transform.localPosition.x > b.transform.localPosition.x ? a : b);
			_lastRightBox = boxes.Aggregate((a, b) => a.transform.localPosition.x < b.transform.localPosition.x ? a : b);
		}
		else if (angZ == 270)
		{
			_lastLeftBox = boxes.Aggregate((a, b) => a.transform.localPosition.y < b.transform.localPosition.y ? a : b);
			_lastRightBox = boxes.Aggregate((a, b) => a.transform.localPosition.y > b.transform.localPosition.y ? a : b);
		}
	}
	#endregion
	#endregion
}