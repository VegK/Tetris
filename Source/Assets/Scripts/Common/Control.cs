using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
	#region Properties
	#region Public
	public float HorizontalDelta = 1f;
	#endregion
	#region Private
	private float _timePressKey;
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
			if (Time.time - _timePressKey >= 0.2f * HorizontalDelta)
			{
				FieldController.Instance.CurrentBrick.MoveLeft();
				_timePressKey = Time.time;
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			if (Time.time - _timePressKey >= 0.2f * HorizontalDelta)
			{
				FieldController.Instance.CurrentBrick.MoveRight();
				_timePressKey = Time.time;
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			FieldController.Instance.CurrentBrick.SpeedUpFall();
		}
	}
	#endregion
	#endregion
}