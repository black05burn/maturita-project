using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	#region Variables
	public SceneFader sceneFader;
	public string mainMenu = "Main Menu";
	public string levelSelect = "Level Select";

	public Button[] levelButtons;
	#endregion

	#region Unity Methods

	private void Start()
	{
		int levelReached = PlayerPrefs.GetInt("levelReached", 1);
		for (int i = levelReached; i < levelButtons.Length; i++)
		{
			levelButtons[i].interactable = false;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			sceneFader.FadeTo(mainMenu);
		}
	}
	#endregion

	public void Select(string levelName)
	{
		sceneFader.FadeTo(levelName);
	}

	public void ResetProgress()
	{
		PlayerPrefs.SetInt("levelReached", 1);
		sceneFader.FadeTo(levelSelect);
	}
}
