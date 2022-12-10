using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class User 
{
    public string username;
    public int score;
    public string localId;
    public User()
    {
        username = LoginManager.FindObjectOfType<LoginManager>().getUsername();
        score = 1;
        localId = LoginManager.FindObjectOfType<LoginManager>().getLocalId();
    }
}
