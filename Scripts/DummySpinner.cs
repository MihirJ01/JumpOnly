using UnityEngine;
using System.Collections;

public class DummySpinner : MonoBehaviour
{
    public float minSpinSpeed = 100f;  // Minimum spin speed
    public float maxSpinSpeed = 300f;  // Maximum spin speed
    public float minSpinTime = 1f;     // Minimum time before stopping
    public float maxSpinTime = 3f;     // Maximum time before stopping
    public float minStopTime = 0.5f;   // Minimum stop time before spinning again
    public float maxStopTime = 2f;     // Maximum stop time before spinning again

    private float spinSpeed = 0f;
    private bool isSpinning = false;

    void Start()
    {
        StartCoroutine(RandomSpin());
    }

    IEnumerator RandomSpin()
    {
        while (true)
        {
            // Set random spin speed
            spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
            isSpinning = true;

            // Spin for a random duration
            float spinDuration = Random.Range(minSpinTime, maxSpinTime);
            yield return new WaitForSeconds(spinDuration);

            // Stop spinning
            isSpinning = false;
            spinSpeed = 0f;

            // Wait for a random stop duration
            float stopDuration = Random.Range(minStopTime, maxStopTime);
            yield return new WaitForSeconds(stopDuration);
        }
    }

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        }
    }
}
