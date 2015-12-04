using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
	#region Properties
	#region Public
	public float Speed { get; set; }

	public event EventHandler DestroyEvent;
	#endregion
	#region Private
	private RectTransform _rect;
	private float _leftEdge;
	#endregion
	#endregion

	#region Methods
	#region Public
	
	#endregion
	#region Private
	private void Start()
	{
		_rect = gameObject.GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (transform.position.x > Camera.main.transform.position.x)
		{
			_leftEdge = _rect.position.x - _rect.sizeDelta.x / 2;
			if (_leftEdge >= Screen.width)
				Destroy(gameObject);
		}

		transform.Translate(Vector2.right * Speed * Time.deltaTime);
	}

	private void OnDestroy()
	{
		if (DestroyEvent != null)
			DestroyEvent(this, null);
	}
	#endregion
	#endregion
}