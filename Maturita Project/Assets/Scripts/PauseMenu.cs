using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	#region Variables
	public GameObject UI;
	public SceneFader sceneFader;
	public string menuSceneName = "Main Menu";
	#endregion

	#region Unity Methods

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
		{
			Toggle();
		}
	}

	public void Toggle()
	{
		UI.SetActive(!UI.activeSelf);

		if (UI.activeSelf)
		{
			//freeze time
			Time.timeScale = 0f;
		} else
		{
			Time.timeScale = 1f;
		}
	}

	#endregion

	public void Retry()
	{
		Toggle();
		sceneFader.FadeTo(SceneManager.GetActiveScene().name);
	}

	public void Menu()
	{
		Toggle();
		sceneFader.FadeTo(menuSceneName);
	}

}
