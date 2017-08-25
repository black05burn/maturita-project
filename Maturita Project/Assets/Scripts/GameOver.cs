using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
	#region Variables
	public SceneFader sceneFader;
	public string menuSceneName = "Main Menu";
	#endregion


	#region Methods for Buttons
	public void Retry()
	{
		sceneFader.FadeTo(SceneManager.GetActiveScene().name);
	}

	public void Menu()
	{
		sceneFader.FadeTo(menuSceneName);
	}
	#endregion
}
