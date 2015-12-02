using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FieldController : MonoBehaviour
{
	#region Properties
	#region Public
	public Vector2 Size = new Vector2(10, 20);
	public float SpeedOnOneLevel = 2f;
	public Transform StartPointBricks;
	[Header("Bricks")]
	public BrickController PrefabBrickI;
	public BrickController PrefabBrickJ;
	public BrickController PrefabBrickL;
	public BrickController PrefabBrickO;
	public BrickController PrefabBrickS;
	public BrickController PrefabBrickT;
	public BrickController PrefabBrickZ;

	public static FieldController Instance;
	public event EventHandler FixedBrickBeforeEvent;
	public event EventHandler FixedBrickAfterEvent;
	public BrickController CurrentBrick { get; private set; }
	public int Level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
		}
	}
	#endregion
	#region Private
	private int _level;
	private GameObject[,] _field;
	private BrickType _nextBrick;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	public void FixBrick(BrickController brick)
	{
		StartCoroutine(FixBrickDelay(brick));
	}

	public bool CheckCellField(int x, int y)
	{
		if (x < 0 || x >= Size.x || y < 0 || y >= Size.y)
			return false;
		return _field[x, y] == null;
	}
	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;

		var box = gameObject.AddComponent<BoxCollider>();
		box.isTrigger = true;

		var boxSize = box.size;
		boxSize.x = Size.x;
		box.size = boxSize;

		var boxCenter = box.center;
		boxCenter.x = Size.x / 2 - 0.5f;
		boxCenter.y = -1f;
		box.center = boxCenter;

		_field = new GameObject[(int)Size.x, (int)Size.y];

		if (Level == 0)
			Level = 1;
	}

	private void Start()
	{
		GameGUI.Instance.Level = Level;

		_nextBrick = GetNextBrick();
		NewBrick();
	}
	
	private void Update()
	{

	}

	private void NewBrick()
	{
		CreateBrick();

		_nextBrick = GetNextBrick();
		GameGUI.Instance.SetNextBrick(_nextBrick);
	}

	private void ClearField()
	{
		for (int x = 0; x < _field.GetLength(0); x++)
			for (int y = 0; y < _field.GetLength(1); y++)
				_field[x, y] = null;

		var count = transform.childCount;
		for (int i = 0; i < count; i++)
			Destroy(transform.GetChild(i).gameObject);
	}

	private BrickType GetNextBrick()
	{
		var bricks = Enum.GetValues(typeof(BrickType));
		return (BrickType)bricks.GetValue(UnityEngine.Random.Range(0, bricks.Length));
	}

	private BrickController GetBrick(BrickType type)
	{
		switch (type)
		{
			default:
			case BrickType.I:
				return PrefabBrickI;
			case BrickType.J:
				return PrefabBrickJ;
			case BrickType.L:
				return PrefabBrickL;
			case BrickType.O:
				return PrefabBrickO;
			case BrickType.S:
				return PrefabBrickS;
			case BrickType.T:
				return PrefabBrickT;
			case BrickType.Z:
				return PrefabBrickZ;
		}
	}

	private void CreateBrick()
	{
		var brick = Instantiate(GetBrick(_nextBrick));
		brick.transform.SetParent(transform);
		brick.transform.position = StartPointBricks.position;
		brick.SpeedFall = Level * SpeedOnOneLevel;
		CurrentBrick = brick;
	}

	private IEnumerator FixBrickDelay(BrickController brick)
	{
		if (brick.SpeedFall == 0)
			yield break;

		if (FixedBrickBeforeEvent != null)
			FixedBrickBeforeEvent(this, null);

		brick.SpeedFall = 0;

		var boxes = brick.GetBoxes();
		brick.DetachBoxes();

		foreach (GameObject box in boxes)
		{
			var pos = box.transform.localPosition;

			var x = Mathf.RoundToInt(pos.x);
			var y = Mathf.RoundToInt(pos.y);

			if (x < 0 || x >= Size.x || y < 0 || y >= Size.y)
			{
				Destroy(box);
				continue;
			}

			pos.x = x;
			pos.y = y;
			box.transform.localPosition = pos;

			_field[x, y] = box;
		}

		if (GameOver())
			yield break;

		yield return StartCoroutine(DestroyLines());
		NewBrick();

		if (FixedBrickAfterEvent != null)
			FixedBrickAfterEvent(this, null);
	}

	private IEnumerator DestroyLines()
	{
		var countX = _field.GetLength(0);
		var countY = _field.GetLength(1);

		for (int y = 0; y < countY; y++)
		{
			var fullCells = 0;
			for (int x = 0; x < countX; x++)
				if (_field[x, y] != null)
					fullCells++;

			if (fullCells == countX)
			{
				for (int x = 0; x < countX; x++)
				{
					Destroy(_field[x, y]);
					_field[x, y] = null;
				}
				DestroyLine();
			}
		}

		var fullPrefCells = 0;
		for (int y = 0; y < countY; y++)
		{
			if (fullPrefCells == countX)
			{
				var boxesDown = new List<GameObject>();
				for (int y2 = y; y2 < countY; y2++)
					for (int x2 = 0; x2 < countX; x2++)
					{
						if (_field[x2, y2] == null)
							continue;

						boxesDown.Add(_field[x2, y2]);
						_field[x2, y2 - 1] = _field[x2, y2];
						_field[x2, y2] = null;
					}

				// TODO: add animation drop
				foreach (GameObject box in boxesDown)
				{
					var pos = box.transform.localPosition;
					pos.y -= 1;
					box.transform.localPosition = pos;
				}

				if (boxesDown.Count > 0)
				{
					y--;
					yield return new WaitForSeconds(0.3f);
				}
			}

			fullPrefCells = 0;
			for (int x = 0; x < countX; x++)
				if (_field[x, y] == null)
					fullPrefCells++;
		}
	}

	private void DestroyLine()
	{
		GameGUI.Instance.Lines++;
		GameGUI.Instance.Score += 5;
	}

	private bool GameOver()
	{
		var res = false;
		var countX = Mathf.RoundToInt(Size.x);
		var countY = Mathf.RoundToInt(Size.y);

		for (int x = 0; x < countX; x++)
			if (_field[x, countY - 1] != null)
			{
				res = true;
				break;
			}

		if (res)
		{

		}

		return res;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Brick")
		{
			FixBrick(CurrentBrick);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		for (int x = 0; x < (int)Size.x; x++)
		{
			for (int y = 0; y < (int)Size.y; y++)
			{
				var pos = transform.position;
				pos.x += x;
				pos.y += y;
				Gizmos.DrawWireCube(pos, Vector2.one);

				if (_field != null && _field[x, y] != null)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawCube(pos, Vector2.one);
					Gizmos.color = Color.red;
				}
			}
		}
	}
	#endregion
	#endregion
}