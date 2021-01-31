using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const string MAIN_MENU = "MainMenu";
    const string UEBER_LEVEL = "Ueberlevel";
    const string LEVEL1 = "Level1";
    const string LEVEL2 = "Level2";
    const string LEVEL3 = "Level3";
    const string LEVEL4 = "Level4";

    [Header("Refs")]
    [SerializeField] GameObject SoulTextsRef;
    [SerializeField] GameObject ButtonsRef;
    [SerializeField] GameObject HowToPlayInfoRef;

    [SerializeField] GameObject StartGameButtonRef;
    [SerializeField] GameObject PauseGameButtonRef;
    [SerializeField] GameObject MiddleButtonRef;
    [SerializeField] GameObject HowToPlayButtonRef;

    [SerializeField] private GameState currentGameState = GameState.MainMenu;

    public enum GameState
    {
        MainMenu,
        Paused,
        Playing,
        HowToPlay
    }

    private void Start()
    {
        currentGameState = GameState.MainMenu;
        UpdateButtonsState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.Playing)
                currentGameState = GameState.Paused;
            else if (currentGameState == GameState.Paused)
                currentGameState = GameState.Playing;
            UpdateButtonsState();
        }
    }

    public void StartGame()
    {
        Debug.Log("Start Game");

        currentGameState = GameState.Playing;
        UpdateButtonsState();

        SceneManager.LoadScene(UEBER_LEVEL);
        SceneManager.LoadScene(LEVEL1, LoadSceneMode.Additive);
        SceneManager.LoadScene(LEVEL2, LoadSceneMode.Additive);
        SceneManager.LoadScene(LEVEL3, LoadSceneMode.Additive);
        SceneManager.LoadScene(LEVEL4, LoadSceneMode.Additive);
    }

    public void SetPause(bool pauseState)
    {
        if (currentGameState == GameState.Playing)
            currentGameState = GameState.Paused;
        else if (currentGameState == GameState.Paused)
            currentGameState = GameState.Playing;        

        UpdateButtonsState();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU);
        currentGameState = GameState.MainMenu;
        UpdateButtonsState();
    }

    public void HowToPlay()
    {
        if (currentGameState == GameState.MainMenu)
            currentGameState = GameState.HowToPlay;
        else if (currentGameState == GameState.HowToPlay)
            currentGameState = GameState.MainMenu;

        UpdateButtonsState();
    }

    private void UpdateButtonsState()
    {
        switch (currentGameState)
        {
            default:
            case GameState.MainMenu:
                Cursor.lockState = CursorLockMode.None;
                SoulTextsRef.SetActive(false);

                StartGameButtonRef.SetActive(true);
                PauseGameButtonRef.SetActive(false);
                MiddleButtonRef.SetActive(false);
                HowToPlayButtonRef.SetActive(true);
                HowToPlayInfoRef.SetActive(false);

                ButtonsRef.SetActive(true);
                break;
            case GameState.Paused:
                Cursor.lockState = CursorLockMode.None;
                HowToPlayButtonRef.SetActive(false);
                HowToPlayInfoRef.SetActive(false);
                SoulTextsRef.SetActive(true);
                StartGameButtonRef.SetActive(false);
                PauseGameButtonRef.SetActive(true);
                MiddleButtonRef.SetActive(true);

                ButtonsRef.SetActive(true);
                break;
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                HowToPlayButtonRef.SetActive(false);
                HowToPlayInfoRef.SetActive(false);
                SoulTextsRef.SetActive(true);
                ButtonsRef.SetActive(false);
                break;
            case GameState.HowToPlay:
                Cursor.lockState = CursorLockMode.None;
                HowToPlayButtonRef.SetActive(true);
                HowToPlayInfoRef.SetActive(true);
                ButtonsRef.SetActive(false);
                break;

        }
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
