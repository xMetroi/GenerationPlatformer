using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [Header("Scene Properties")]
    [SerializeField] string sceneName;

    [Header("Level Properties")]
    [SerializeField] string LevelName;
    [SerializeField] int initialCountDown;
    [SerializeField] float maxTimeToPerfect;

    private LobbyUIManager lobbyUIManager;
    private TMP_Text levelText;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        lobbyUIManager = GameObject.FindGameObjectWithTag("LobbyUIManager").GetComponent<LobbyUIManager>();
        levelText = GetComponentInChildren<TMP_Text>();

        levelText.text = LevelName;
    }

    public void OnLevelButtonClicked()
    {
        Level level = new Level(LevelName, maxTimeToPerfect, initialCountDown);
        lobbyUIManager.LoadLevel(sceneName, level);
    }
}
