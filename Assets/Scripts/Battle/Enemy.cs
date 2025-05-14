using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int speed = 10;
    public Element element = Element.Fire;
    private SpriteRenderer spriteRenderer;

    public HealthBar healthBar; // Referência para a barra de vida

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Inicializa o SpriteRenderer
        healthBar.SetMaxHealth(maxHealth);

        if (GameManager.Instance != null)
        {
            EnemyData data = GameManager.Instance.currentEnemyData;

            if (data != null)
            {
                gameObject.name = data.name;
                health = data.health;
                maxHealth = data.health;
                speed = data.speed;
                element = data.element;

                // Aplica a cor corretamente
                spriteRenderer.color = data.color;

                healthBar.SetHealth(health);
                Debug.Log("Inimigo carregado: " + gameObject.name + " | Cor: " + data.color);
            }
        }
    }

    public ActionType ChooseAction(Player player)
    {
        int rand = Random.Range(0, 100);

        if (health <= 30 && rand < 50)
        {
            return ActionType.Heal; // Se estiver com pouca vida, pode curar
        }
        else if (speed < player.speed && rand < 70)
        {
            return ActionType.SpeedBoost; // Se estiver mais lento, pode aumentar velocidade
        }

        return ActionType.Attack; // Caso contrário, ataca
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0) health = 0;
        healthBar.SetHealth(health);
        Debug.Log("Inimigo tomou " + amount + " de dano! Vida restante: " + health);
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        healthBar.SetHealth(health);
        Debug.Log("Inimigo curou " + amount + " de vida! Vida atual: " + health);
    }

    public void SetAttributes(EnemyData data)
    {
        health = data.health;
        speed = data.speed;
        element = data.element;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = data.color; // Aplica a cor corretamente
    }
}
