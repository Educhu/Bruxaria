using UnityEngine;
using System.Collections.Generic;

public class BattleUIManager : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject spellButtons;
    public GameObject parchmentButtons;
    public GameObject LogText;
    public GameObject alfaBeto;

    private void Start()
    {
        RandomizeLetterTexts();
    }

    public void OpenSpellWriting()
    {
        RandomizeLetterTexts();

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

    private void RandomizeLetterTexts()
    {
        for (int i = 0; i < alfaBeto.transform.childCount; i++)
        {
            Transform letraObj = alfaBeto.transform.GetChild(i);

            Transform textoTransform = letraObj.Find("Text (TMP)");

            if (textoTransform != null)
            {

                bool mostrarTexto = Random.value > 0.5f;
                textoTransform.gameObject.SetActive(mostrarTexto);
            }
        }
    }
}
