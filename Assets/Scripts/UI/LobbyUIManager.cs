using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject PersistentCanvas;
    [SerializeField] private GameObject GameLobbyCanvas;
    [SerializeField] private GameObject StartGameCanvas;

    #region Events

    public static event Action<Level> OnLevelLoaded;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region GameLobby

    public void OnStartGameButton()
    {
        GameLobbyCanvas.SetActive(false);
        StartGameCanvas.SetActive(true);
    }

    public void OnOptionsButton()
    {

    }

    public void OnExitButton()
    {
        
    }

    #endregion

    #region StartGame

    public void LoadLevel(string sceneName, Level level)
    {
        SceneManager.LoadScene(sceneName);
        OnLevelLoaded?.Invoke(level);
    }

    public void OnBackToLobbyButton()
    {
        StartGameCanvas.SetActive(false);
        GameLobbyCanvas.SetActive(true);
    }

    #endregion
}
