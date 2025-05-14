using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour
{
    public static BattleLog Instance { get; private set; }
    public TextMeshProUGUI logText;
    public ScrollRect scrollRect;
    private List<string> logEntries = new List<string>();
    private const int maxEntries = 20;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddLogEntry(string entry)
    {
        logEntries.Add(entry);
        if (logEntries.Count > maxEntries)
        {
            logEntries.RemoveAt(0);
        }
        UpdateLogDisplay();
    }

    private void UpdateLogDisplay()
    {
        logText.text = string.Join("\n", logEntries);
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f; // Mantém a rolagem no final
    }
}