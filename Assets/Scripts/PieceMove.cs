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

    //���� ó������ ��ĭ�� ������ �� �ִ�.
    //ŷ�� ���� ĳ������ ���ؼ� �ʿ��ϴ�.
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
