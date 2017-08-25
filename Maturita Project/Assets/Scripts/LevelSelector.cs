using UnityEngine;

public class LevelSelector : MonoBehaviour {

	#region Variables
	public SceneFader sceneFader;
	#endregion

	#region Unity Methods
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
