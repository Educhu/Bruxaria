using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public GameObject mainButtons;   // Painel dos botões principais (Ex: Escrever Feitiço, Usar Pergaminho)
    public GameObject spellButtons;  // Painel dos botões do alfabeto para escrever feitiço
    public GameObject parchmentButtons;
    public GameObject LogText;

    public void OpenSpellWriting()
    {
        mainButtons.SetActive(false);
        spellButtons.SetActive(true);
        LogText.SetActive(false);
    }

    public void CloseSpellWriting()
    {
        spellButtons.SetActive(false);
        mainButtons.SetActive(true);
        LogText.SetActive(true);
    }

    public void CloseParchmentButtons()
    {
        parchmentButtons.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void OpenParchmentButtons()
    {
        parchmentButtons.SetActive(true);
        mainButtons.SetActive(false);
    }
}
