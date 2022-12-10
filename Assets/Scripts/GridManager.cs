using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height; //both equal to 8

    [SerializeField] private Tile tile;

    [SerializeField] private Transform cam; //we want to change the position of camera after setting up the board

    [SerializeField] private GameObject chessBoard;
    private void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for(int x=0; x<width;x++)
        {
            for(int y=0; y< height; y++)
            {
                var spawnedTile = Instantiate(tile, new Vector3(x, y), Quaternion.identity, chessBoard.transform); //instantiating tiles 
                spawnedTile.name = $"Tile {x} {y}"; //naming tile prefabs
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0); // x is even amd y is not even
                spawnedTile.Init(isOffset);
                

            }

        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -30);
    }
}
