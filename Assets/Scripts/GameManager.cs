using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

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
        yield return null; // Espera 1 frame
        yield return new WaitForSeconds(0.1f); // Segurança extra
        ApplyEnemyAttributes();
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
                currentEnemyData = new EnemyData(
                    "Fogo", 100, 10, Element.Fire, Color.red,
                    Resources.Load<AnimationClip>("Anims/Fire/Idle"),
                    Resources.Load<AnimationClip>("Anims/Fire/Attack"),
                    Resources.Load<AnimationClip>("Anims/Fire/Hit"),
                    Resources.Load<AnimationClip>("Anims/Fire/Die")
                );
                break;

                // Repete para os demais elementos
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
    public Color color;

    public AnimationClip idleClip;
    public AnimationClip attackClip;
    public AnimationClip hitClip;
    public AnimationClip dieClip;

    public EnemyData(string name, int health, int speed, Element element, Color color,
                     AnimationClip idle, AnimationClip attack, AnimationClip hit, AnimationClip die)
    {
        this.name = name;
        this.health = health;
        this.speed = speed;
        this.element = element;
        this.color = color;

        this.idleClip = idle;
        this.attackClip = attack;
        this.hitClip = hit;
        this.dieClip = die;
    }
}
