using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    const string MAIN_MENU = "MainMenu";
    const string UEBER_LEVEL = "Ueberlevel";
    const string LEVEL1 = "Level1";
    const string LEVEL2 = "Level2";
    const string LEVEL3 = "Level3";
    const string LEVEL4 = "Level4";
    const string LEVEL5 = "Level5";

    [Header("Refs")]
    [SerializeField] GameObject SoulTextsRef;
    [SerializeField] GameObject ButtonsRef;
    [SerializeField] GameObject HowToPlayInfoRef;

    [SerializeField] GameObject StartGameButtonRef;
    [SerializeField] GameObject PauseGameButtonRef;
    [SerializeField] GameObject MiddleButtonRef;
    [SerializeField] GameObject HowToPlayButtonRef;
    [SerializeField] GameObject GameWonScreen;

    [SerializeField] private GameState currentGameState = GameState.MainMenu;

    public enum GameState
    {
        MainMenu,
        Paused,
        Playing,
        HowToPlay,
        GameWon
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        currentGameState = GameState.MainMenu;
        UpdateUIState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.Playing)
                currentGameState = GameState.Paused;
            else if (currentGameState == GameState.Paused)
                currentGameState = GameState.Playing;
            UpdateUIState();
        }
    }

    public void StartGame()
    {
        Debug.Log("Start Game");

        currentGameState = GameState.Playing;
        UpdateUIState();

        SceneManager.LoadScene(UEBER_LEVEL);
        SceneManager.LoadSceneAsync(LEVEL1, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(LEVEL2, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(LEVEL3, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(LEVEL4, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(LEVEL5, LoadSceneMode.Additive);
    }

    public void SetPause(bool pauseState)
    {
        if (currentGameState == GameState.Playing)
            currentGameState = GameState.Paused;
        else if (currentGameState == GameState.Paused)
            currentGameState = GameState.Playing;        

        UpdateUIState();
    }

    public void BackToMainMenu()
    {
        currentGameState = GameState.MainMenu;
        UpdateUIState();
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void HowToPlay()
    {
        if (currentGameState == GameState.MainMenu)
            currentGameState = GameState.HowToPlay;
        else if (currentGameState == GameState.HowToPlay)
            currentGameState = GameState.MainMenu;

        UpdateUIState();
    }

    public void GameWon()
    {
        currentGameState = GameState.GameWon;
        UpdateUIState();
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void ResetGame()
    {
        currentGameState = GameState.MainMenu;
        SeelenManager.instance.ResetSouls();
        UpdateUIState();
    }

    private void UpdateUIState()
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
                GameWonScreen.SetActive(false);

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
                GameWonScreen.SetActive(false);

                ButtonsRef.SetActive(true);
                break;
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                HowToPlayButtonRef.SetActive(false);
                HowToPlayInfoRef.SetActive(false);
                SoulTextsRef.SetActive(true);
                ButtonsRef.SetActive(false);
                GameWonScreen.SetActive(false);
                break;
            case GameState.HowToPlay:
                Cursor.lockState = CursorLockMode.None;
                HowToPlayButtonRef.SetActive(true);
                HowToPlayInfoRef.SetActive(true);
                ButtonsRef.SetActive(false);
                GameWonScreen.SetActive(false);
                SoulTextsRef.SetActive(false);
                break;
            case GameState.GameWon:
                Cursor.lockState = CursorLockMode.None;
                HowToPlayButtonRef.SetActive(false);
                HowToPlayInfoRef.SetActive(false);
                ButtonsRef.SetActive(false); 
                GameWonScreen.SetActive(true);
                SoulTextsRef.SetActive(true);
                break;
        }
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
