﻿using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber = 0;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    void Start()
    {
        NextWave ();
    }

    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;

            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate (enemy, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }    
    }

    void NextWave()
    {
        currentWaveNumber++;
        print (" Wave current number " + currentWaveNumber);

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves [currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    void OnEnemyDeath()
    {
        print ("enemy die");
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0) 
        {
            NextWave ();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
