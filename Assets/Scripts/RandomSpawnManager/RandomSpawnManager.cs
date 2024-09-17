using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnManager : MonoBehaviour
{

    [Header("Random Spawn Properties")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject cannonballPrefab;
    [SerializeField] private List<GameObject> activeCannons;
    [SerializeField] private float destroyTime;
    [SerializeField] private float cannonRate;
    [SerializeField] private float cannonBallSpeed;
    [SerializeField] private float listLenght;

    [Space]

    [SerializeField] private PlayerStats playerStats;

    private void OnEnable()
    {
        PlayerStats.OnPlayerDeathAdded += OnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDeathAdded -= OnPlayerDeath;
    }

    void Start()
    {
        playerStats.SetTimerStatus(true);

        InvokeRepeating(nameof(SpawnCannonball), 0.5f, cannonRate);
    }

    private void Update()
    {
        activeCannons.RemoveAll(GameObject => GameObject == null);
    }

    private void SpawnCannonball()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        GameObject go = Instantiate(cannonballPrefab, spawnPoints[randomIndex].position, Quaternion.identity);

        activeCannons.Add(go);

        go.GetComponent<CannonballManager>().SetSpeed(cannonBallSpeed);


        cannonBallSpeed += 0.5f;

        Destroy(go, destroyTime);
    }

    private void OnPlayerDeath()
    {
        foreach (var go in activeCannons) { Destroy(go); }
        playerStats.SetTimeCounter(0);
        cannonBallSpeed = 5;
    }
}
