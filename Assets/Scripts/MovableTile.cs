using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTile : MonoBehaviour
{
    [SerializeField] private Color moveColor, attackColor;
    public GameObject gameManager;

    //The Chesspiece that was tapped to create this Movable Tile
    GameObject tappedChessPiece = null;

    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = attackColor;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = moveColor;
        }
    }
    public void OnMouseUp()
    {
        Debug.Log("MATRIX Y");
        Debug.Log(matrixY);
        string checkColour="likhit";
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        ChessPiece cpsc = tappedChessPiece.GetComponent<ChessPiece>();
        //Destroy the victim Chesspiece
        if (attack)
        {
            GameObject cp = gameManager.GetComponent<GameManager>().GetPosition(matrixX, matrixY);
            
            Destroy(cp);
           
        }
        //Set the Chesspiece's original location to be empty
        gameManager.GetComponent<GameManager>().SetPositionEmpty(tappedChessPiece.GetComponent<ChessPiece>().GetXBoard(),tappedChessPiece.GetComponent<ChessPiece>().GetYBoard());
        
        if ((cpsc.GetChessPieceName() == "black_pawn" || cpsc.GetChessPieceName() == "white_pawn") && (matrixY == 7 || matrixY == 0))
        {
            gameManager.GetComponent<GameManager>().getChessPiece(cpsc.colour,matrixX,matrixY);
            gameManager.GetComponent<GameManager>().ChessPieceSelectionChoice();
            Destroy(tappedChessPiece);
        }
           
        //
        cpsc.SetFirstPawnMoveFalse();
        //Move reference chess piece to this position
        cpsc.SetXBoard(matrixX);
        cpsc.SetXBoard(matrixX);
        cpsc.SetYBoard(matrixY);
        cpsc.SetCoords();
        


        //Update the matrix
        gameManager.GetComponent<GameManager>().SetPosition(tappedChessPiece);

        //Switch Current Player
        gameManager.GetComponent<GameManager>().NextTurn();

        //Destroy the move plates including self
        tappedChessPiece.GetComponent<ChessPiece>().DestroyMovableTiles();
    }
   
    public void SetCoords(int x , int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetTappedChessPieceTile(GameObject obj)
    {
        tappedChessPiece = obj;
    }

    public GameObject GetTappedChessPieceTile()
    {
        return tappedChessPiece;
    }
}
