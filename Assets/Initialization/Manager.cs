using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set;}
    const string _mainMenuSceneName = "MainMenu";
    const string _gameSceneName = "Game";

    [SerializeField] GameOptions _gameOptions;
    public GameOptions GameOptions => _gameOptions;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        GoToMainMenu();
    }

    void UnloadGameScene()
    {
        Scene gameScene = SceneManager.GetSceneByName(_gameSceneName);
        if(gameScene.isLoaded)
            SceneManager.UnloadSceneAsync(gameScene);
    }
    void UnloadMenuScene()
    {
        Scene menuScene = SceneManager.GetSceneByName(_mainMenuSceneName);
        if(menuScene.isLoaded)
            SceneManager.UnloadSceneAsync(menuScene);
    }

    public void GoToMainMenu()
    {
        UnloadGameScene();
        SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Additive);
    }

    public void GoToGame()
    {
        UnloadGameScene();
        UnloadMenuScene();
        SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Additive);
        StartCoroutine(SetGameSceneActive());
    }

    IEnumerator SetGameSceneActive()
    {
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_gameSceneName));
    }
}
