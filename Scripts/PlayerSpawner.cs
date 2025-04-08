using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;  // Assign all character prefabs in the Inspector
    public Transform respawnPoint;      // Set this to your level's respawn point

    private static GameObject currentPlayer; // To track and destroy previous players

    void Start()
    {
        SpawnCharacter();
    }

    void SpawnCharacter()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);  // Destroy previous character if exists
        }

        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0); // Get selected character index
        Debug.Log("Selected Character Index: " + selectedCharacterIndex);

        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= playerPrefabs.Length)
        {
            Debug.LogError("Invalid Character Index: " + selectedCharacterIndex);
            return;
        }

        // Instantiate the new character at the respawn point
        currentPlayer = Instantiate(playerPrefabs[selectedCharacterIndex], respawnPoint.position, Quaternion.identity);
        Debug.Log("Spawned Character: " + currentPlayer.name);
    }
}
