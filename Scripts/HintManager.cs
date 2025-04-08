using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hintCounterText; // Hint count UI
    [SerializeField] private Button hintButton;       // Hint button (bulb icon)
    [SerializeField] private GameObject hintPanel;    // Panel that shows the hint
    [SerializeField] private TMP_Text hintMessageText; // Hint message inside the panel

    private int hintsAvailable;
    private string levelHintKey;

    private void Start()
    {
        hintsAvailable = PlayerPrefs.GetInt("HintCount", 2); // Load total hints
        levelHintKey = "HintUsed_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        UpdateHintUI();
        hintButton.onClick.AddListener(UseHint);
    }

    private void UseHint()
    {
        // Check if the hint was already used
        if (PlayerPrefs.GetInt(levelHintKey, 0) == 0)
        {
            if (hintsAvailable > 0)
            {
                hintsAvailable--;
                PlayerPrefs.SetInt("HintCount", hintsAvailable);
                PlayerPrefs.SetInt(levelHintKey, 1); // Mark hint as used for this level
                PlayerPrefs.Save();
            }
        }

        ShowHint();
        UpdateHintUI();
    }

    private void ShowHint()
    {
        hintPanel.SetActive(true); // Show the hint panel
        hintMessageText.text = GetLevelHint(); // Set the hint message
    }

    private void UpdateHintUI()
    {
        hintCounterText.text = hintsAvailable.ToString();

        // Keep the hint button active if the hint was used before
        hintButton.interactable = (hintsAvailable > 0 || PlayerPrefs.GetInt(levelHintKey, 0) == 1);
    }

    private string GetLevelHint()
    {
        // Assign unique hints per level
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        switch (levelName)
        {
            case "Level1": return "Try jumping over the obstacles!";
            case "Level2": return "Use the lever to open the gate!";
            case "Level3": return "The key is hidden behind the rocks!";
            default: return "Best Of Luck!";
        }
    }

    public void CloseHintPanel()
    {
        hintPanel.SetActive(false); // Hide the hint panel when closed
    }
}
