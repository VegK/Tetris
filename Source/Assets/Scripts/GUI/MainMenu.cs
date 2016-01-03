using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	#region Properties
	#region Public
	
	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnClickStart()
	{
		SceneManager.LoadScene("Game");
	}
	#endregion
	#region Private

	#endregion
	#endregion
}