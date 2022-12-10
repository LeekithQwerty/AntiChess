using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameSetter: MonoBehaviour
{

    [SerializeField] GameObject player1GameObject;
    [SerializeField] GameObject player2GameObject;
    public string player1name;
    public TMP_InputField player1nameIF;
    public string player2name;
    public TMP_InputField player2nameIF;


    public void setPlayername()
    {
        player1name = player1nameIF.text;
        player2name = player2nameIF.text;
        Debug.Log(player1name);
    }


    public string getPlayer1name()
    {
        return player1name;
    }

    public string getPlayer2name()
    {
        return player2name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
