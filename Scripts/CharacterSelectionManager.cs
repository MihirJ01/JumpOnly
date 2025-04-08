using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public Image characterDisplay;   // UI Image for character display
    public TMP_Text buyButtonText;   // TMP Text for Buy/Select button
    public Button leftButton, rightButton, actionButton; // Buttons
    public Animator characterAnimator;  // Animator for character bounce effect

    public Sprite[] characterImages;  // Assign character UI images
    public string[] characterNames;   // Assign character names (must match prefab names)

    private int currentCharacterIndex = 0;

    void Start()
    {
        InitializeCharacterSelection();
        leftButton.onClick.AddListener(PreviousCharacter);
        rightButton.onClick.AddListener(NextCharacter);
        actionButton.onClick.AddListener(OnActionButtonClicked);
    }

    private void InitializeCharacterSelection()
    {
        // If no character is selected, default to the first one
        if (!PlayerPrefs.HasKey("SelectedCharacter"))
        {
            PlayerPrefs.SetString("SelectedCharacter", characterNames[0]);
            PlayerPrefs.SetInt("Purchased_" + characterNames[0], 1); // Mark first character as purchased
            PlayerPrefs.Save();
        }

        LoadCharacter();
    }

    public void NextCharacter()
    {
        PlayBounceAnimation();
        currentCharacterIndex = (currentCharacterIndex + 1) % characterImages.Length;
        UpdateCharacter();
    }

    public void PreviousCharacter()
    {
        PlayBounceAnimation();
        currentCharacterIndex = (currentCharacterIndex - 1 + characterImages.Length) % characterImages.Length;
        UpdateCharacter();
    }

    private void OnActionButtonClicked()
    {
        string characterName = characterNames[currentCharacterIndex];

        if (IsCharacterPurchased(characterName))
        {
            SelectCharacter();
        }
        else
        {
            BuyCharacter();
        }
    }

    private void SelectCharacter()
    {
        string selectedCharacterName = characterNames[currentCharacterIndex];
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacterName);
        PlayerPrefs.Save();
        UpdateCharacter();
    }

    private void BuyCharacter()
    {
        string characterName = characterNames[currentCharacterIndex];

        // Mark the character as purchased (Free for now)
        PlayerPrefs.SetInt("Purchased_" + characterName, 1);
        PlayerPrefs.Save();

        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        characterDisplay.sprite = characterImages[currentCharacterIndex];
        string savedCharacter = PlayerPrefs.GetString("SelectedCharacter", characterNames[0]);
        string currentCharacter = characterNames[currentCharacterIndex];

        if (IsCharacterPurchased(currentCharacter))
        {
            actionButton.interactable = true;
            buyButtonText.text = (currentCharacter == savedCharacter) ? "Selected" : "Select";
        }
        else
        {
            actionButton.interactable = true;
            buyButtonText.text = "Free";
        }
    }

    private void LoadCharacter()
    {
        string savedCharacter = PlayerPrefs.GetString("SelectedCharacter", characterNames[0]);

        for (int i = 0; i < characterNames.Length; i++)
        {
            if (characterNames[i] == savedCharacter)
            {
                currentCharacterIndex = i;
                break;
            }
        }

        UpdateCharacter();
    }

    private bool IsCharacterPurchased(string characterName)
    {
        return PlayerPrefs.GetInt("Purchased_" + characterName, 0) == 1;
    }

    private void PlayBounceAnimation()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("Bounce");
        }
    }
}
