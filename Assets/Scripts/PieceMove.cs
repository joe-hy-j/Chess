using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PieceMove : MonoBehaviour
{
    public enum MoveType
    {
        King,
        Queen,
        Rook,
        Knight,
        Bishop,
        Pawn
    }

    public enum Team
    {
        White,
        Black
    }

    public int xPos = 5;
    public int yPos=2;

    public float speed = 1f;

    public MoveType moveType = MoveType.King;
    public Team team = Team.White;

    private Vector3 dir;

    private Vector3 destination;

    private bool isArrive;

    //폰은 처음에는 두칸을 움직일 수 있다.
    //킹과 룩은 캐슬링을 위해서 필요하다.
    private bool isFirstMove = true;
    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        isArrive = true;

        BoardManager.SetPieceInBoard(this.gameObject, xPos, yPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isArrive)
        {
            dir = destination - transform.position;
            dir.Normalize();

            transform.Translate(dir * speed * Time.deltaTime);

            // 만일, 나의 위치와 목적지의 위치의 거리가 0.1m 이내라면...
            if (Vector3.Distance(transform.position, destination) < speed * Time.deltaTime)
            {
                // 나의 위치를 목적지의 위치로 갱신한다.
                transform.position = destination;
                isArrive = true;
            }
        }

    }

    public void Move(int x, int y)
    {

        //만약 이동범위내에 이동할 수 있는 곳이라면
        if (CheckMoveRange(x, y))
        {
            //목적지로 잡는다
            destination = new Vector3(XLocation.GetLocation(x), 0, YLocation.GetLocation(y));

            //출발 시작!
            isArrive = false; 
            //현재 position을 갱신한다.
            xPos = x;
            yPos = y;

            //만약 첫 번재 움직임이였다면, isFirstMove 를 false 로 바꿔주자!
            if (isFirstMove)
            {
                isFirstMove = false;
            }
        }
    }



    private bool CheckMoveRange(int x, int y)
    {
        int xDistance;
        int yDistance;

        xDistance = Mathf.Abs(x - xPos);
        yDistance = Mathf.Abs(y - yPos);

        if(xDistance == 0 && yDistance == 0)
        {
            return false;
        }

        switch (moveType)
        {
            case MoveType.King:
                return CheckKingMove(x,y);
            case MoveType.Queen:
                return CheckQueenMove(x,y);
            case MoveType.Rook:
                return CheckRookMove(x,y);
            case MoveType.Bishop:
                return CheckBishopMove(x,y);
            case MoveType.Knight:
                if (xDistance + yDistance == 3 && (xDistance == 1 || yDistance == 1))
                    return true;
                else
                    return false;
            case MoveType.Pawn:
                if (!isFirstMove && x == xPos && y == yPos + 1)
                {
                    return true;
                }
                else if (isFirstMove && x == xPos && (y == yPos + 2 || y == yPos + 1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
    private bool CheckKingMove(int x, int y)
    {
        int xDistance;
        int yDistance;

        xDistance = Mathf.Abs(x - xPos);
        yDistance = Mathf.Abs(y - yPos);

        if (xDistance == 0 && yDistance == 0)
        {
            return false;
        }

        if ((xDistance + yDistance <= 2) && (xDistance == 1 || yDistance == 1))
        {
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            else
                return true;
        }
        else
            return false;
    }

    private bool CheckQueenMove(int x, int y)
    {
        int xDistance;
        int yDistance;

        xDistance = Mathf.Abs(x - xPos);
        yDistance = Mathf.Abs(y - yPos);

        if (xDistance == 0 && yDistance == 0)
        {
            return false;
        }

        //대각선 거리에 있을 때
        if ((xDistance == yDistance))
        {
            int xDirection = (x - xPos)/xDistance;
            int yDirection = (y - yPos)/yDistance;
            //만약 오는 거리에 상대방/내가 있다면
            for(int i =1; i<xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i*xDirection, yPos + i*yDirection))
                {
                    return false;
                }
            }
            //만약 도착 지점에 내 친구가 있다면
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //좌우 이동일대
        else if (xDistance != 0 && yDistance == 0)
        {
            int xDirection = (x - xPos) / xDistance;
            //만약 오는 거리에 상대방/내가 있다면
            for (int i = 1; i < xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i * xDirection, yPos))
                {
                    return false;
                }
            }
            //만약 도착 지점에 내 친구가 있다면
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //상하 이동일때
        else if (xDistance == 0 && yDistance != 0)
        {
            int yDirection = (y - yPos) / yDistance;
            //만약 오는 거리에 상대방/내가 있다면
            for (int i = 1; i < yDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos, yPos + i * yDirection))
                {
                    return false;
                }
            }
            //만약 도착 지점에 내 친구가 있다면
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //다 아닐때
        else
            return false;
    }

    private bool CheckRookMove(int x, int y)
    {
        int xDistance;
        int yDistance;

        xDistance = Mathf.Abs(x - xPos);
        yDistance = Mathf.Abs(y - yPos);

        if (xDistance == 0 && yDistance == 0)
        {
            return false;
        }

        if (xDistance != 0 && yDistance == 0)
        {
            int xDirection = (x - xPos) / xDistance;
            //만약 오는 거리에 상대방/내가 있다면
            for (int i = 1; i < xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i * xDirection, yPos))
                {
                    return false;
                }
            }
            //만약 도착 지점에 내 친구가 있다면
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //상하 이동일때
        else if (xDistance == 0 && yDistance != 0)
        {
            int yDirection = (y - yPos) / yDistance;
            //만약 오는 거리에 상대방/내가 있다면
            for (int i = 1; i < yDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos, yPos + i * yDirection))
                {
                    return false;
                }
            }
            //만약 도착 지점에 내 친구가 있다면
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //다 아닐때
        else
            return false;
    }

    private bool CheckBishopMove(int x, int y)
    {
        int xDistance;
        int yDistance;

        xDistance = Mathf.Abs(x - xPos);
        yDistance = Mathf.Abs(y - yPos);

        if (xDistance == 0 && yDistance == 0)
        {
            return false;
        }

        //대각선 거리에 있을 때
        if ((xDistance == yDistance))
        {
            int xDirection = (x - xPos) / xDistance;
            int yDirection = (y - yPos) / yDistance;
            //만약 오는 거리에 상대방/내가 있다면
            for (int i = 1; i < xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i * xDirection, yPos + i * yDirection))
                {
                    return false;
                }
            }
            //만약 도착 지점에 내 친구가 있다면
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
