using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;        
    public TextMeshProUGUI timerText;    
    public GameObject endGamePanel; 
    public TextMeshProUGUI finalScoreText;  

    private int score = 0;        
    private float timeRemaining = 60f; //Starting time
    private bool isGameOver = false;   
    private bool isWarningActive = false; //Last 10 seconds warning
    private bool isTimerActive = false; 

    void Start()
    {
        score = 0;
        UpdateScoreText();        
        UpdateTimerText();      
        endGamePanel.SetActive(false);
    }

    void Update()
    {
        if (isTimerActive && !isGameOver)
        {
            timeRemaining -= Time.deltaTime; 
            if (timeRemaining <= 10f && !isWarningActive)
            {
                isWarningActive = true;
                StartCoroutine(WarningEffect());
            }

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(); 
            }
            UpdateTimerText(); 
        }
    }

    public void AddScore(int points)
    {
        score += points;       
        StartCoroutine(TemporaryColorChange(points));
    }

    private void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score;

        if (score < 0)
        {
            scoreText.color = Color.red;
        }
        else
        {
            scoreText.color = Color.green;
        }
    }

    //If an unwanted object is hit, the score flashes red momentarily.
    private IEnumerator TemporaryColorChange(int points)
    {
        if (points < 0) 
        {
            scoreText.color = Color.red;        
            yield return new WaitForSeconds(0.5f); 
            UpdateScoreText();                 
        }
        else
        {
            UpdateScoreText();                   
        }
    }
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); //Calculate minutes
        int seconds = Mathf.FloorToInt(timeRemaining % 60); //Calculate seconds
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void GameOver()
    {
        isGameOver = true;         
        endGamePanel.SetActive(true); //Show end game panel
        finalScoreText.text = "FINAL SCORE:\n" + score;
        Time.timeScale = 0; 
    }
    public void StartGame()
    {
        if (!isTimerActive) 
        {
            isTimerActive = true;
        }
    }
    public void RestartGame() 
    {
        Time.timeScale = 1; 
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload game screen
    }

    //Last 10 seconds warning
    private IEnumerator WarningEffect()
    {
        while (timeRemaining > 0 && timeRemaining <= 10f)
        {
            timerText.color = Color.red;  
            yield return new WaitForSeconds(0.5f);
            timerText.color = Color.white;
            yield return new WaitForSeconds(0.5f); 
        }
        if (timeRemaining <= 0)
        {
            timerText.color = Color.red;
        }
    }
}
