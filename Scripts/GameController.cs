using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public PlayerController player;
    public GameObject backgroundMusicObject;

    AudioSource audioSource;
    public AudioClip loseAlertSound;

    public int intelCount = 0;
    private bool allSecretsCollected = false;
    public bool doorCanOpen { get { return allSecretsCollected; } }

    public bool upgradedSpeed = false;
    public float upgradeSpeed = 3;
    public bool upgradedSight = false;
    public float upgradeSightRadius = 6;

    private string nextLevelToLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void LoseGame(string displayText)
    {
        GameController.instance.PlaySound(loseAlertSound);
        player.MoveAction.Disable();
        UIHandler.instance.ShowLoseScreen(displayText);
        UIHandler.instance.timerIsRunning = false;
    }

    public void WinLevel(string nextLevel, string currentLevel)
    {
        player.MoveAction.Disable();
        UIHandler.instance.timerIsRunning = false;
        UIHandler.instance.ShowWinScreen(currentLevel);
        nextLevelToLoad = nextLevel;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelToLoad);
        if (nextLevelToLoad == "Final_Level")
        {
            UIHandler.instance.ShowEndGameScreen();
        }
        else
        {
            player.MoveAction.Enable();
            UIHandler.instance.timerIsRunning = true;
        }
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        UIHandler.instance.HideInstructions();
        UIHandler.instance.HideEndGameScreen();
        UIHandler.instance.timerIsRunning = true;
        player.MoveAction.Enable();
        StartCoroutine(FlashInstructionBanner());
    }

    public void RestartGame()
    {
        Debug.Log("Restart Game");
        UIHandler.instance.HideLoseScreen();
        UIHandler.instance.HideEndGameScreen();
        UIHandler.instance.ShowInstructions();
        SceneManager.LoadScene("Level_1");
        UIHandler.instance.inventoryMessageLabel.text = "";
        intelCount = 0;
        UIHandler.instance.UpdateIntelCounter();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CheckCheatCodes(string enteredCode)
    {
        switch (enteredCode.ToUpper())
        {
            case "ZOOMIES":
                Debug.Log("move faster");
                UIHandler.instance.inputFeedbackLabel.text = "ZOOMIES accepted. Faster movement activated";
                player.moveSpeed = upgradeSpeed;
                upgradedSpeed = true;
                break;
            case "CAT EYES":
                Debug.Log("see farther");
                UIHandler.instance.inputFeedbackLabel.text = "CAT EYES accepted. Extended vision activated";
                GameObject.Find("Player Light 2D").GetComponent<Light2D>().pointLightOuterRadius = upgradeSightRadius;
                upgradedSight = true;
                break;
            default:
                Debug.Log("upgrade not found");
                UIHandler.instance.inputFeedbackLabel.text = "command not found";
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public IEnumerator FlashInstructionBanner()
    {
        UIHandler.instance.ShowInstructionBanner();
        yield return new WaitForSeconds(10);
        UIHandler.instance.HideInstructionBanner();
    }
}
