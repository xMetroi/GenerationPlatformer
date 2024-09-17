using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Level Properties")]
    [SerializeField] private string levelName;
    [SerializeField] private float maxTimeToPerfect;
    [SerializeField] private int initialCountDown;
    bool isLevelLoaded;

    [Header("Player Properties")]
    [SerializeField] GameObject playerReference;
    [SerializeField] Transform initialSpawnPoint;
    [SerializeField] GameObject playerPrefab;

    [Header("Countdown Properties")]
    [SerializeField] TMP_Text countdownText;
    float timer;
    bool startCountdown;

    [Header("Finished Level Properties")]
    [SerializeField] private TMP_Text completedLevelText;
    [SerializeField] private TMP_Text timeElapsedText;
    [SerializeField] private TMP_Text deathsCountText;
    [SerializeField] private List<GameObject> beansList;


    [Header("UI References")]
    [SerializeField] private GameObject levelInfoCanvas;
    [SerializeField] private GameObject initialCountdownCanvas;
    [SerializeField] private GameObject LevelFinishedCanvas;
    [SerializeField] private TMP_Text levelText;

    #region Events

    public static event Action OnLevelInitialized;

    public static event Action OnEndedCountdown;

    public static event Action OnPlayerSpawned;

    #endregion

    private static GameManager instance;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoad;
        LobbyUIManager.OnLevelLoaded += OnLevelInfoLoaded;
        PlayerStats.OnPlayerDeathAdded += OnPlayerDeath;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoad;
        LobbyUIManager.OnLevelLoaded -= OnLevelInfoLoaded;
        PlayerStats.OnPlayerDeathAdded -= OnPlayerDeath;
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameLobby") return;

        isLevelLoaded = true;

        InitializeLevel();
    }

    private void OnLevelInfoLoaded(Level level)
    {
        levelName = level.levelName;
        maxTimeToPerfect = level.maxTimeToPerfect;
        initialCountDown = level.initialCountdown;

        levelText.text = levelName;
    }

    #region Initialize

    private void InitializeLevel()
    {
        OnLevelInitialized?.Invoke();

        StartCountdown(initialCountDown);

        levelInfoCanvas.SetActive(true);
        initialSpawnPoint = GameObject.FindGameObjectWithTag("InitialSpawnPoint").transform;
        GameObject go = Instantiate(playerPrefab, initialSpawnPoint.position, Quaternion.identity);
        InitializePlayer(go);

        OnPlayerSpawned?.Invoke();
    }

    private void InitializePlayer(GameObject player)
    {
        playerReference = player;

        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        
    }

    #endregion

    private void Update()
    {
        if (startCountdown)
            Countdown();
    }

    #region Countdown

    private void Countdown()
    {
        initialCountdownCanvas.SetActive(true);

        timer -= Time.deltaTime;

        if (timer >= 1)
        {
            countdownText.text = $"<color=#FFFF00>{(int) timer}</color>";
        }
        else
        {
            countdownText.text = $"<color=#6AA84F>GOO!!</color>";
            Invoke(nameof(StopCountdown), 0.5f);
        }
    }

    private void StartCountdown(float seconds)
    {
        startCountdown = true;
        timer = seconds;
    }

    private void StopCountdown()
    {        
        initialCountdownCanvas.SetActive(false);
        startCountdown = false;

        OnEndedCountdown?.Invoke();
    }

    #endregion

    private void OnPlayerDeath()
    {

    }

    public void FinishLevel()
    {
        LevelFinishedCanvas.SetActive(true);

        PlayerStats playerStats = playerReference.GetComponent<PlayerStats>();

        completedLevelText.text = $"Level <color=#fff900>{levelName}</color> completed";
        timeElapsedText.text = $"Time elapsed: <color=#fff900>{(int) playerStats.GetTimeCounter()}</color>";
        deathsCountText.text = $"Death Count: <color=#f44336>{playerStats.GetDeathCounter()}</color>";

        for (int i = 0; i < playerStats.GetBeanCount(); i++)
        {            
            beansList[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        
        playerReference.SetActive(false);
    }

    private void ResetFinishedLevelCanvas()
    {
        completedLevelText.text = $"Level <color=#fff900>{0}</color> completed";
        timeElapsedText.text = $"Time elapsed: <color=#fff900>{0}</color>";
        deathsCountText.text = $"Death Count: <color=#f44336>{0}</color>";

        foreach (var bean in beansList)
        {
            bean.GetComponent<Image>().color = new Color32(108, 108, 108, 255);
        }
    }

    public void OnGameFinishedBackToLobbyButton()
    {
        levelInfoCanvas.SetActive(false);
        LevelFinishedCanvas.SetActive(false);

        ResetFinishedLevelCanvas();

        Destroy(playerReference);

        SceneManager.LoadScene("GameLobby");
    }
}
