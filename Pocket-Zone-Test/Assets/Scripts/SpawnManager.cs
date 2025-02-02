using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float delay = 1;
    [SerializeField] int repeatRate = 3;
    #region Границы спавна
    float lowX = 0f;    
    float highX = 11f;
    float lowY = 6f;
    float highY = -6f;
    #endregion
    void Start()
    {

        StartCoroutine(SpawnEnemy(repeatRate, delay));
    }

    private IEnumerator SpawnEnemy(int spawnCount, float secondsToSpawn)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(secondsToSpawn);
            Vector2 randomPos = new Vector2(Random.Range(lowX, highX), Random.Range(lowY, highY));
            Instantiate(enemyPrefab, randomPos, enemyPrefab.transform.rotation);

            
        }
    }
}
