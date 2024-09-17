using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Level Stats")]
    [SerializeField] private float timeCounter;
    [SerializeField] private int deathCounter;
    [SerializeField] private Vector2 actualCheckpoint;
    [SerializeField] private Transform deathHeight;

    [Header("Dash Properties")]
    [SerializeField] private int dashCount;
    [SerializeField] private int maxDashCount;

    [Header("Bean Properties")]
    [SerializeField] private int beanCount;
    [SerializeField] private int maxBeanCount;

    bool startTimer;

    #region events

    public static event Action OnPlayerTimeAdded;
    public static event Action OnPlayerTimeRemoved;

    public static event Action OnPlayerDeathAdded;
    public static event Action OnPlayerDeathRemoved;

    public static event Action OnPlayerDashAdded;
    public static event Action OnPlayerDashRemoved;

    public static event Action OnPlayerBeanAdded;
    public static event Action OnPlayerBeanRemoved;

    #endregion

    private void Start()
    {
        deathHeight = GameObject.FindGameObjectWithTag("DeathHeight").transform;
        actualCheckpoint = GameObject.FindGameObjectWithTag("InitialSpawnPoint").transform.position;
    }

    #region Getter / Setter

    public void SetTimeCounter(int time)
    {
        this.timeCounter = time;
    }

    public float GetTimeCounter()
    {
        return timeCounter;
    }

    public void SetActualCheckpoint(Vector2 pos)
    {
        this.actualCheckpoint = pos;
    }

    public Vector2 GetActualCheckpoint()
    {
        return actualCheckpoint;
    }

    public void SetDeathCounter(int deathCounter)
    {
        this.deathCounter = deathCounter;
    }

    public int GetDeathCounter()
    {
        return deathCounter;
    }

    public void SetDashCount(int dashCount)
    {
        this.dashCount = dashCount;
    }

    public void SetDashMaxCount(int maxDashCount)
    {
        this.maxDashCount = maxDashCount;
    }

    public int GetDashCount()
    {
        return this.dashCount;
    }

    public int GetMaxDashCount()
    {
        return this.maxDashCount;
    }

    public void SetBeanCount(int beanCount)
    {
        this.beanCount = beanCount;
    }

    public void SetBeanMaxCount(int maxbeanCount)
    {
        this.maxBeanCount = maxbeanCount;
    }

    public int GetBeanCount()
    {
        return this.beanCount;
    }

    public int GetMaxBeanCount()
    {
        return this.maxBeanCount;
    }

    #endregion

    private void Update()
    {
        if (startTimer)
        {
            timeCounter += Time.deltaTime;
        }

        if (transform.position.y <= deathHeight.position.y)
        {
            KillPlayer(1);
        }
    }

    #region Methods

    public void SetTimerStatus(bool timerStatus)
    {
        this.startTimer = timerStatus;
    }

    public bool GetTimerStatus()
    {
        return startTimer;
    }

    public void AddTime(int amount)
    {
        this.timeCounter += amount;

        OnPlayerTimeAdded?.Invoke();
    }

    public void RemoveTime(int amount)
    {
        this.timeCounter -= amount;

        OnPlayerTimeRemoved?.Invoke();
    }

    public void AddDeath(int amount)
    {
        this.deathCounter += amount;

        OnPlayerDeathAdded?.Invoke();
    }

    public void RemoveDeath(int amount)
    {
        this.deathCounter -= amount;

        OnPlayerDeathRemoved?.Invoke();
    }

    public void AddDash(int amount)
    {
        dashCount += amount;

        OnPlayerDashAdded?.Invoke();
    }

    public void RemoveDash(int amount)
    {
        dashCount-= amount;

        OnPlayerDashRemoved?.Invoke();
    }

    public void AddBean(int amount)
    {
        beanCount += amount;

        OnPlayerBeanAdded?.Invoke();
    }

    public void RemoveBean(int amount)
    {
        beanCount-= amount;

        OnPlayerBeanRemoved?.Invoke();
    }

    public void KillPlayer(int deathsToAdd)
    {
        transform.position = actualCheckpoint;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        SetDashCount(0);
        AddDeath(deathsToAdd);
    }

    #endregion
}
