using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	#region Variables
	public SceneFader sceneFader;

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
			sceneFader.FadeTo("Main Menu");
		}
	}
	#endregion

	public void Select(string levelName)
	{
		sceneFader.FadeTo(levelName);
	}
}
