using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{
	#region Properties
	#region Public
	[Header("Game")]
	public GameObject UIParentGame;
	public Text UILevelValue;
	public Text UIScoreValue;
	public Text UILinesValue;
	public GameObject UINextBrickPanel;
	[Header("Pause")]
	public GameObject UIParentPause;
	[Header("Game over")]
	public GameObject UIParentGameOver;
	public Text UIFinalLevelValue;
	public Text UIFinalScoreValue;
	public Text UIFinalLinesValue;
	[Header("Bricks")]
	public BrickController PrefabBrickI;
	public BrickController PrefabBrickJ;
	public BrickController PrefabBrickL;
	public BrickController PrefabBrickO;
	public BrickController PrefabBrickS;
	public BrickController PrefabBrickT;
	public BrickController PrefabBrickZ;

	public static GameGUI Instance;
	public event EventHandler RestartEvent;
	public int Level
	{
		set
		{
			_level = value;
			UILevelValue.text = _level.ToString();
		}
		get
		{
			return _level;
		}
	}
	public int Score
	{
		set
		{
			_score = value;
			UIScoreValue.text = _score.ToString();
		}
		get
		{
			return _score;
		}
	}
	public int Lines
	{
		set
		{
			_lines = value;
			UILinesValue.text = _lines.ToString();
		}
		get
		{
			return _lines;
		}
	}
	#endregion
	#region Private
	private int _level;
	private int _score;
	private int _lines;
	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnClickPause()
	{
		FieldController.Instance.Pause = true;
		ShowPauseGUI();
	}

	public void OnClickPlay()
	{
		FieldController.Instance.Pause = false;
		ShowGameGUI();
	}

	public void OnClickRestart()
	{
		ShowGameGUI();
		if (RestartEvent != null)
			RestartEvent(this, EventArgs.Empty);
	}

	public void OnClickMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void OnClickShareFacebook()
	{
		// TODO: share facebook
	}

	public void ShowGameGUI()
	{
		UIParentGame.SetActive(true);
		UIParentPause.SetActive(false);
		UIParentGameOver.SetActive(false);
	}

	public void ShowGameOverGUI()
	{
		UIFinalLevelValue.text = Level.ToString();
		UIFinalScoreValue.text = Score.ToString();
		UIFinalLinesValue.text = Lines.ToString();

		UIParentGame.SetActive(false);
		UIParentPause.SetActive(false);
		UIParentGameOver.SetActive(true);
	}

	public void SetNextBrick(BrickType type)
	{
		BrickController brick;
		switch (type)
		{
			default:
			case BrickType.I:
				brick = PrefabBrickI;
				break;
			case BrickType.J:
				brick = PrefabBrickJ;
				break;
			case BrickType.L:
				brick = PrefabBrickL;
				break;
			case BrickType.O:
				brick = PrefabBrickO;
				break;
			case BrickType.S:
				brick = PrefabBrickS;
				break;
			case BrickType.T:
				brick = PrefabBrickT;
				break;
			case BrickType.Z:
				brick = PrefabBrickZ;
				break;
		}

		var childsCount = UINextBrickPanel.transform.childCount;
		for (int i = 0; i < childsCount; i++)
			Destroy(UINextBrickPanel.transform.GetChild(i).gameObject);

		var obj = Instantiate(brick);
		obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
		var offsetX = 0.0f;
		var offsetY = 0.0f;
		switch (obj.TypeBrick)
		{
			case BrickType.I:
				offsetX = -0.33f;
				break;
			case BrickType.J:
				offsetY = -0.5f;
				break;
			case BrickType.L:
				offsetY = 0.33f;
				break;
			case BrickType.O:
				offsetX = 0.33f;
				offsetY = -0.33f;
				break;
			case BrickType.S:
				offsetY = -0.5f;
				break;
			case BrickType.T:
				offsetY = -0.5f;
				break;
			case BrickType.Z:
				offsetY = -0.5f;
				break;
		}
		var pos = Camera.main.ScreenToWorldPoint(UINextBrickPanel.transform.position);
		pos.x += offsetX;
		pos.y += offsetY;
		pos.z = -1;
		obj.transform.position = pos;
		obj.transform.SetParent(UINextBrickPanel.transform);
	}
	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;
	}

	private void ShowPauseGUI()
	{
		UIParentGame.SetActive(false);
		UIParentPause.SetActive(true);
		UIParentGameOver.SetActive(false);
	}
	#endregion
	#endregion
}