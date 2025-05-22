using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private HashSet<string> defeatedEnemies = new HashSet<string>();

    public string lastEnemy;
    public int unlockedPhase = 1;
    public int currentPhase = 1;

    public EnemyData currentEnemyData;
    public List<EnemyData> enemiesByPhase = new List<EnemyData>();

    public Image fadeImage; // referencie o Image do painel preto no Inspector

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

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Primeiro ativa ou encontra o painel de fade da cena atual
        Image currentFadeImage = GetFadeImageInScene();

        if (currentFadeImage != null)
        {
            currentFadeImage.gameObject.SetActive(true);

            // Fade in (alpha de 0 -> 1)
            float duration = 1f;
            float t = 0f;
            Color color = currentFadeImage.color;

            while (t < duration)
            {
                t += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, t / duration);
                currentFadeImage.color = color;
                yield return null;
            }
        }

        yield return new WaitForSeconds(2f); // tempo antes da troca de cena

        // Carrega a nova cena
        SceneManager.LoadScene(sceneName);

        // Aguarda 1 frame para garantir que a nova cena carregou
        yield return null;

        // Agora procura o novo painel de fade na nova cena e faz fade out (opcional)
        Image newFadeImage = GetFadeImageInScene();
        if (newFadeImage != null)
        {
            newFadeImage.gameObject.SetActive(true);
            Color color = newFadeImage.color;
            color.a = 1f;
            newFadeImage.color = color;

            // Fade out (alpha de 1 -> 0)
            float duration = 1f;
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, t / duration);
                newFadeImage.color = color;
                yield return null;
            }

            newFadeImage.gameObject.SetActive(false);
        }
    }

    private Image GetFadeImageInScene()
    {
        GameObject fadeObj = GameObject.FindGameObjectWithTag("Fade");
        if (fadeObj != null)
        {
            return fadeObj.GetComponent<Image>();
        }

        Debug.LogWarning("FadePanel com tag 'Fade' não encontrado na cena.");
        return null;
    }

    private void InitializeEnemies()
    {
        enemiesByPhase.Clear();

        enemiesByPhase.Add(new EnemyData("Fogo", 100, 10, Element.Fire, Color.red, 0));
        enemiesByPhase.Add(new EnemyData("Água", 120, 10, Element.Water, Color.blue, 1));
        //enemiesByPhase.Add(new EnemyData("Terra", 150, 6, Element.Earth, new Color(0.4f, 0.25f, 0.1f), 2));
        enemiesByPhase.Add(new EnemyData("Ar", 90, 10, Element.Air, Color.white, 3));
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
        LoadSceneWithFade("GameOver");
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
        LoadSceneWithFade("Fases");
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
