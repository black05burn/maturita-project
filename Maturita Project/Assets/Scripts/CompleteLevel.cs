using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour {

	#region Variables
	public SceneFader sceneFader;
	public string menuSceneName = "Main Menu";

	public string nextLevel = "Level 02";
	public int levelToUnlock = 2;
	#endregion

	private void Start()
	{
		Destroy(FindObjectOfType<Player>().gameObject);
	}

	#region Methods for Buttons
	public void Continue()
	{
		//Time.timeScale = 1f;
		PlayerPrefs.SetInt("levelReached", levelToUnlock);
		sceneFader.FadeTo(nextLevel);
	}

	public void Menu()
	{
		//Time.timeScale = 1f;
		sceneFader.FadeTo(menuSceneName);
	}
	#endregion
}
