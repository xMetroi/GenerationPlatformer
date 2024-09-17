using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Title / Level Properties")]
    [SerializeField] private TMP_Text LevelTitle;
    [SerializeField] private TMP_Text actualLevelText;

    [Header("Time Properties")]
    [SerializeField] private TMP_Text timeAmountText;

    [Header("Death Properties")]
    [SerializeField] private TMP_Text deathAmountText;

    [Header("Dash Properties")]
    [SerializeField] private TMP_Text dashAmountText;

    [Header("Bean Properties")]
    [SerializeField] private List<GameObject> beansList;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        PlayerStats.OnPlayerDeathAdded += OnDeathAdded;
        PlayerStats.OnPlayerDeathRemoved += OnDeathRemoved;

        PlayerStats.OnPlayerDashAdded += OnDashAdded;
        PlayerStats.OnPlayerDashRemoved += OnDashRemoved;

        PlayerStats.OnPlayerBeanAdded += OnBeanAdded;
        PlayerStats.OnPlayerBeanRemoved += OnBeanRemoved;

        GameManager.OnEndedCountdown += OnEndCountdown;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDeathAdded -= OnDeathAdded;
        PlayerStats.OnPlayerDeathRemoved -= OnDeathRemoved;

        PlayerStats.OnPlayerDashAdded -= OnDashAdded;
        PlayerStats.OnPlayerDashRemoved -= OnDashRemoved;

        PlayerStats.OnPlayerBeanAdded -= OnBeanAdded;
        PlayerStats.OnPlayerBeanRemoved -= OnBeanRemoved;

        GameManager.OnEndedCountdown -= OnEndCountdown;
    }

    private void Update()
    {
        UpdateTimerInfo(playerStats);
    }

    private void UpdateTimerInfo(PlayerStats playerStats)
    {
        if (playerStats.GetTimerStatus())
        {
            timeAmountText.text = $"{(int) playerStats.GetTimeCounter()}";
        }
    }

    private void OnDeathAdded()
    {
        deathAmountText.text = $"{playerStats.GetDeathCounter()}";
    }

    private void OnDeathRemoved()
    {
        deathAmountText.text = $"{playerStats.GetDeathCounter()}";
    }

    private void OnDashAdded()
    {
        dashAmountText.text = $"{playerStats.GetDashCount()}";
    }

    private void OnDashRemoved()
    {
        dashAmountText.text = $"{playerStats.GetDashCount()}";
    }

    private void OnBeanAdded()
    {
        if (playerStats.GetBeanCount() > 0 && playerStats.GetBeanCount() <= 3) 
        {
            for (int i = 0; i < playerStats.GetBeanCount(); i++)
            {
                beansList[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }

    private void OnBeanRemoved()
    {
        if (playerStats.GetBeanCount() > 0 && playerStats.GetBeanCount() <= 3)
        {
            for (int i = 0; i < playerStats.GetBeanCount(); i++)
            {
                beansList[i].GetComponent<Image>().color = new Color32(108, 108, 108, 255);
            }
        }
    }

    private void OnEndCountdown()
    {
        playerStats.SetTimerStatus(true);
    }
}
