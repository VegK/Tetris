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
			_bufferBoxes[i] = transform.GetChild(i).gameObject;

		return _bufferBoxes;
	}

	public void MoveLeft()
	{
		if (ExitBeyondField(-1))
			return;

		var pos = transform.localPosition;
		pos.x -= 1;
		transform.localPosition = pos;
	}

	public void MoveRight()
	{
		if (ExitBeyondField(1))
			return;

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
	private void Awake()
	{
		var boxes = GetBoxes();
		_lastLeftBox = boxes.Aggregate((a, b) => a.transform.localPosition.x < b.transform.localPosition.x ? a : b);
		_lastRightBox = boxes.Aggregate((a, b) => a.transform.localPosition.x > b.transform.localPosition.x ? a : b);
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
		var speed = Vector3.down;
		speed.y *= SpeedFall * _speedUpFall * Time.deltaTime;
		transform.Translate(speed);
		_speedUpFall = 1;
	}

	private bool ExitBeyondField(int diff)
	{
		var res = (_lastRightBox.transform.position.x + diff >= FieldController.Instance.Size.x);
		res |= (_lastLeftBox.transform.position.x + diff < 0);
		return res;
	}
	#endregion
	#endregion
}