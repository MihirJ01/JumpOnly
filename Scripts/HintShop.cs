using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;

public class HintShop : MonoBehaviour, IUnityAdsShowListener
{
    [SerializeField] private TMP_Text hintCounterText; // UI text displaying hints
    [SerializeField] private Button watchAdButton;     // Button to watch an ad for hints

    [SerializeField] private string androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string iosAdUnitId = "Rewarded_iOS";

    private string adUnitId;

    private void Awake()
    {
        #if UNITY_IOS
            adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
            adUnitId = androidAdUnitId;
        #endif

        // Load hint counter on game start
        UpdateHintUI();

        // Add button listener
        watchAdButton.onClick.AddListener(ShowRewardedAd);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this); // Play the ad directly
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Ad watched, rewarding 3 hints.");

            // Reward the player with 3 hints
            int hints = PlayerPrefs.GetInt("HintCount", 2);
            hints += 3;
            PlayerPrefs.SetInt("HintCount", hints);
            PlayerPrefs.Save();

            // Update UI
            UpdateHintUI();
        }
    }

    private void UpdateHintUI()
    {
        hintCounterText.text = PlayerPrefs.GetInt("HintCount", 2).ToString();
    }

    // Unused Unity Ads Callbacks (Required for Interface)
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}
