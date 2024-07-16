using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PieceMove;

public class BoardManager : MonoBehaviour
{

    private static GameObject[,] board;
    // Start is called before the first frame update
    void Start()
    {
        board = new GameObject[8, 8];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void SetPieceInBoard(GameObject piece,int x, int y)
    {
        board[x - 1, y - 1] = piece;
    }

    public static bool isEnemyPieceExist(int x, int y, PieceMove.Team team)
    {
        if (board[x - 1, y - 1] == null)
            return false;
        else if (board[x - 1, y - 1].GetComponent<PieceMove>().team == team)
            return false;
        else
            return true;
    }
    public static bool isAllyPieceExist(int x, int y,PieceMove.Team team)
    {
        if (board[x - 1, y - 1] == null)
            return false;
        else if (board[x - 1, y - 1].GetComponent<PieceMove>().team != team)
            return false;
        else
            return true;
    }

    public static bool isPieceExist(int x, int y)
    {
        if (board[x - 1, y - 1] == null)
            return false;
        else
            return true;
    }
    
}
