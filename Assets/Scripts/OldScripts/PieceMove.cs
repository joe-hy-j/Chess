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

    private bool [,] attackRange;
    private bool [,] moveRange;
    public bool GetMoveRange(int x, int y)
    {
        return moveRange[x-1, y-1];
    }
    public bool GetAttackRange(int x, int y)
    {
        if(moveType == MoveType.Pawn)
        {
            return attackRange[x-1, y-1];
        }
        else
        {
            return GetMoveRange(x, y);
        }
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
        /**
        //false �� ������ bool �迭�� ����ٰ� ���� ���� ������ ������ ����.
        attackRange = new bool[8, 8];
        moveRange = new bool[8, 8];

        destination = transform.position;
        isArrive = true;

        BoardManager.SetPieceInBoard(this.gameObject, xPos, yPos);
        SetRange();

        if (moveType == MoveType.King)
            BoardManager.SetKingPosition(xPos, yPos, team);
        **/
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
    /**
    public void Move(int x, int y)
    {

        //���� �̵��������� �̵��� �� �ִ� ���̶��
        if (moveRange[x-1,y-1])
        {
            if (BoardManager.isEnemyPieceExist(x, y, team))
            {
                Capture(x, y);
            }
            
            //BoardManager �� �� ������ ����ؾ� �ȴ�
            BoardManager.SetPieceInBoard(this.gameObject, xPos, yPos,x,y);
            
            //ŷ�̸� ��ġ ������ ����Ѵ�.
            if (moveType == MoveType.King)
                BoardManager.SetKingPosition(x, y, team);

            //���� ���� üũ ��Ȳ�̿�����
            //üũ ��Ȳ ���� �Ѵ�.
            if (BoardManager.IsCheckState(team))
            {
                BoardManager.DeleteCheckPieces();
            }
            
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
    public void Capture(int x, int y)
    {
        BoardManager.CapturePiece(x, y, team);
    }
    public void SetRange()
    {
        //���� attackrange�� moverange�� �ʱ�ȭ ��Ų��.
        for(int i =0; i<attackRange.GetLength(0); i++)
        {
            for(int j =0; j<attackRange.GetLength(1); j++)
            {
                attackRange[i, j] = false;
            }
        }

        for(int i =0;i<moveRange.GetLength(0); i++)
        {
            for(int j =0;j<moveRange.GetLength(1);j++)
            {
                moveRange[i, j] = false;
            }
        }

        switch (moveType)
        {
            case MoveType.King:
                SetKingRange();
                break;
            case MoveType.Queen:
                SetQueenRange();
                break;
            case MoveType.Rook:
                SetRookRange();
                break;
            case MoveType.Bishop:
                SetBishopRange();
                break;
            case MoveType.Knight:
                break;
            case MoveType.Pawn:
                SetPawnRange();
                break;
        }
        if (BoardManager.IsCheckState(team))
        {
            switch (moveType)
            {
                case MoveType.King:
                    break;
                case MoveType.Pawn:
                    for(int i =0; i<moveRange.GetLength(0);i++)
                        for(int j =0; j<moveRange.GetLength(1); j++)
                            moveRange[i, j] =  attackRange[i, j] && BoardManager.checkedTeamMoveInfo[i, j];
                    break;
                default:
                    for (int i = 0; i < moveRange.GetLength(0); i++)
                        for (int j = 0; j < moveRange.GetLength(1); j++)
                            moveRange[i, j] = moveRange[i, j] && BoardManager.checkedTeamMoveInfo[i, j];
                    break;

            }
        }
  
        //���� �� ���� ������ ŷ�� �ִ��� üũ �ؾ� �ȴ�.
        int index = team == Team.White ? 1 : 0;
        int kingxPos = BoardManager.kingPosition[index].x;
        int kingyPos = BoardManager.kingPosition[index].y;

        //print("Team" + team+kingxPos+ ", "+ kingyPos);
        if (moveType == MoveType.Pawn && attackRange[kingxPos - 1, kingyPos - 1]==true)
        {
            print("Check!");
            BoardManager.AddCheckPieces(this.gameObject,team);
        }
        else if (moveRange[kingxPos - 1, kingyPos - 1]==true)
        {
            print("check!");
            BoardManager.AddCheckPieces(this.gameObject,team);
        }
    }


    private void SetKingRange()
    {
        bool isLeftCorner = xPos == 1 ? true : false;
        bool isRightCorner = xPos ==8 ? true : false;
        bool isUpCorner = yPos == 8 ? true : false;
        bool isDownCorner = yPos == 1 ? true : false;

        List<int> checkX = new List<int>();
        List<int> checkY = new List<int>();

        if (!isLeftCorner)
        {
            checkX.Add(-1);         
        }
        if (!isRightCorner)
        {
            checkX.Add(1);
        }
        checkX.Add(0);


        if (!isUpCorner)
        {
            checkY.Add(1);
        }
        if (!isDownCorner)
        {
            checkY.Add(-1);
        }
        checkY.Add(0);
        BoardManager.PrintAttackRange();
        foreach (int xDistance in checkX)
        {
            foreach (int yDistance in checkY)
            {
                if (xDistance == 0 && yDistance == 0)
                    continue;
                //������ ���� ���� �� ������ �ȵ�
                //������ ���� ���ݿ� ������ �ȵ�
                if(!BoardManager.isAllyPieceExist(xPos+xDistance, yPos+yDistance,team) && !BoardManager.GetAttackRange(xPos + xDistance, yPos + yDistance, team))
                {
                    if(team == Team.White)
                        print("xDistance: " + xDistance + " yDistance" + yDistance);
                    moveRange[xPos+xDistance-1,yPos+yDistance-1] = true;
                }
            }
        }
    }
    private void SetQueenRange()
    {
        SetBishopRange();
        SetRookRange();
    }

    private void SetBishopRange()
    {
        //���� ���� �� �� �ִ� �Ÿ�
        int left_up_maxDistance = xPos-1<8-yPos?xPos-1:8-yPos;
        //������ ���� �� �� �ִ� �Ÿ�
        int right_up_maxDistance = 8-xPos<8-yPos?8-xPos:8-yPos;
        //�� �Ʒ�
        int left_down_maxDistance = xPos-1<yPos-1?xPos-1:yPos-1;
        //������ �Ʒ�
        int right_down_maxDistance = 8-xPos<yPos-1?8-xPos:yPos-1;

        for(int i =1; i<=left_up_maxDistance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos - i, yPos + i, team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos - i, yPos + i, team))
            {
                attackRange[xPos - i-1, yPos + i-1] = true;
                moveRange[xPos-i-1, yPos + i-1] = true;
                break;
            }
            else
            {
                moveRange[xPos - i - 1, yPos + i - 1] = true;
            }
        }

        for (int i = 1; i <= left_down_maxDistance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos - i, yPos - i, team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos - i, yPos - i, team))
            {
                attackRange[xPos - i - 1, yPos - i - 1] = true;
                moveRange[xPos - i - 1, yPos - i - 1] = true;
                break;
            }
            else
            {
                moveRange[xPos - i - 1, yPos - i - 1] = true;
            }
        }

        for (int i = 1; i <= right_up_maxDistance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos + i, yPos + i, team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos + i, yPos + i, team))
            {
                attackRange[xPos + i - 1, yPos + i - 1] = true;
                moveRange[xPos + i - 1, yPos + i - 1] = true;
                break;
            }
            else
            {
                moveRange[xPos + i - 1, yPos + i - 1] = true;
            }
        }

        for (int i = 1; i <= right_down_maxDistance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos + i, yPos - i, team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos + i, yPos - i, team))
            {
                attackRange[xPos + i - 1, yPos - i - 1] = true;
                moveRange[xPos + i - 1, yPos - i - 1] = true;
                break;
            }
            else
            {
                moveRange[xPos + i - 1, yPos - i - 1] = true;
            }
        }
    }

    private void SetRookRange()
    {
        int left_max_distance = xPos - 1;
        int right_max_distance = 8 - xPos;
        int up_max_distance = 8 - yPos;
        int down_max_distance = yPos - 1;

        for(int i =1; i<=left_max_distance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos-i,yPos,team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos - i, yPos, team))
            {
                attackRange[xPos - i - 1, yPos - 1] = true;
                moveRange[xPos - i - 1, yPos - 1] = true;
                break;
            }
            else
            {
                moveRange[xPos - i - 1, yPos - 1] = true;
            }
        }
        for (int i = 1; i <= right_max_distance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos + i, yPos, team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos + i, yPos, team))
            {
                attackRange[xPos + i - 1, yPos - 1] = true;
                moveRange[xPos + i - 1, yPos - 1] = true;
                break;
            }
            else
            {
                moveRange[xPos + i - 1, yPos - 1] = true;
            }
        }

        for (int i = 1; i <= up_max_distance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos , yPos +i, team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos, yPos+i, team))
            {
                attackRange[xPos - 1, yPos +i - 1] = true;
                moveRange[xPos - 1, yPos + i - 1] = true;
                break;
            }
            else
            {
                moveRange[xPos - 1, yPos + i - 1] = true;
            }
        }

        for (int i = 1; i <= down_max_distance; i++)
        {
            if (BoardManager.isAllyPieceExist(xPos, yPos - i, team))
            {
                break;
            }
            else if (BoardManager.isEnemyPieceExist(xPos, yPos - i, team))
            {
                attackRange[xPos - 1, yPos - i - 1] = true;
                moveRange[xPos - 1, yPos - i - 1] = true;
                break;
            }
            else
            {
                moveRange[xPos - 1, yPos - i - 1] = true;
            }
        }
    }

    private void SetKnightRange()
    {
        //���� ���� �ȿ� �ְ� �츮 �� �⹰�� ������ ��������
        //���߿� �� �����ϱ�!!
    }
    private void SetPawnRange()
    {
        int direction = 1;
        if (team == Team.Black)
            direction = -1;

        if (isFirstMove)
        {
            if(!BoardManager.isPieceExist(xPos,yPos+2*direction))
                moveRange[xPos - 1, yPos + direction*1] = true;
        }

        //������ �� �� �տ� �⹰�� ����, 1�� 8������ ���̸� �� �� �ִ�.
        if(yPos + direction <=8 && yPos + direction >= 1)
        {
            if (!BoardManager.isPieceExist(xPos, yPos + direction))
            {
                moveRange[xPos - 1, yPos - 1 + direction] = true;
            }
        }

        //���� ���� �밢���� ������ �밢���� ���� ����� ���ݹ����� �̵������� �߰��Ѵ�.
        if(xPos-1 >=1 && yPos+direction >=1 && yPos+direction <= 8)
        {
            if (BoardManager.isEnemyPieceExist(xPos - 1, yPos + direction, team))
            {
                moveRange[xPos - 2, yPos + direction - 1] = true;
            }
            attackRange[xPos - 2, yPos + direction - 1] = true;
        }

        if (xPos + 1 >= 1 && yPos + direction >= 1 && yPos + direction <= 8)
        {
            if (BoardManager.isEnemyPieceExist(xPos + 1, yPos + direction, team))
            {
                moveRange[xPos , yPos + direction - 1] = true;
            }
            attackRange[xPos , yPos + direction - 1] = true;
        }



    }

    private void AttackPieces(int x, int y)
    {
        BoardManager.CapturePiece(x, y, team);
    }
    **/
}
