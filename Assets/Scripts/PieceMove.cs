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

    //���� ó������ ��ĭ�� ������ �� �ִ�.
    //ŷ�� ���� ĳ������ ���ؼ� �ʿ��ϴ�.
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

            // ����, ���� ��ġ�� �������� ��ġ�� �Ÿ��� 0.1m �̳����...
            if (Vector3.Distance(transform.position, destination) < speed * Time.deltaTime)
            {
                // ���� ��ġ�� �������� ��ġ�� �����Ѵ�.
                transform.position = destination;
                isArrive = true;
            }
        }

    }

    public void Move(int x, int y)
    {

        //���� �̵��������� �̵��� �� �ִ� ���̶��
        if (CheckMoveRange(x, y))
        {
            //�������� ��´�
            destination = new Vector3(XLocation.GetLocation(x), 0, YLocation.GetLocation(y));

            //��� ����!
            isArrive = false; 
            //���� position�� �����Ѵ�.
            xPos = x;
            yPos = y;

            //���� ù ���� �������̿��ٸ�, isFirstMove �� false �� �ٲ�����!
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

        //�밢�� �Ÿ��� ���� ��
        if ((xDistance == yDistance))
        {
            int xDirection = (x - xPos)/xDistance;
            int yDirection = (y - yPos)/yDistance;
            //���� ���� �Ÿ��� ����/���� �ִٸ�
            for(int i =1; i<xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i*xDirection, yPos + i*yDirection))
                {
                    return false;
                }
            }
            //���� ���� ������ �� ģ���� �ִٸ�
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //�¿� �̵��ϴ�
        else if (xDistance != 0 && yDistance == 0)
        {
            int xDirection = (x - xPos) / xDistance;
            //���� ���� �Ÿ��� ����/���� �ִٸ�
            for (int i = 1; i < xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i * xDirection, yPos))
                {
                    return false;
                }
            }
            //���� ���� ������ �� ģ���� �ִٸ�
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //���� �̵��϶�
        else if (xDistance == 0 && yDistance != 0)
        {
            int yDirection = (y - yPos) / yDistance;
            //���� ���� �Ÿ��� ����/���� �ִٸ�
            for (int i = 1; i < yDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos, yPos + i * yDirection))
                {
                    return false;
                }
            }
            //���� ���� ������ �� ģ���� �ִٸ�
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //�� �ƴҶ�
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
            //���� ���� �Ÿ��� ����/���� �ִٸ�
            for (int i = 1; i < xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i * xDirection, yPos))
                {
                    return false;
                }
            }
            //���� ���� ������ �� ģ���� �ִٸ�
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //���� �̵��϶�
        else if (xDistance == 0 && yDistance != 0)
        {
            int yDirection = (y - yPos) / yDistance;
            //���� ���� �Ÿ��� ����/���� �ִٸ�
            for (int i = 1; i < yDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos, yPos + i * yDirection))
                {
                    return false;
                }
            }
            //���� ���� ������ �� ģ���� �ִٸ�
            if (BoardManager.isAllyPieceExist(x, y, team))
                return false;
            return true;
        }
        //�� �ƴҶ�
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

        //�밢�� �Ÿ��� ���� ��
        if ((xDistance == yDistance))
        {
            int xDirection = (x - xPos) / xDistance;
            int yDirection = (y - yPos) / yDistance;
            //���� ���� �Ÿ��� ����/���� �ִٸ�
            for (int i = 1; i < xDistance; i++)
            {
                if (BoardManager.isPieceExist(xPos + i * xDirection, yPos + i * yDirection))
                {
                    return false;
                }
            }
            //���� ���� ������ �� ģ���� �ִٸ�
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
