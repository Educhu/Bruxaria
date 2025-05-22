using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int speed = 15;
    public Element element = Element.Air; // O elemento base do player é Ar
    public Element selectedElement = Element.Air; // Elemento do ataque escolhido pelo feitiço

    public HealthBar healthBar; // Referência para a barra de vida

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void SetSelectedElement(Element newElement)
    {
        selectedElement = newElement;
    }
    public void HealAction()
    {
        ChooseAction(ActionType.Heal);
    }

    public void SpeedBoostAction()
    {
        ChooseAction(ActionType.SpeedBoost);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0) health = 0;

        healthBar.SetHealth(health);

        Debug.Log("Player tomou " + amount + " de dano! Vida restante: " + health);

        StartCoroutine(FlashRed());
    }

    public void IncreaseHealth(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        healthBar.SetHealth(health);
        Debug.Log("Player curou " + amount + " de vida! Vida atual: " + health);
    }

    public void IncreaseSpeed(int amount)
    {
        speed += amount;
        Debug.Log("Player aumentou a velocidade para: " + speed);
    }

    public void ChooseAction(ActionType action)
    {
        BattleController battleController = FindAnyObjectByType<BattleController>();

        if (battleController == null)
        {
            Debug.LogError("ERRO: BattleController não encontrado na cena!");
            return;
        }

        battleController.PlayerCastsSpell(action);
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer == null)
            yield break;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = originalColor;
    }

}

