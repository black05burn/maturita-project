using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	#region Variables
	public GameObject UI;
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

	public void Retry()
	{
		Toggle();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Menu()
	{
		print("Main Menu");
	}

	#endregion
}
