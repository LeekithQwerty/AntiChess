using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credentials : MonoBehaviour
{
    private string username;
    public int score;
    private void Awake()
    {
        

        DontDestroyOnLoad(gameObject);

 

    }


    public void setUsername(string username)
    {
        this.username = username;
    }
    public string getUsername()
    {
        return username;
    }
    public void setScore(int score)
    {
        this.score = score;
    }
    public int getScore()
    {
        return score;
    }
}
