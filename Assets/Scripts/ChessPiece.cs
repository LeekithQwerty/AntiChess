using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject movableTile;

    private int xBoard = 1;
    private int yBoard = 1;


    private bool firstPawnMove = true;
    private bool checkForAttack = false;
    private bool instantiateOnlyAttackTiles = false;
    private string player; // black player or white player
    private string playerChoice;
    public string colour;
    public bool chessPieceAtEnd=false;
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;
    void Start()
    {
        firstPawnMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void Activate()
    {
        //Get the game manager
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        //Take the instantiated location and adjust transform
        SetCoords();

        //Choose correct sprite based on piece's name
        switch (this.name) //catching the name from GameManager script
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        //Get the board value in order to convert to xy coords
        float xCord = xBoard;
        float yCord = yBoard+0.38f;
        this.transform.position = new Vector3(xCord, yCord, (yCord - 9));
    }


    //set
    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
         // setting the layer of the player so that its visible 
    }

    public void SetPlayerLayer()
    {
        GameObject cp = gameManager.GetComponent<GameManager>().GetPosition(xBoard, yBoard);
        Debug.Log(-(yBoard + 1));
        cp.transform.position = new Vector3(xBoard, yBoard, -(yBoard+1));
    }
    //get
    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }

    private void OnMouseUp()
    {
        GameManager sc = gameManager.GetComponent<GameManager>();
        Debug.Log("ON CLICK");
        if (!sc.IsGameOver() && sc.GetCurrentPlayer() == player)
        {
            GameObject cp = sc.GetPosition(xBoard, yBoard);
            DestroyMovableTiles();

            if (sc.canAttack == false)
            {
                
                checkForAttack = false;
                InitiateMovableTiles();
                Debug.Log("attakers cannot attack ");
            }
            else if (sc.canAttack==true && !sc.CheckIfChessPieceIsInAttackers(cp)) //if attakers can attack and if this piece is in not in attackers then 
            {
                //Debug.Log(sc.CheckIfChessPieceIsInAttackers(cp));
                Debug.Log("attakers can attack but this piece is in not in attackers");
            }
            else if(sc.canAttack == true && sc.CheckIfChessPieceIsInAttackers(cp)) //if attakers can attack and if this piece is in attackers then 
            {
                Debug.Log("attakers can attack and this piece is in attackers");
                instantiateOnlyAttackTiles = true;
                InitiateMovableTiles();
                instantiateOnlyAttackTiles = false;
                Debug.Log(instantiateOnlyAttackTiles);
            }
            else
            {
                Debug.Log("CHESSPIECE ERROR");
            }
            
            
        }
    }

  

    public void DestroyMovableTiles()
    {
        GameObject[] movableTiles = GameObject.FindGameObjectsWithTag("MovableTile");
        for( int i = 0; i < movableTiles.Length; i++)
        {
            Destroy(movableTiles[i]);
        }
    }

    public void InitiateMovableTiles()
    {
        Debug.Log(this.name);
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                QueenMoves(checkForAttack); 
                break;
            case "black_knight":
            case "white_knight":
                KnightMoves(checkForAttack );
                break;
            case "black_bishop":
            case "white_bishop":
                BishopMoves(checkForAttack);
                break;
            case "black_king":
            case "white_king":
                KingMoves(checkForAttack);
                break;
            case "black_rook":
            case "white_rook":
                RookMoves(checkForAttack);
                break;
            case "black_pawn":
            case "white_pawn":
                PawnMove(checkForAttack);
                break;
        }
    }

    private void PawnMove(bool checkForAttack)
    {
        if (player == "black")
        {
            if (firstPawnMove)
            {
                PawnMoves(xBoard, yBoard - 2, checkForAttack);
            }
            PawnMoves(xBoard, yBoard - 1, checkForAttack);
        }
        else
        {
            Debug.Log("IN PAWN MOVE WHITE");
            if (firstPawnMove)
            {
                PawnMoves(xBoard, yBoard + 2, checkForAttack);
            }
            PawnMoves(xBoard, yBoard + 1, checkForAttack);
        }
    }


    private void RookMoves(bool checkForAttack)
    {
        LineMovableTile(1, 0, checkForAttack);
        LineMovableTile(0, 1, checkForAttack);
        LineMovableTile(-1, 0, checkForAttack);
        LineMovableTile(0, -1, checkForAttack);
    }

    private void BishopMoves(bool checkForAttack)
    {
        LineMovableTile(1, 1, checkForAttack);
        LineMovableTile(1, -1, checkForAttack);
        LineMovableTile(-1, 1, checkForAttack);
        LineMovableTile(-1, -1, checkForAttack);
    }

    private void QueenMoves(bool checkForAttack)
    {
        LineMovableTile(1, 0, checkForAttack);
        LineMovableTile(0, 1, checkForAttack);
        LineMovableTile(1, 1, checkForAttack);
        LineMovableTile(-1, 0, checkForAttack);
        LineMovableTile(0, -1, checkForAttack);
        LineMovableTile(-1, -1, checkForAttack);
        LineMovableTile(-1, 1, checkForAttack);
        LineMovableTile(1, -1, checkForAttack);
    }

    public void KnightMoves(bool checkForAttack)
    {
        PointMovableTile(xBoard + 1, yBoard + 2, checkForAttack);
        PointMovableTile(xBoard - 1, yBoard + 2, checkForAttack);
        PointMovableTile(xBoard + 2, yBoard + 1, checkForAttack);
        PointMovableTile(xBoard + 2, yBoard - 1, checkForAttack);
        PointMovableTile(xBoard + 1, yBoard - 2, checkForAttack);
        PointMovableTile(xBoard - 1, yBoard - 2, checkForAttack);
        PointMovableTile(xBoard - 2, yBoard + 1, checkForAttack);
        PointMovableTile(xBoard - 2, yBoard - 1, checkForAttack);
    }

    public void KingMoves(bool checkForAttack)
    {
        PointMovableTile(xBoard, yBoard + 1, checkForAttack);
        PointMovableTile(xBoard, yBoard - 1, checkForAttack);
        PointMovableTile(xBoard - 1, yBoard, checkForAttack);
        PointMovableTile(xBoard - 1, yBoard - 1, checkForAttack);
        PointMovableTile(xBoard - 1, yBoard + 1, checkForAttack);
        PointMovableTile(xBoard + 1, yBoard , checkForAttack);
        PointMovableTile(xBoard + 1, yBoard - 1, checkForAttack);
        PointMovableTile(xBoard + 1, yBoard + 1, checkForAttack);
    }

    public void LineMovableTile(int xIncrement, int yIncrement, bool checkForAttac)
    {
               
        GameManager sc = gameManager.GetComponent<GameManager>(); //geting the GameManager script from gameManager Game obj

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;
               
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            if (instantiateOnlyAttackTiles == true && checkForAttac == false)
            {
                x += xIncrement;
                y += yIncrement;
            }
            else if (checkForAttac == false && instantiateOnlyAttackTiles == false)
            {
                
                CreateMovableTile(x, y);
                x += xIncrement;
                y += yIncrement;
                //Debug.Log("in checkattac");

            }
            else if (checkForAttac==true && instantiateOnlyAttackTiles == false)
            {
                x += xIncrement;
                y += yIncrement;
            }
            else
            {
                Debug.Log("ERROR IN LINE");
            }
            
        }
      
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<ChessPiece>().player != player && (checkForAttac == false) && (instantiateOnlyAttackTiles == true)) // checks if there is opponent piece after the last null
        {
            CreateAttackMovableTile(x, y);
        }
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<ChessPiece>().player != player && (checkForAttac == true))
        {
      
            GameObject cp = sc.GetPosition(xBoard, yBoard);
            sc.SetAttackers(cp);

        }
      
    }

   
    public void PointMovableTile(int x, int y, bool checkForAttac)
    {
        GameManager sc = gameManager.GetComponent<GameManager>();
        if (sc.PositionOnBoard(x, y)) // checks if future move is on the board
        {
            GameObject cp = sc.GetPosition(x, y);
            if (cp==null && checkForAttac == false && instantiateOnlyAttackTiles == false)
            {
                CreateMovableTile(x, y);
            }
            else if (cp == null)
            {
                Debug.Log("cp is null");
            }
            else if ((checkForAttac == false) && (instantiateOnlyAttackTiles == true) && cp.GetComponent<ChessPiece>().player != player)
            {
                CreateAttackMovableTile(x, y);
            }
            else if ((checkForAttac == true) && cp.GetComponent<ChessPiece>().player != player)
            {
                GameObject cpp = sc.GetPosition(xBoard, yBoard);
                sc.SetAttackers(cpp);
            }
            else
            {
                Debug.Log("THROW ERROR");
            }
        }
    }

    public void SetFirstPawnMoveFalse()
    {
        firstPawnMove = false;
    }

    public void PawnMoves(int x, int y, bool checkForAttac)
    {
        GameManager sc = gameManager.GetComponent<GameManager>();
        
            if (sc.PositionOnBoard(x, y)) // checks if future move is on the board
            {

                GameObject cp = sc.GetPosition(x, y);
                Debug.Log(cp);
                Debug.Log(checkForAttac);
                Debug.Log(instantiateOnlyAttackTiles);
                if (cp == null && checkForAttac == false && instantiateOnlyAttackTiles == false)
                {
                    CreateMovableTile(x, y);
                    
                }
                else
                {
                    if (colour == "black" && firstPawnMove==true)
                    {
                        y = y + 1;
                    }
                    else if(colour=="white" && firstPawnMove==true)
                    {
                        y = y - 1;
                    }
                    if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<ChessPiece>().player != player && (checkForAttac == false) && (instantiateOnlyAttackTiles == true))
                    {
                        CreateAttackMovableTile(x + 1, y);
                    }

                    else if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<ChessPiece>().player != player && (checkForAttac == false) && (instantiateOnlyAttackTiles == true))
                    {
                        CreateAttackMovableTile(x - 1, y);
                    }

                    else if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<ChessPiece>().player != player && (checkForAttac == true))
                    {
                        GameObject cpp = sc.GetPosition(xBoard, yBoard);
                        sc.SetAttackers(cpp);
                    }

                    else if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<ChessPiece>().player != player && (checkForAttac == true))
                    {
                        GameObject cpp = sc.GetPosition(xBoard, yBoard);
                        sc.SetAttackers(cpp);
                    }
                }
                
            }
        
       
    }

    public string GetChessPieceName()
    {
        return this.name;
    }
    public void CreateMovableTile(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;
      
        //Set actual unity values
        GameObject mp = Instantiate(movableTile, new Vector3(x, y, -9.9f), Quaternion.identity);

        MovableTile mpScript = mp.GetComponent<MovableTile>();
        mpScript.SetTappedChessPieceTile(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void CreateAttackMovableTile(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Set actual unity values
        GameObject mp = Instantiate(movableTile, new Vector3(x, y, -9.9f), Quaternion.identity);

        MovableTile mpScript = mp.GetComponent<MovableTile>();
        mpScript.attack = true;
        mpScript.SetTappedChessPieceTile(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }




    public void CheckForAttack()
    {
        bool checkForAttac = true;
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                QueenMoves(checkForAttac);
               
                break;
            case "black_rook":
            case "white_rook":
                RookMoves(checkForAttac);
                break;

            case "black_bishop":
            case "white_bishop":
                BishopMoves(checkForAttac);
                break;
            
            case "black_knight":
            case "white_knight":
                KnightMoves(checkForAttac);
                break;
            
            case "black_king":
            case "white_king":
                KingMoves(checkForAttac);
                break;
            
            case "black_pawn":
            case "white_pawn":
                PawnMove(checkForAttac);
                break;
             
        }
    }
    
}
