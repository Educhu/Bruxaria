using TMPro;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public Vector3 enemyPosition;

    public ActionType PlayerAction { get; private set; }
    public ActionType EnemyAction { get; private set; }



    public void PlayerCastsSpell(ActionType action)
    {
        BattleLog.Instance.AddLogEntry("Jogador lançou feitiço: " + action);

        if (action == ActionType.Attack)
        {
            Debug.Log("Elemento de ataque atualizado para: " + player.selectedElement);
            //BattleLog.Instance.AddLog("Elemento de ataque atualizado para: " + player.selectedElement);
        }

        PlayerAction = action;
        GetEnemyAction();
        ExecuteTurn();
    }

    private void GetEnemyAction()
    {
        EnemyAction = enemy.ChooseAction(player); // Passa o Player para a decisão do inimigo
        BattleLog.Instance.AddLogEntry("Inimigo escolheu: " + EnemyAction);
    }

    private void ExecuteTurn()
    {
        BattleLog.Instance.AddLogEntry("Executando turno...");

        bool playerFirst = player.speed > enemy.speed;
        bool enemyFirst = !playerFirst && Random.Range(0, 2) == 0;

        if (playerFirst)
        {
            BattleLog.Instance.AddLogEntry("Jogador age primeiro");
            ResolveAction(PlayerAction, true);

            if (enemy.health <= 0) // Inimigo derrotado
            {
                GameManager.Instance.BattleWon();
                return;
            }
        }

        BattleLog.Instance.AddLogEntry("Inimigo executa ação");
        ResolveAction(EnemyAction, false);

        if (player.health <= 0) // Player morreu
        {
            GameManager.Instance.GameOver();
            return;
        }

        if (!playerFirst)
        {
            BattleLog.Instance.AddLogEntry("Inimigo age primeiro");
            ResolveAction(PlayerAction, true);

            if (enemy.health <= 0) // Inimigo derrotado
            {
                GameManager.Instance.BattleWon();
                return;
            }
        }

        BattleLog.Instance.AddLogEntry("Turno finalizado.");
    }

    private void ResolveAction(ActionType action, bool isPlayer)
    {
        if (isPlayer)
        {
            //Debug.Log("Elemento do jogador no momento do ataque: " + player.selectedElement);
            BattleLog.Instance.AddLogEntry("Elemento do jogador no momento do ataque: " + player.selectedElement);
            SpellEffectManager.Instance.SpawnEffect(player.selectedElement, enemyPosition);
            switch (action)
            {
                case ActionType.Attack:
                    int damage = CalculateDamage(player.selectedElement, (Element)enemy.element);
                    enemy.TakeDamage(damage);
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
                    BattleLog.Instance.AddLogEntry("Ataque do inimigo com elemento: " + enemy.element);
                    int damage = CalculateDamage(enemy.element, player.selectedElement); // aqui o erro
                    player.TakeDamage(damage);
                    break;
                case ActionType.Heal:
                    enemy.Heal(20);
                    break;
                case ActionType.SpeedBoost:
                    enemy.speed += 5;
                    break;
            }
        }
    }

    private int CalculateDamage(Element attackerElement, Element defender)
    {
        int baseDamage = 30;
        if ((attackerElement == Element.Fire && defender == Element.Earth) ||
            (attackerElement == Element.Earth && defender == Element.Water) ||
            (attackerElement == Element.Water && defender == Element.Fire) ||
            (attackerElement == Element.Air && defender == Element.Poison) ||
            (attackerElement == Element.Poison && defender == Element.Metal) ||
            (attackerElement == Element.Eletric && defender == Element.Air) ||
            (attackerElement == Element.Metal && defender == Element.Eletric))


        {
            return baseDamage + 20; // Dano aumentado
        }
        if ((attackerElement == Element.Fire && defender == Element.Water) ||
            (attackerElement == Element.Earth && defender == Element.Fire) ||
            (attackerElement == Element.Water && defender == Element.Earth) ||
            (attackerElement == Element.Air && defender == Element.Eletric) ||
            (attackerElement == Element.Poison && defender == Element.Air) ||
            (attackerElement == Element.Eletric && defender == Element.Metal) ||
            (attackerElement == Element.Metal && defender == Element.Poison))

        {
            return baseDamage - 20; // Dano reduzido
        }
        if ((attackerElement == Element.Amongus))
            return baseDamage + 1000;

        return baseDamage; // Dano neutro

    }
}

public enum ActionType { Attack, Heal, SpeedBoost }
public enum Element { Fire, Water, Earth, Air, Poison, Metal, Eletric, Amongus}