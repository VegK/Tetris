using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FieldController : MonoBehaviour
{
	#region Properties
	#region Public
	public Vector2 Size = new Vector2(10, 20);
	public Transform StartPointBricks;
	[Header("Bricks")]
	public GameObject BrickI;
	public GameObject BrickJ;
	public GameObject BrickL;
	public GameObject BrickO;
	public GameObject BrickS;
	public GameObject BrickT;
	public GameObject BrickZ;
	#endregion
	#region Private
	private bool[,] _field;
	private BrickType _nextBrick;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		_field = new bool[(int)Size.x, (int)Size.y];

	}

	private void Start()
	{
		
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

	private GameObject GetBrick(BrickType type)
	{
		switch (type)
		{
			default:
			case BrickType.I:
				return BrickI;
			case BrickType.J:
				return BrickJ;
			case BrickType.L:
				return BrickL;
			case BrickType.O:
				return BrickO;
			case BrickType.S:
				return BrickS;
			case BrickType.T:
				return BrickT;
			case BrickType.Z:
				return BrickZ;
		}
	}

	private void CreateBrick()
	{
		var brick = Instantiate(GetBrick(_nextBrick));
		brick.transform.position = StartPointBricks.position;
	}
	#endregion
	#endregion
}