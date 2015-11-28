using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
	#region Properties
	#region Public
	public float HorizontalDelta = 1f;
	public float RotateDelta = 1f;
	#endregion
	#region Private
	private float _timePressKeyLeft;
	private float _timePressKeyRight;
	private float _timePressKeyRotate;
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
		ControlKeybord();
	}

	private void ControlKeybord()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			if (Time.time - _timePressKeyLeft >= 0.2f * HorizontalDelta)
			{
				FieldController.Instance.CurrentBrick.MoveLeft();
				_timePressKeyLeft = Time.time;
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			if (Time.time - _timePressKeyRight >= 0.2f * HorizontalDelta)
			{
				FieldController.Instance.CurrentBrick.MoveRight();
				_timePressKeyRight = Time.time;
			}
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			FieldController.Instance.CurrentBrick.SpeedUpFall();
		}
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			if (Time.time - _timePressKeyRotate >= 0.2f * RotateDelta)
			{
				FieldController.Instance.CurrentBrick.Rotate();
				_timePressKeyRotate = Time.time;
			}
		}
	}
	#endregion
	#endregion
}