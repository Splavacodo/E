using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0; 
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText; 

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length; 
        if (numGameSessions > 1)
        {
            gameObject.SetActive(false); 
            Destroy(gameObject); 
        }
        else
        {
            DontDestroyOnLoad(gameObject); 
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString(); 
    }

    public void ProccessPlayerDeath()
    {
        if(playerLives > 1)
        {
            SubtractLife(); 
        }
        else
        {
            ResetGameSession(); 
        }
    }

    private void SubtractLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString(); 
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject); 
    }

}
