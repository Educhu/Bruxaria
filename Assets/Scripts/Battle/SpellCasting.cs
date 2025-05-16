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
        spellBook.Add("CURA", (ActionType.Heal, Element.Air)); // Cura n�o tem elemento relevante
        spellBook.Add("VELOCIDADE", (ActionType.SpeedBoost, Element.Air)); // Velocidade tamb�m n�o
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
            BattleLog.Instance.AddLogEntry("Feiti�o lan�ado: " + spellName);

            var (actionType, spellElement) = spellBook[spellName]; // Obt�m a��o e elemento

            // Se for um ataque, define o elemento corretamente
            if (actionType == ActionType.Attack)
            {
                player.SetSelectedElement(spellElement);
            }

            if (player == null)
            {
                Debug.LogError("ERRO: O player est� nulo no SpellCasting!");
            }
            else
            {
                player.ChooseAction(actionType);
            }
        }
        else
        {
            BattleLog.Instance.AddLogEntry("Nenhum feiti�o encontrado!");
        }

        currentInput.Clear();
        spellText.text = "";
    }
}