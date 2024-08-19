using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    [Header("Audio Settings")]
    public AudioDuck audioDucking;
    public AudioClip woodFootstep;
    public AudioClip grassFootstep;
    public float footstepInterval = 0.5f;
    
    private Rigidbody2D rb;
    private AudioSource audioSource;
    
    private bool isGrounded;
    private GameObject currentFloor;
    
    private float footstepTimer;
    private string currentGroundTag;
    private int woodLayer;
    private int grassLayer;

    private float moveInput;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        woodLayer = LayerMask.NameToLayer("WoodLayer");
        grassLayer = LayerMask.NameToLayer("GrassLayer");
        Debug.Log("Wood Layer: " + woodLayer);
        Debug.Log("Grass Layer: " + grassLayer);
    }

    void Update()
    {

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (isGrounded && moveInput != 0)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval;
            }
        }

        if (moveInput == 0)
        {
            audioSource.Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentFloor = collision.gameObject;
        if (collision.gameObject.layer == woodLayer || collision.gameObject.layer == grassLayer && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Grounded on: " + LayerMask.LayerToName(collision.gameObject.layer));
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentFloor = null;
        if (collision.gameObject.layer == woodLayer || collision.gameObject.layer == grassLayer && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Left ground: " + LayerMask.LayerToName(collision.gameObject.layer));
        }
    }

    void PlayFootstepSound()
    {
        if (isGrounded)
        {

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (currentFloor.layer == woodLayer)
            {
                audioSource.clip = woodFootstep;
                Debug.Log("Playing wood footstep sound");
            }
            else if (currentFloor.layer == grassLayer)
            {
                audioSource.clip = grassFootstep;
                Debug.Log("Playing grass footstep sound");
            }

            audioSource.Play();
            audioDucking.DuckAudio();
        }
    }
}