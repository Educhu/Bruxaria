using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public Button[] phaseButtons; // Arraste os botões das fases no Inspector

    private void Start()
    {
        int unlockedPhase = GameManager.Instance.unlockedPhase;

        for (int i = 0; i < phaseButtons.Length; i++)
        {
            phaseButtons[i].interactable = (i + 1) <= unlockedPhase;
        }
    }

    public void StartBattle(int phase)
    {
        GameManager.Instance.StartBattle(phase);
    }
}