using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static bool _isPaused;
    private static bool _isGameOver;
    public static bool IsPaused {get{return _isPaused;}}
    public static bool IsGameOver {get{return _isGameOver;}}

    private static GameObject gameOverScreen;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("Game manager was not created");
            }            

            return _instance;
        }        
    }

    private void Awake()
    {
        Debug.Log("Manager awake");
        _instance = this;

        if (_instance != null && _instance != this)
        {          
            Debug.Log("Manager destroyed");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Manager else");
            _instance = this;
        }

        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen");
        gameOverScreen.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void GameOver()
    {
        _isGameOver = true;

        gameOverScreen.SetActive(true);

        PauseGame();
    }  

    public void RestartLevel()
    {
        if(_isPaused)
        {
            UnpauseGame();
        }

        _isGameOver = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}
