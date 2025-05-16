using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class SpellCasting : MonoBehaviour
{
    public TextMeshProUGUI spellText;
    private StringBuilder currentInput = new StringBuilder();
    private Dictionary<string, (ActionType, Element)> spellBook = new Dictionary<string, (ActionType, Element)>();
    public Player player;

    void Start()
    {
        player = FindAnyObjectByType<Player>(); // Encontra automaticamente o Player na cena

        if (player == null)
        {
            Debug.LogError("ERRO: Nenhum Player encontrado na cena!");
        }

        spellBook.Add("FOGO", (ActionType.Attack, Element.Fire));
        spellBook.Add("AGUA", (ActionType.Attack, Element.Water));
        spellBook.Add("TERRA", (ActionType.Attack, Element.Earth));
        spellBook.Add("AR", (ActionType.Attack, Element.Air));
        spellBook.Add("CURA", (ActionType.Heal, Element.Air)); // Cura não tem elemento relevante
        spellBook.Add("VELOCIDADE", (ActionType.SpeedBoost, Element.Air)); // Velocidade também não
    }

    public void AddLetter(string letter)
    {
        currentInput.Append(letter);
        spellText.text = currentInput.ToString();
    }

    public void ClearSpell()
    {
        currentInput.Clear();
        spellText.text = "";
    }

    public void CastSpell()
    {
        string spellName = currentInput.ToString().ToUpper();

        if (spellBook.ContainsKey(spellName))
        {
            BattleLog.Instance.AddLogEntry("Feitiço lançado: " + spellName);

            var (actionType, spellElement) = spellBook[spellName]; // Obtém ação e elemento

            // Se for um ataque, define o elemento corretamente
            if (actionType == ActionType.Attack)
            {
                player.SetSelectedElement(spellElement);
            }

            if (player == null)
            {
                Debug.LogError("ERRO: O player está nulo no SpellCasting!");
            }
            else
            {
                player.ChooseAction(actionType);
            }
        }
        else
        {
            BattleLog.Instance.AddLogEntry("Nenhum feitiço encontrado!");
        }

        currentInput.Clear();
        spellText.text = "";
    }
}