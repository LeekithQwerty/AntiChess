using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;
public class GameManager1 : MonoBehaviour
{
    [SerializeField] GameObject chessPiece;
    [SerializeField] float timeRemainingBlack = 600f;
    [SerializeField] float timeRemainingWhite = 600f;
    [SerializeField] bool timerIsRunningBlack = false;
    [SerializeField] bool timerIsRunningWhite = false;

    //Canvas Text Control Variables
    public TextMeshProUGUI timeBlack;
    public TextMeshProUGUI timeWhite;
    public TextMeshProUGUI winnerText;
    [SerializeField] TextMeshProUGUI player1NameText;
    [SerializeField] TextMeshProUGUI player2NameText;

    private bool playMode = true;

    [SerializeField] GameObject translucent;
    [SerializeField] GameObject chessPieceSelectionChoice;
    private GameObject[,] positions = new GameObject[8, 8]; // a matrix of the chess board with player's piece's game objects at postions, others set at null
    private GameObject[] playerBlack = new GameObject[16]; // black player's  piece's ganme objects 
    private GameObject[] playerWhite = new GameObject[16]; // white player's  piece's ganme objects
    private GameObject[,] attackers = new GameObject[8, 8]; // all chess pieces that can possibly attack
    private GameObject[,] tempPositions = new GameObject[8, 8];
    private string currentPlayer = "white"; //current player's turn
    Dictionary<int, int> swap = new Dictionary<int, int>();

    public bool canAttack = false;
    public string colourPawn;
    private bool gameOver = false; // Game state
    int x, y;
    public string player1name;
    public string player2name;

    public void Start()
    {
        ///timeBlack = GetComponent<TextMeshProUGUI>();
        ///timeWhite = GetComponent<TextMeshProUGUI>();
        timerIsRunningWhite = true;
        swap.Add(0, 7);
        swap.Add(1, 6);
        swap.Add(2, 5);
        swap.Add(3, 4);
        swap.Add(4, 3);
        swap.Add(5, 2);
        swap.Add(6, 1);
        swap.Add(7, 0);
        //Instantiate(ChessPiece,new Vector3(1,0.38f,-1),Quaternion.identity); This line was the first line for instantiating the chess piece

        // add specific chess piecies with their start position in array
        playerWhite = new GameObject[] { CreateChessPiece("white_rook", 0, 0, "white"), CreateChessPiece("white_knight", 1, 0, "white") ,
            CreateChessPiece("white_bishop", 2, 0, "white"), CreateChessPiece("white_queen", 3, 0, "white"), CreateChessPiece("white_king", 4, 0, "white"),
            CreateChessPiece("white_bishop", 5, 0,"white"), CreateChessPiece("white_knight", 6, 0,"white"), CreateChessPiece("white_rook", 7, 0, "white"),
            CreateChessPiece("white_pawn", 0, 1, "white"), CreateChessPiece("white_pawn", 1, 1, "white"), CreateChessPiece("white_pawn", 2, 1, "white"),
            CreateChessPiece("white_pawn", 3, 1, "white"), CreateChessPiece("white_pawn", 4, 1, "white"), CreateChessPiece("white_pawn", 5, 1, "white"),
            CreateChessPiece("white_pawn", 6, 1, "white"), CreateChessPiece("white_pawn", 7, 1, "white")};
        playerBlack = new GameObject[] { CreateChessPiece("black_rook", 0, 7, "black"), CreateChessPiece("black_knight",1,7, "black"),
            CreateChessPiece("black_bishop",2,7, "black"), CreateChessPiece("black_queen",3,7, "black"), CreateChessPiece("black_king",4,7, "black"),
            CreateChessPiece("black_bishop",5,7, "black"), CreateChessPiece("black_knight",6,7, "black"), CreateChessPiece("black_rook",7,7, "black"),
            CreateChessPiece("black_pawn", 0, 6, "black"), CreateChessPiece("black_pawn", 1, 6, "black"), CreateChessPiece("black_pawn", 2, 6, "black"),
            CreateChessPiece("black_pawn", 3, 6, "black"), CreateChessPiece("black_pawn", 4, 6, "black"), CreateChessPiece("black_pawn", 5, 6, "black"),
            CreateChessPiece("black_pawn", 6, 6, "black"), CreateChessPiece("black_pawn", 7, 6, "black") };

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]); //FYI playerBlack[1] means create white rook at postion 0,0
            SetPosition(playerWhite[i]);
        }

        //username=Credentials.FindObjectOfType<Credentials>().getUsername();
        player1name = Credentials.FindObjectOfType<PlayerNameSetter>().getPlayer1name();
        player2name = Credentials.FindObjectOfType<PlayerNameSetter>().getPlayer2name();
        SetPlayerName();

    }

    private void SetPlayerName()
    {
        player1NameText.text = player1name;
        player2NameText.text = player2name;
    }
    public GameObject CreateChessPiece(string name, int x, int y, string colour)
    {
        GameObject obj = Instantiate(chessPiece, new Vector3(0, 0, -1), Quaternion.identity); //Accessing the object
        ChessPiece cp = obj.GetComponent<ChessPiece>(); // Accessing the script
        cp.name = name;  //Throwing the name to the  Chess Piece script
        cp.colour = colour;
        cp.SetXBoard(x);
        cp.SetYBoard(y);
        cp.Activate(); // activating this function as all the values were thrown
        return obj; // returing to the arrary
    }

    public void SetPosition(GameObject obj)
    {
        ChessPiece cp = obj.GetComponent<ChessPiece>();
        //Overwrites either empty space or whatever was there

        positions[cp.GetXBoard(), cp.GetYBoard()] = obj;
    }

    public void SetAttackers(GameObject attacker) // sets Active attackers in attackers matrix
    {
        ChessPiece cp = attacker.GetComponent<ChessPiece>();
        //Overwrites either empty space or whatever was there
        attackers[cp.GetXBoard(), cp.GetYBoard()] = attacker;
        canAttack = true;
    }
    public void SetAttackersNull() // sets attackers null 
    {
        canAttack = false;
        for (int y = 0; y <= 7; y++)
        {
            for (int x = 0; x <= 7; x++)
            {
                attackers[x, y] = null;
            }
        }
    }
    public bool CheckIfChessPieceIsInAttackers(GameObject checkAttacker)
    {
        for (int y = 0; y <= 7; y++)
        {
            for (int x = 0; x <= 7; x++)
            {
                if (attackers[x, y] == checkAttacker)
                    return true;
            }
        }
        return false;
    }
    public void SetPositionEmpty(int x, int y) //used to set the null vaules where the space is empty
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y) ///used to reurn the [position
    {
        return positions[x, y];
    }
    public GameObject GetBlackPosition(int i)
    {
        return playerBlack[i];
    }
    public GameObject GetWhitePosition(int i)
    {
        return playerWhite[i];
    }
    public bool PositionOnBoard(int x, int y) // this function returns true when the position in the boundry of the board
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer() // To get the current player string : white or black
    {
        return currentPlayer;
    }

    public bool IsGameOver()  // Check the if the game is over
    {
        return gameOver;
    }
    public void ChessPieceSelectionChoice() //pawn end selection 
    {
        translucent.SetActive(true);
        chessPieceSelectionChoice.SetActive(true);

    }

    public void getChessPiece(string colourpawn, int x, int y)
    {
        colourPawn = colourpawn;
        this.x = x;
        this.y = y;
    }
    public void onClickChessPieceChoice(string chessPieceChoice)
    {

        translucent.SetActive(false);
        chessPieceSelectionChoice.SetActive(false);
        SetPosition(CreateChessPiece(colourPawn + "_" + chessPieceChoice, x, y, colourPawn));
    }

    public void NextTurn()  // To switch the turn from one player to another
    {
        //flipBoard();
        //Debug.Log("positions...... ");
        SetAttackersNull();
        CheckIfPlayerLeft();
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
            timerIsRunningBlack = true;
            timerIsRunningWhite = false;

        }
        else
        {
            currentPlayer = "white";
            timerIsRunningWhite = true;
            timerIsRunningBlack = false;
        }
        CheckOpponentInPath();

    }
    private void CheckOpponentInPath()
    {

        if (currentPlayer == "black") // checks for black
        {
            for (int x = 0; x <= 7; x++)
            {
                for (int y = 0; y <= 7; y++)
                {
                    if (positions[x, y] == null)
                    {
                        continue;
                    }
                    else if (positions[x, y].GetComponent<ChessPiece>().colour == "black")
                    {
                        positions[x, y].GetComponent<ChessPiece>().CheckForAttack();
                    }
                }
            }
        }

        if (currentPlayer == "white") // checks for white
        {
            for (int x = 0; x <= 7; x++)
            {
                for (int y = 0; y <= 7; y++)
                {
                    if (positions[x, y] == null)
                    {
                        continue;
                    }
                    else if (positions[x, y].GetComponent<ChessPiece>().colour == "white")
                    {
                        positions[x, y].GetComponent<ChessPiece>().CheckForAttack();
                    }
                }
            }
        }
    }
    private void flipBoard()
    {
        Debug.Log("YYYYYYYYYYYYYYYYYYYYYYYYYY");
        for (int x = 0; x <= 7; x++)
        {
            for (int y = 0; y <= 7; y++)
            {
                Debug.Log(positions[x, y]);
            }
        }
        Debug.Log("Flipped");
        for (int x = 0; x <= 7; x++)
        {
            for (int y = 0; y <= 7; y++)
            {
                tempPositions[x, y] = positions[x, y];


                if (positions[x, y] == null)
                {
                    continue;
                }
                else
                {

                    GameObject cp = GetComponent<GameManager>().GetPosition(x, y);
                    Destroy(cp);
                }

            }
        }

        for (int x = 0; x <= 7; x++)
        {
            for (int y = 0; y <= 7; y++)
            {

                positions[swap[x], swap[y]] = tempPositions[x, y];


                if (positions[swap[x], swap[y]] == null)
                {
                    continue;

                }
                else
                {


                    ChessPiece cp = positions[swap[x], swap[y]].GetComponent<ChessPiece>();
                    CreateChessPiece(cp.name, swap[x], swap[y], cp.colour);

                }


            }


        }
        Debug.Log("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
        for (int x = 0; x <= 7; x++)
        {
            for (int y = 0; y <= 7; y++)
            {
                Debug.Log(positions[x, y]);
            }
        }
        /*
        for (int x = 0; x <= 7; x++)
        {
            for (int y = 0; y <= 7; y++)
            {
                if (positions[x, y] == null)
                {
                    continue;
                }
                else
                {
                    ChessPiece cp = positions[x, y].GetComponent<ChessPiece>(); //destroying script
                    // Debug.Log(string.Format("{0} {1} :", x, y));
                    // Debug.Log(cp.name == "black_pawn");
                    Destroy(cp);

                }

            }
        }
        */
    }

    public void Update()
    {
        if (playMode == true)
        {
            if (timerIsRunningBlack)
            {
                if (timeRemainingBlack > 0)
                {
                    timeRemainingBlack -= Time.deltaTime;
                    DisplayTime(timeRemainingBlack);
                }
                else
                {
                    timeRemainingBlack = 0;
                    timerIsRunningBlack = false;
                }
            }
            if (timerIsRunningWhite)
            {
                if (timeRemainingWhite > 0)
                {
                    timeRemainingWhite -= Time.deltaTime;
                    DisplayTime(timeRemainingWhite);
                }
                else
                {

                    timeRemainingWhite = 0;
                    timerIsRunningWhite = false;
                }
            }
            if (gameOver == true && Input.GetMouseButtonDown(0))
            {
                gameOver = false;
                SceneManager.LoadScene("Game"); //Restarts the game by loading the scene over again
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (timerIsRunningBlack)
        {
            timeBlack.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        if (timerIsRunningWhite)
        {
            timeWhite.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }


    public void CheckIfPlayerLeft()
    {

        Debug.Log("Called");
        int counterBlack = 0;
        int counterWhite = 0;
        bool isWinner = true;

        for (int x = 0; x <= 7; x++)
        {
            for (int y = 0; y <= 7; y++)
            {
                if (positions[x, y] == null)
                {
                    Debug.Log("IN NULL");
                    continue;
                }
                else if (positions[x, y].GetComponent<ChessPiece>().colour == "black")
                {
                    counterBlack++;
                }
                else if (positions[x, y].GetComponent<ChessPiece>().colour == "white")
                {
                    counterWhite++;
                }
            }
        }




        Debug.Log(counterWhite);
        Debug.Log(counterBlack);
        if (counterWhite == 0)
        {
            playMode = false;
            Winner("White");
        }
        else if (counterBlack == 0)
        {
            playMode = false;
            Winner("Black");
        }


    }
    public void Winner(string playerWinner)
    {
        gameOver = true;
        winnerText.text = playerWinner + " is the winner";
        translucent.SetActive(true);

    }

}
