using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	#region Variables
	public string levelToLoad = "Level 01";
	#endregion

	public void Play()
	{
		SceneManager.LoadScene(levelToLoad);
	}

	public void Quit()
	{
		print("Exiting...");
		Application.Quit();
	}
}
