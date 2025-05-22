using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;

    public Player player;
    public Enemy[] enemies; // Esses são os inimigos já posicionados manualmente na cena (arrastados no Inspector)
    private int enemyIndex = 0;
    private Enemy currentEnemy;

    public Vector3 enemyPosition;
    public Transform spellSpawnPoint;

    public ActionType PlayerAction { get; private set; }
    public ActionType EnemyAction { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Pega a lista de EnemyData que o GameManager preparou com base na fase
        var dataList = GameManager.Instance.enemiesByPhase;

        if (dataList.Count != enemies.Length)
        {
            Debug.LogError("Quantidade de EnemyData e Enemy na cena não coincidem!");
            return;
        }

        // Atribui os dados para cada inimigo manual da cena
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetAttributes(dataList[i]);
            enemies[i].gameObject.SetActive(i == 0); // Só ativa o primeiro inimigo no início
        }

        currentEnemy = enemies[enemyIndex];
    }

    public void OnSpellButtonPressed(int elementIndex)
    {
        Element chosenElement = (Element)elementIndex;
        player.selectedElement = chosenElement;
        PlayerCastsSpell(ActionType.Attack);
    }

    public void PlayerCastsSpell(ActionType action)
    {
        if (currentEnemy == null)
        {
            Debug.LogError("currentEnemy está null! Verifique se o inimigo foi inicializado corretamente.");
            return;
        }

        BattleLog.Instance.AddLogEntry("Jogador lançou feitiço: " + action);

        PlayerAction = action;
        EnemyAction = currentEnemy.ChooseAction(player);
        BattleLog.Instance.AddLogEntry("Inimigo escolheu: " + EnemyAction);

        ExecuteTurn();
    }

    private void ExecuteTurn()
    {
        BattleLog.Instance.AddLogEntry("Executando turno...");

        bool playerFirst = player.speed > currentEnemy.speed;
        bool enemyFirst = !playerFirst && Random.Range(0, 2) == 0;

        if (playerFirst)
        {
            BattleLog.Instance.AddLogEntry("Jogador age primeiro");
            ResolveAction(PlayerAction, true);

            if (currentEnemy.health <= 0) return;
        }

        BattleLog.Instance.AddLogEntry("Inimigo executa ação");
        ResolveAction(EnemyAction, false);

        if (player.health <= 0)
        {
            GameManager.Instance.GameOver();
            return;
        }

        if (!playerFirst)
        {
            BattleLog.Instance.AddLogEntry("Jogador age agora");
            ResolveAction(PlayerAction, true);
        }

        BattleLog.Instance.AddLogEntry("Turno finalizado.");
    }

    private void ResolveAction(ActionType action, bool isPlayer)
    {
        if (isPlayer)
        {
            BattleLog.Instance.AddLogEntry("Elemento do jogador: " + player.selectedElement);
            SpellEffectManager.Instance.SpawnEffect(player.selectedElement, enemyPosition);

            switch (action)
            {
                case ActionType.Attack:
                    int damage = CalculateDamage(player.selectedElement, currentEnemy.element);
                    currentEnemy.TakeDamage(damage);
                    break;
                case ActionType.Heal:
                    player.IncreaseHealth(20);
                    break;
                case ActionType.SpeedBoost:
                    player.IncreaseSpeed(5);
                    break;
            }
        }
        else
        {
            BattleLog.Instance.AddLogEntry("Ação do inimigo: " + action);
            switch (action)
            {
                case ActionType.Attack:
                    int damage = CalculateDamage(currentEnemy.element, player.selectedElement);
                    player.TakeDamage(damage);
                    break;
                case ActionType.Heal:
                    currentEnemy.Heal(20);
                    break;
                case ActionType.SpeedBoost:
                    currentEnemy.speed += 5;
                    break;
            }
        }
    }

    private int CalculateDamage(Element attacker, Element defender)
    {
        int baseDamage = 30;

        if ((attacker == Element.Fire && defender == Element.Earth) ||
            (attacker == Element.Earth && defender == Element.Water) ||
            (attacker == Element.Water && defender == Element.Fire) ||
            (attacker == Element.Air && defender == Element.Poison) ||
            (attacker == Element.Poison && defender == Element.Metal) ||
            (attacker == Element.Eletric && defender == Element.Air) ||
            (attacker == Element.Metal && defender == Element.Eletric))
        {
            return baseDamage + 20;
        }

        if ((attacker == Element.Fire && defender == Element.Water) ||
            (attacker == Element.Earth && defender == Element.Fire) ||
            (attacker == Element.Water && defender == Element.Earth) ||
            (attacker == Element.Air && defender == Element.Eletric) ||
            (attacker == Element.Poison && defender == Element.Air) ||
            (attacker == Element.Eletric && defender == Element.Metal) ||
            (attacker == Element.Metal && defender == Element.Poison))
        {
            return baseDamage - 20;
        }

        if (attacker == Element.Amongus)
            return baseDamage + 1000;

        return baseDamage;
    }

    public void OnEnemyDefeated()
    {
        enemyIndex++;
        if (enemyIndex >= enemies.Length)
        {
            Debug.Log("Vitória! Todos os inimigos derrotados.");
            GameManager.Instance.BattleWon();
        }
        else
        {
            Debug.Log("Próximo inimigo!");
            currentEnemy = enemies[enemyIndex];
            currentEnemy.gameObject.SetActive(true);
        }
    }
}
public enum ActionType { Attack, Heal, SpeedBoost }
public enum Element { Fire, Water, Earth, Air, Poison, Metal, Eletric, Amongus }
