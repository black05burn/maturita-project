using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{

	#region Variables
	public SceneFader sceneFader;
	public string menuSceneName = "Main Menu";
	#endregion

	#region Methods for Buttons
	public void Menu()
	{
		//Time.timeScale = 1f;
		sceneFader.FadeTo(menuSceneName);
	}
	#endregion
}
