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
    public Vector3 restartPosition;

    public AudioClip explosiveSound;
    public AudioClip goalSound;
    public bool isOnGround = true;
    public float jumpForce = 5f;
    private GameManager gameManager;
    public TextMeshProUGUI speedmeterText;
    public TextMeshProUGUI rpmText;
    public ParticleSystem dirtParticle;
    private bool isHighspeed = false;
    private bool isRunning = false;   
    public AudioClip drivingSound;
    public AudioClip highspeedSound;
    public AudioClip idleSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        restartPosition = new Vector3(-7.5f, 5f, -118f);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        audioSource.loop = true;
        audioSource.clip = idleSound;
        audioSource.volume = 0.2f;
        audioSource.Play();

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
                isHighspeed = false;
                isRunning = false;            
            } else if(isOnGround)
            {
                playerRb.AddRelativeForce(Vector3.forward * forwardInput * horsePower);
                transform.Rotate(Vector3.up, horizontalInput * rotateSpeed);
                speed = Mathf.Round(playerRb.velocity.magnitude * 3.6f);  // 2.237f to MPH value

                if (speed > 0 && !isRunning)
                {
                    isRunning = true;
                    // play running sound  
                    playClip(drivingSound, 0.7f);

                } else if (speed < 1 && isRunning)
                {
                    isRunning = false;
                    // stop running sound
                    audioSource.Stop();
                    playClip(idleSound, 0.2f);
                }

                if (speed > 100 && !isHighspeed)
                {
                    dirtParticle.Play();
                    isHighspeed = true;
                    audioSource.Stop();
                    playClip(highspeedSound, 0.2f);

                } else if(speed < 100 && isHighspeed)
                {
                    dirtParticle.Stop();
                    isHighspeed = false;
                    audioSource.Stop();
                    playClip(drivingSound, 0.7f);
                }

                if (speed >= 100)
                {
                    speedmeterText.color = new Color(255, 0, 0, 255);
                    rpmText.color = new Color(255, 0, 0, 255);
                } else
                {
                    speedmeterText.color = new Color(0, 255, 0, 255);
                    rpmText.color = new Color(0, 255, 0, 255);
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
            transform.position = restartPosition;
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

    private void playClip(AudioClip clipName, float clipVolume)
    {
        audioSource.loop = true;
        audioSource.clip = clipName;
        audioSource.volume = clipVolume;
        audioSource.Play();
    }
}
