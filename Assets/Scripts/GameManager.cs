using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private HashSet<string> defeatedEnemies = new HashSet<string>(); // Armazena inimigos derrotados
    public string lastEnemy; // Guarda o inimigo atual para controle da batalha
    public int unlockedPhase = 1; // Começa apenas com a primeira fase liberada
    public int currentPhase = 1; // Fase ativa
    public EnemyData currentEnemyData; // Guarda os atributos do inimigo carregado
    public Sprite fireEnemySprite;
    public Sprite waterEnemySprite;
    public Sprite earthEnemySprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o GameManager ao trocar de cena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartBattle(int phase)
    {
        if (phase <= unlockedPhase) // Apenas fases desbloqueadas podem ser acessadas
        {
            currentPhase = phase;
            SetEnemyAttributes(phase);
            LoadScene("Combate"); // Nome da cena de batalha
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Carregando cena de morte...");
        LoadScene("GameOver"); // Nome da cena de Game Over
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Menu"); // Substitua "Menu" pelo nome exato da sua cena do menu
    }

    //public void BattleWon()
    //{
    //    if (!string.IsNullOrEmpty(lastEnemy))
    //    {
    //        defeatedEnemies.Add(lastEnemy); // Marca o inimigo como derrotado
    //    }

    //    Debug.Log("Batalha vencida! Retornando ao mapa...");
    //    LoadScene("Mapa"); // Nome da cena do mapa
    //}
    public void BattleWon()
    {
        if (currentPhase > 0 && currentPhase < 3)
        {
            unlockedPhase = currentPhase + 1; // Libera a próxima fase
        }

        Debug.Log("Batalha vencida! Retornando ao mapa...");
        LoadScene("Fases");
    }
    private void SetEnemyAttributes(int phase)
    {
        switch (phase)
        {
            case 1:
                currentEnemyData = new EnemyData("Fogo", 100, 10, Element.Fire, Color.red);
                break;
            case 2:
                currentEnemyData = new EnemyData("Água", 100, 15, Element.Water, Color.blue);
                break;
            case 3:
                currentEnemyData = new EnemyData("Terra", 100, 12, Element.Earth, Color.green);
                break;
        }

        ApplyEnemyAttributes();
    }

    private void ApplyEnemyAttributes()
    {
        Enemy enemy = FindObjectOfType<Enemy>();
        if (enemy != null)
        {
            enemy.SetAttributes(currentEnemyData);
        }
    }

    public bool IsEnemyDefeated(string enemyName)
    {
        return defeatedEnemies.Contains(enemyName);
    }
}

public class EnemyData
{
    public string name;
    public int health;
    public int speed;
    public Element element;
    public Color color; // Nova propriedade para armazenar a cor

    public EnemyData(string name, int health, int speed, Element element, Color color)
    {
        this.name = name;
        this.health = health;
        this.speed = speed;
        this.element = element;
        this.color = color;
    }
}
