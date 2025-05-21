using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int speed = 10;
    public Element element = Element.Fire;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public HealthBar healthBar;

    private Animator animator;

    [Header("Animator Setup")]
    public RuntimeAnimatorController baseAnimatorController;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (healthBar != null)
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

                if (animator != null)
                    animator.SetInteger("ElementIndex", (int)data.element); // Muda a animação via parâmetro

                if (healthBar != null)
                    healthBar.SetHealth(health);

                Debug.Log("Inimigo carregado: " + gameObject.name + " | Elemento: " + data.element);
            }
        }
    }

    public void SetAttributes(EnemyData data)
    {
        health = data.health;
        speed = data.speed;
        element = data.element;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(data.health);
            healthBar.SetHealth(data.health);
        }

        if (animator != null)
        animator.SetInteger("ElementIndex", data.elementIndex);
    }

    public ActionType ChooseAction(Player player)
    {
        int rand = Random.Range(0, 100);

        if (health <= 30 && rand < 50)
            return ActionType.Heal;
        else if (speed < player.speed && rand < 70)
            return ActionType.SpeedBoost;

        return ActionType.Attack;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0) health = 0;

        if (healthBar != null)
            healthBar.SetHealth(health);

        StartCoroutine(FlashRed());

        Debug.Log("Inimigo tomou " + amount + " de dano! Vida restante: " + health);
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer == null)
            yield break;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = originalColor;
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(health);

        Debug.Log("Inimigo curou " + amount + " de vida! Vida atual: " + health);
    }
}