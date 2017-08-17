using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    
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

    private void Awake()
    {
        instance = this;
        Player.PlayerHasEnteredFinish += LoadScene;
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
                LoadScene();
            }
            //GAME OVER (load first scene)
            else if (gameIsOver)
            {
                Player.isInvisible = false;
                FindObjectOfType<Player>().GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                SceneManager.LoadScene(0);
            }
        }
    }

    void LoadScene()
    {
        Player.isInvisible = false;
        SceneManager.LoadScene(sceneToLoad);
    }

    void ShowGameOverUI()
    {
        //WORK IN PROGRESS
        gameOverText.text = "Game Over";
        gameIsOver = true;
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= ShowGameOverUI;
    }

}
