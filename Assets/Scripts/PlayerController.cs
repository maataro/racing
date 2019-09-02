using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rpm;
    [SerializeField] private float horsePower = 20f;
    [SerializeField] private float rotateSpeed = 0.05f;
    private Rigidbody playerRb;
    private AudioSource audioSource;
    [SerializeField] private Vector3 startPosition;
    public AudioClip explosiveSound;
    public AudioClip goalSound;
    public bool isOnGround = true;
    public float jumpForce = 5f;
    private GameManager gameManager;
    public TextMeshProUGUI speedmeterText;
    public TextMeshProUGUI rpmText;
    public ParticleSystem dirtParticle;
    private bool isRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startPosition = new Vector3(-7.5f, 5f, -118f);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameActive)
        {
            float forwardInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                forwardInput = 0f;
                //dirtParticle.Stop();
                //playerAudio.PlayOneShot(jumpSound, 1.0f);

                dirtParticle.Stop();
                isRunning = false;

            
            } else if(isOnGround)
            {
                playerRb.AddRelativeForce(Vector3.forward * forwardInput * horsePower);
                transform.Rotate(Vector3.up, horizontalInput * rotateSpeed);
                speed = Mathf.Round(playerRb.velocity.magnitude * 3.6f);  // 2.237f to MPH value

                if (speed > 100 && !isRunning)
                {
                    dirtParticle.Play();
                    isRunning = true;
                } else if(speed < 100 && isRunning)
                {
                    dirtParticle.Stop();
                    isRunning = false;                    
                }

                if (speed >= 100)
                {
                    speedmeterText.color = new Color(255, 0, 0, 255);
                } else
                {
                    speedmeterText.color = new Color(0, 255, 0, 255);
                }
                speedmeterText.SetText("Speed: " + speed + " km/h");
                rpm = Mathf.Round((speed % 30) * 40);

                rpmText.SetText("RPM: " + rpm);
            }


        }
                             
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Bottom"))
        {
            playerRb.velocity = Vector3.zero;
            transform.position = startPosition;
            //transform.rotation = Quaternion.Euler(Vector3.forward);
            
        }       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Explosive"))
        {
            audioSource.PlayOneShot(explosiveSound);
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            //audioSource.PlayOneShot(goalSound);
            gameManager.GoalSound();
            gameManager.ClearGame();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            //dirtParticle.Play();
        }
    }
}

/*
public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce = 5f;
    public float gravityModifier = 0f;
    public bool isOnGround = true;
    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);

        }
    }
}
*/