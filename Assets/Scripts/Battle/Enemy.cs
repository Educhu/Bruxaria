using UnityEngine;
using System.Collections.Generic;
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

    [Header("Idle Clips")]
    public AnimationClip fireIdle;
    public AnimationClip waterIdle;
    public AnimationClip earthIdle;
    public AnimationClip airIdle;
    public AnimationClip metalIdle;
    public AnimationClip venomIdle;
    public AnimationClip rayIdle;

    [Header("Attack Clips")]
    public AnimationClip fireAttack;
    public AnimationClip waterAttack;
    public AnimationClip earthAttack;
    public AnimationClip airAttack;

    [Header("Hit Clips")]
    public AnimationClip fireHit;
    public AnimationClip waterHit;
    public AnimationClip earthHit;
    public AnimationClip airHit;

    [Header("Die Clips")]
    public AnimationClip fireDie;
    public AnimationClip waterDie;
    public AnimationClip earthDie;
    public AnimationClip airDie;

    private void Start()
    {
        animator = GetComponent<Animator>();
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

                // Define o parâmetro de animação
                animator.SetInteger("ElementIndex", (int)data.element);

                healthBar.SetHealth(health);
                Debug.Log("Inimigo carregado: " + gameObject.name + " | Elemento: " + data.element);
            }
        }
    }

    public enum ElementIndice
    {
        Fire = 0,
        Water = 1,
        Earth = 2,
        Air = 3,
        Metal = 4,
        Venom = 5,
        Ray = 6
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void SetAttributes(EnemyData data)
    {
        health = data.health;
        speed = data.speed;
        element = data.element;
        healthBar.SetMaxHealth(data.health);
        healthBar.SetHealth(data.health);
        ApplyElementAnimations(data);
    }

    public void ApplyElementAnimations(EnemyData data)
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        var overrideController = new AnimatorOverrideController(baseAnimatorController);

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        foreach (var pair in baseAnimatorController.animationClips)
        {
            switch (pair.name)
            {
                case "Idle":
                    overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(pair, data.idleClip));
                    break;
                case "Attack":
                    overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(pair, data.attackClip));
                    break;
                case "Hit":
                    overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(pair, data.hitClip));
                    break;
                case "Die":
                    overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(pair, data.dieClip));
                    break;
            }
        }

        overrideController.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrideController;
    }

    private AnimationClip GetIdleClip(Element e)
    {
        return e switch
        {
            Element.Fire => fireIdle,
            Element.Water => waterIdle,
            Element.Earth => earthIdle,
            Element.Air => airIdle,
            _ => fireIdle
        };
    }

    private AnimationClip GetAttackClip(Element e)
    {
        return e switch
        {
            Element.Fire => fireAttack,
            Element.Water => waterAttack,
            Element.Earth => earthAttack,
            Element.Air => airAttack,
            _ => fireAttack
        };
    }

    private AnimationClip GetHitClip(Element e)
    {
        return e switch
        {
            Element.Fire => fireHit,
            Element.Water => waterHit,
            Element.Earth => earthHit,
            Element.Air => airHit,
            _ => fireHit
        };
    }

    private AnimationClip GetDieClip(Element e)
    {
        return e switch
        {
            Element.Fire => fireDie,
            Element.Water => waterDie,
            Element.Earth => earthDie,
            Element.Air => airDie,
            _ => fireDie
        };
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
        healthBar.SetHealth(health);
        StartCoroutine(FlashRed());

        Debug.Log("Inimigo tomou " + amount + " de dano! Vida restante: " + health);
    }
    private IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color original = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = original;
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        healthBar.SetHealth(health);
        Debug.Log("Inimigo curou " + amount + " de vida! Vida atual: " + health);
    }
}