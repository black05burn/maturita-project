using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	#region Variables


    [Header("Game over")]
	public GameObject gameOverUI;
    //public Text gameOverText;
    //bool gameIsOver = false;

    [Header("Blink")]
    public Text cooldownBlinkText;
    public RawImage blinkActive;

    [Header("Invisibility")]
    public Text cooldownInvisibilityText;
    public RawImage invisibilityActive;

    //can bee reached from every class (only one game manager)
    public static Game instance;
	//dev/cheat mode
	public static bool dev = false;
	#endregion

	#region Unity Methods
	private void Awake()
    {
        Player.PlayerHasEnteredFinish += LoadSceneOnLevelFinish;
		Guard.OnGuardHasSpottedPlayer += ShowGameOverUI;
    }

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		//changing scenes
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//DEV MODE (load next scene)
			if (dev)
			{
				LoadSceneOnLevelFinish();
			}
		}

		//dev (cheat) mode activated/deactivated
		if (Input.GetKeyDown(KeyCode.I) && Input.GetKeyDown(KeyCode.L) && Input.GetKeyDown(KeyCode.M))
		{
			dev = !dev;
			print("devMode = " + dev);
		}
	}

	private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= ShowGameOverUI;
		Player.PlayerHasEnteredFinish -= LoadSceneOnLevelFinish;
    }

	#endregion
    void LoadSceneOnLevelFinish()
    {
		SceneManager.LoadScene(0);
    }

    void ShowGameOverUI()
    {
		gameOverUI.SetActive(true);
    }

}
