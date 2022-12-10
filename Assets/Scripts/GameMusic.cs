using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        int musicStatusCount = FindObjectsOfType<GameMusic>().Length;
        if (musicStatusCount > 1)
        {
          

            Destroy(gameObject);

        }
        else
        {
         
            DontDestroyOnLoad(gameObject);
        }
       
    }
}

