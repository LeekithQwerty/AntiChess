using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex -1);
    }
    public void LoadStartScene()
    {
        
        //FindObjectOfType<Level>().SetLevelNumber(0);
        //FindObjectOfType<GameSession>().SetScore(0);
        //Debug.Log("AFTER SET SCORE ");
       // FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {

        SceneManager.LoadScene(3);
    }

    public void LoadRulesScene()
    {

        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
