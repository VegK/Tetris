using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdditiveScene : MonoBehaviour
{
	#region Properties
	#region Public
	public string Scene = "GameGUI";

	public LoadEvent BeforeLoad = new LoadEvent();
	public LoadEvent AfterLoad = new LoadEvent();
	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private IEnumerator Start()
	{
		BeforeLoad.Invoke();
		yield return SceneManager.LoadSceneAsync(Scene, LoadSceneMode.Additive);
		AfterLoad.Invoke();
	}
	#endregion
	#endregion
}

[Serializable]
public class LoadEvent : UnityEvent
{

}