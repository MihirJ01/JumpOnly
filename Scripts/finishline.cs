using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishline : MonoBehaviour
{
  [SerializeField] private AudioSource audioSource; // Assign the AudioSource in the Inspector

 private void OnTriggerEnter2D(Collider2D collision)
 {
    if(collision.CompareTag("Player"))
    {
        if (audioSource != null)
            {
                audioSource.Stop();  // Stop the assigned audio
            }
        unlockednewLevel();
    }
 }
 
 void unlockednewLevel()
 {
    if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
    {
        PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("UnlockedLevel" , PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
        PlayerPrefs.Save();
    }
 }
}
