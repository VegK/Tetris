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
	private bool[,] _field;
	private BrickType _nextBrick;
	private BrickController _currentBrick;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		var box = gameObject.AddComponent<BoxCollider>();
		box.isTrigger = true;

		var boxSize = box.size;
		boxSize.x = Size.x;
		box.size = boxSize;

		var boxCenter = box.center;
		boxCenter.x = Size.x / 2 - 0.5f;
		boxCenter.y = -1f;
		box.center = boxCenter;

		_field = new bool[(int)Size.x, (int)Size.y];

		if (Level == 0)
			Level = 1;
	}

	private void Start()
	{
		_nextBrick = GetNextBrick();
		CreateBrick();

		_nextBrick = GetNextBrick();
	}
	
	private void Update()
	{

	}

	private void ClearField()
	{
		for (int x = 0; x < _field.GetLength(0); x++)
			for (int y = 0; y < _field.GetLength(1); y++)
				_field[x, y] = false;

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
		_currentBrick = brick;
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Brick")
		{
			_currentBrick.SpeedFall = 0;

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
			}
		}
	}
	#endregion
	#endregion
}