using System.Collections;
using System.Collections.Generic;
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


    public int xPos = 5;
    public int yPos=2;

    public float speed = 1f;

    public MoveType moveType = MoveType.King;

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
                if ((xDistance + yDistance <=2) &&(xDistance == 1 || yDistance == 1))
                    return true;
                else
                    return false;
            case MoveType.Queen:
                if ((xDistance == yDistance))
                    return true;
                else if(xDistance != 0 && yDistance == 0)
                    return true;
                else if(xDistance ==0 && yDistance!=0)
                    return true;
                else
                    return false;
            case MoveType.Rook:
                if (xDistance == 0 && yDistance != 0)
                    return true;
                else if (xDistance != 0 && yDistance == 0)
                    return true;
                else
                    return false;
            case MoveType.Bishop:
                if (xDistance == yDistance)
                    return true;
                else
                    return false;
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
}
