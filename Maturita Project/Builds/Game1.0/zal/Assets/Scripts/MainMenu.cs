using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	#region Variables
	public string levelToLoad = "Level 01";
	public SceneFader sceneFader;
	#endregion

	public void Play()
	{
		sceneFader.FadeTo(levelToLoad);
	}

	public void Quit()
	{
		print("Exiting...");
		Application.Quit();
	}
}
