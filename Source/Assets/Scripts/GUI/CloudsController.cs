using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudsController : MonoBehaviour
{
	#region Properties
	#region Public
	public Sprite[] Sprites;
	public int MaxCloud = 3;
	[Header("Time spawn cloud (seconds)")]
	public int TimeSpawnMin = 5;
	public int TimeSpawnMax = 10;
	[Header("Speed cloud")]
	public int SpeedMin = 2;
	public int SpeedMax = 5;

	public static CloudsController Instance;
	#endregion
	#region Private
	private System.Random _random;
	private int _cloudsCount;
	private bool _spawn;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;
		_random = new System.Random();
		if (Sprites.Length == 0)
			gameObject.SetActive(false);
	}

	private void Start()
	{
		for (int i = 0; i < MaxCloud; i++)
		{
			var sprite = Sprites[_random.Next(0, Sprites.Length)];
			CreateCloud(sprite, true);
		}
	}

	private void StartSpawn()
	{
		if (!gameObject.activeInHierarchy)
			return;
		if (_spawn)
			return;
		_spawn = true;
		StartCoroutine(Spawn());
	}

	private IEnumerator Spawn()
	{
		while (true)
		{
			if (_cloudsCount >= MaxCloud)
			{
				_spawn = false;
				yield break;
			}

			var sec = Random.Range(TimeSpawnMin, TimeSpawnMax);
			yield return new WaitForSeconds(sec);

			var sprite = Sprites[Random.Range(0, Sprites.Length)];
			CreateCloud(sprite, false);
		}
	}

	private void CreateCloud(Sprite sprite, bool randomPositionX)
	{
		var obj = new GameObject();
		obj.name = sprite.name;
		obj.transform.SetParent(transform);
		var pos = Vector2.zero;
		pos.y = _random.Next(-5, 24) * 10;
		obj.transform.localPosition = pos;
		if (randomPositionX)
		{
			pos = obj.transform.position;
			pos.x = _random.Next(0, Screen.width);
			obj.transform.position = pos;
		}
		obj.transform.localScale = Vector3.one;

		var img = obj.AddComponent<Image>();
		img.sprite = sprite;
		img.SetNativeSize();

		var cloud = obj.AddComponent<CloudController>();
		cloud.DestroyEvent += (o, e) => DestroyCloud();
		cloud.Speed = _random.Next(SpeedMin, SpeedMax) * 10;
		_cloudsCount++;
	}

	private void DestroyCloud()
	{
		_cloudsCount--;
		StartSpawn();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 5f);
	}
	#endregion
	#endregion
}