using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Cinemachine; // Import Cinemachine

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float bounceForce = 10f;
    public float deathHeight = -10f;
    public float respawnDelay = 2f;

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // 🎥 Camera reference

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    public Button leftButton;
    public Button rightButton;

    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private SceneTransitionManager sceneTransition;

    [SerializeField] private ParticleSystem jumpEffect;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem[] finishEffect;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource bounceSound;

    void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransitionManager>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        if (respawnPoint == null)
            respawnPoint = transform;

        RemoveUnselectedCharacters();
        MoveToRespawnPoint();
        SetCameraTarget(); // 🎥 Assign selected character to Cinemachine camera

        if (leftButton != null)
            AddButtonListeners(leftButton, MoveLeftStart, MoveLeftStop);
        if (rightButton != null)
            AddButtonListeners(rightButton, MoveRightStart, MoveRightStop);
    }

    void RemoveUnselectedCharacters()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "");

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name != selectedCharacter)
            {
                Destroy(player);
            }
        }
    }

    void MoveToRespawnPoint()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
    }

    void SetCameraTarget()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = transform; // 🎥 Make the camera follow this player
        }
        else
        {
            Debug.LogWarning("Virtual Camera not assigned in the inspector!");
        }
    }

    void Update()
    {
        if (transform.position.y < deathHeight)
            Die();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            isMovingLeft = true;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            isMovingRight = true;

        if (Input.GetKeyUp(KeyCode.LeftArrow))
            isMovingLeft = false;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            isMovingRight = false;

        if (isMovingLeft)
            rb.velocity = new Vector2(-moveSpeed, bounceForce);
        else if (isMovingRight)
            rb.velocity = new Vector2(moveSpeed, bounceForce);
    }

    void MoveLeftStart() { isMovingLeft = true; }
    void MoveLeftStop() { isMovingLeft = false; }
    void MoveRightStart() { isMovingRight = true; }
    void MoveRightStop() { isMovingRight = false; }

    void AddButtonListeners(Button button, System.Action onPress, System.Action onRelease)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((eventData) => onPress.Invoke());
        trigger.triggers.Add(pointerDown);

        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((eventData) => onRelease.Invoke());
        trigger.triggers.Add(pointerUp);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.velocity = new Vector2(0, bounceForce);
            PlayParticle(jumpEffect);
            PlaySound(bounceSound);
        }
        else if (collision.gameObject.CompareTag("RWall") || collision.gameObject.CompareTag("LWall"))
        {
            Vector2 relativeVelocity = rb.velocity - collision.relativeVelocity;
            rb.velocity = new Vector2(-Mathf.Abs(relativeVelocity.x) * 0.5f, bounceForce);
            PlayParticle(jumpEffect);
            PlaySound(bounceSound);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish")) 
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            LevelComplete();
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");

        spriteRenderer.enabled = false;
        playerCollider.enabled = false;
        rb.simulated = false;

        if (deathEffect != null)
        {
            ParticleSystem instance = Instantiate(deathEffect, transform.position, Quaternion.identity);
            instance.Play();
            Destroy(instance.gameObject, 2f);
        }

        PlaySound(deathSound);
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnDelay);

        transform.position = respawnPoint.position;
        rb.simulated = true;
        spriteRenderer.enabled = true;
        playerCollider.enabled = true;
        Debug.Log("Player Respawned!");
    }

    void LevelComplete()
    {
        Debug.Log("Level Complete! Playing transition animation...");

        if (finishEffect != null && finishEffect.Length > 0)
        {
            foreach (ParticleSystem effect in finishEffect)
            {
                if (effect != null)
                {
                    ParticleSystem instance = Instantiate(effect, transform.position, Quaternion.identity);
                    instance.Play();
                    Destroy(instance.gameObject, 3f);
                }
            }
        }

        PlaySound(winSound);
        StartCoroutine(LevelTransition());
    }

    IEnumerator LevelTransition()
    {
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (sceneTransition != null)
        {
            sceneTransition.LoadNextLevel();
        }
        else
        {
            Debug.LogError("SceneTransitionManager is missing! Loading level directly.");
            LevelManager.instance.LoadNextLevel();
        }
    }

    void PlayParticle(ParticleSystem particle)
    {
        if (particle != null)
        {
            ParticleSystem instance = Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, 2f);
        }
    }

    void PlaySound(AudioSource sound)
    {
        if (sound != null)
            sound.Play();
    }
}
