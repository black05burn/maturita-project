using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    
    public int sceneToLoad;
    public Text gameOverText;
    public Text cooldownBlinkText;
    public RawImage blinkActive;
    public Text cooldownInvisibilityText;
    public RawImage invisibilityActive;

    bool gameIsOver = false;

    public static Game instance;

    private void Awake()
    {
        instance = this;
        Player.PlayerHasEnteredFinish += LoadScene;
        Guard.OnGuardHasSpottedPlayer += ShowGameOverUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Player.dev)
            {
                LoadScene();
            }
            else if (gameIsOver)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void ShowGameOverUI()
    {
        gameOverText.text = "Game Over";
        gameIsOver = true;
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= ShowGameOverUI;
    }

}
