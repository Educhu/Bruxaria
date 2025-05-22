using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private HashSet<string> defeatedEnemies = new HashSet<string>();

    public string lastEnemy;
    public int unlockedPhase = 1;
    public int currentPhase = 1;

    public EnemyData currentEnemyData;
    public List<EnemyData> enemiesByPhase = new List<EnemyData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEnemies(); // Preenche a lista
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEnemies()
    {
        enemiesByPhase.Clear();

        enemiesByPhase.Add(new EnemyData("Fogo", 100, 10, Element.Fire, Color.red, 0));
        enemiesByPhase.Add(new EnemyData("Água", 120, 8, Element.Water, Color.blue, 1));
        enemiesByPhase.Add(new EnemyData("Terra", 150, 6, Element.Earth, new Color(0.4f, 0.25f, 0.1f), 2));
        enemiesByPhase.Add(new EnemyData("Ar", 90, 12, Element.Air, Color.white, 3));
        enemiesByPhase.Add(new EnemyData("Metal", 100, 10, Element.Metal, Color.red, 4));
        enemiesByPhase.Add(new EnemyData("Veneno", 100, 10, Element.Poison, Color.red, 5));
        enemiesByPhase.Add(new EnemyData("Raio", 100, 10, Element.Eletric, Color.red, 6));
    }

    public void StartBattle(int phase)
    {
        if (phase <= unlockedPhase)
        {
            currentPhase = phase;
            SetEnemyAttributes(phase);
            StartCoroutine(LoadBattleAndApply());
        }
    }

    private IEnumerator LoadBattleAndApply()
    {
        LoadScene("Combate");
        yield return null;
        yield return new WaitForSeconds(0.1f);
        ApplyEnemyAttributes();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Carregando cena de morte...");
        LoadScene("GameOver");
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BattleWon()
    {
        if (currentPhase > 0 && currentPhase < enemiesByPhase.Count)
        {
            unlockedPhase = currentPhase + 1;
        }

        Debug.Log("Batalha vencida! Retornando ao mapa...");
        LoadScene("Fases");
    }

    private void SetEnemyAttributes(int phase)
    {
        int index = phase - 1;
        if (index >= 0 && index < enemiesByPhase.Count)
        {
            currentEnemyData = enemiesByPhase[index];
        }
        else
        {
            Debug.LogWarning("Fase inválida ou não configurada.");
        }
    }

    private void ApplyEnemyAttributes()
    {
        Enemy enemy = FindObjectOfType<Enemy>();
        if (enemy != null && currentEnemyData != null)
        {
            enemy.SetAttributes(currentEnemyData);
        }
        else
        {
            Debug.LogWarning("Enemy ou EnemyData não encontrado para aplicar atributos.");
        }
    }

    public bool IsEnemyDefeated(string enemyName)
    {
        return defeatedEnemies.Contains(enemyName);
    }
}

[System.Serializable]
public class EnemyData
{
    public string name;
    public int health;
    public int speed;
    public Element element;
    public Color color;
    public int elementIndex; // <- AQUI está o index que o Animator vai usar

    public EnemyData(string name, int health, int speed, Element element, Color color, int elementIndex)
    {
        this.name = name;
        this.health = health;
        this.speed = speed;
        this.element = element;
        this.color = color;
        this.elementIndex = elementIndex;
    }
}
