using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	#region Variables
	public int sceneToLoad; //NEEDS CHANGING

    [Header("Game over")]
    public Text gameOverText;
    bool gameIsOver = false;

    [Header("Blink")]
    public Text cooldownBlinkText;
    public RawImage blinkActive;

    [Header("Invisibility")]
    public Text cooldownInvisibilityText;
    public RawImage invisibilityActive;

    //can bee reached from every class (only one game manager)
    public static Game instance;
	#endregion

	#region Unity Methods
	private void Awake()
    {
        instance = this;
        Player.PlayerHasEnteredFinish += LoadSceneOnLevelFinish;
		Guard.OnGuardHasSpottedPlayer += ShowGameOverUI;
    }

    private void Update()
    {
        //changing scenes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //DEV MODE (load next scene)
            if (Player.dev)
            {
                LoadSceneOnLevelFinish();
            }
            //GAME OVER (load first scene)
            else if (gameIsOver)
            {
                SceneManager.LoadScene(0);
            }
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
		Player.isInvisible = false;
		FindObjectOfType<Player>().GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		SceneManager.LoadScene(sceneToLoad);
    }

    void ShowGameOverUI()
    {
        //WORK IN PROGRESS
        gameOverText.text = "Game Over";
        gameIsOver = true;
    }


}
