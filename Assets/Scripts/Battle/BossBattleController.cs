using UnityEngine;

public class BossBattleController : MonoBehaviour
{
    public Transform[] enemySpawnPoints;
    public GameObject[] enemyPrefabs; // 0 e 1 = inimigos comuns, 2 = boss

    private int currentEnemyIndex = 0;
    private GameObject currentEnemy;

    void Start()
    {
        SpawnNextEnemy();
    }

    void SpawnNextEnemy()
    {
        if (currentEnemy != null)
            Destroy(currentEnemy);

        if (currentEnemyIndex < enemyPrefabs.Length)
        {
            currentEnemy = Instantiate(enemyPrefabs[currentEnemyIndex], enemySpawnPoints[currentEnemyIndex].position, Quaternion.identity);
        }
        else
        {
            Victory();
        }
    }

    public void OnEnemyDefeated()
    {
        currentEnemyIndex++;
        SpawnNextEnemy();
    }

    void Victory()
    {
        Debug.Log("Vitória! Todos os inimigos foram derrotados.");
        // Pode carregar uma cena de vitória ou mostrar UI
    }
}

