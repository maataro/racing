using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip goalSound;
    public bool isGameActive;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject gameClearScreen;
    public NpcController npcController;
    private float soundVolume = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        npcController = GameObject.Find("NPC").GetComponent<NpcController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        npcController.speed = difficulty;
        titleScreen.SetActive(false);
        audioSource.volume = soundVolume;
        audioSource.Play();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        isGameActive = false;
        //audioSource.Stop(); 
    }

    public void ClearGame()
    {
        gameClearScreen.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        audioSource.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoalSound()
    {
        audioSource.PlayOneShot(goalSound);
    }
}
