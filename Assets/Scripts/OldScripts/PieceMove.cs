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

    //폰은 처음에는 두칸을 움직일 수 있다.
    //킹과 룩은 캐슬링을 위해서 필요하다.
    private bool isFirstMove = true;
    // Start is called before the first frame update
    void Start()
    {
        /**
        //false 로 구성된 bool 배열임 여기다가 공격 가능 범위를 저장할 거임.
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

            // 만일, 나의 위치와 목적지의 위치의 거리가 0.1m 이내라면...
            if (Vector3.Distance(transform.position, destination) < speed * Time.deltaTime)
            {
                // 나의 위치를 목적지의 위치로 갱신한다.
                transform.position = destination;
                isArrive = true;
            }
        }

    }
    /**
    public void Move(int x, int y)
    {

        //만약 이동범위내에 이동할 수 있는 곳이라면
        if (moveRange[x-1,y-1])
        {
            if (BoardManager.isEnemyPieceExist(x, y, team))
            {
                Capture(x, y);
            }
            
            //BoardManager 에 내 정보를 등록해야 된다
            BoardManager.SetPieceInBoard(this.gameObject, xPos, yPos,x,y);
            
            //킹이면 위치 변경을 등록한다.
            if (moveType == MoveType.King)
                BoardManager.SetKingPosition(x, y, team);

            //만약 전에 체크 상황이였으면
            //체크 상황 해제 한다.
            if (BoardManager.IsCheckState(team))
            {
                BoardManager.DeleteCheckPieces();
            }
            
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
    public void Capture(int x, int y)
    {
        BoardManager.CapturePiece(x, y, team);
    }
    public void SetRange()
    {
        //먼저 attackrange와 moverange를 초기화 시킨다.
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
  
        //만약 내 공격 범위에 킹이 있는지 체크 해야 된다.
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
                //갈려는 곳에 같은 팀 있으면 안됨
                //갈려는 곳이 공격에 있으면 안됨
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
        //왼쪽 위로 갈 수 있는 거리
        int left_up_maxDistance = xPos-1<8-yPos?xPos-1:8-yPos;
        //오른쪽 위로 갈 수 있는 거리
        int right_up_maxDistance = 8-xPos<8-yPos?8-xPos:8-yPos;
        //왼 아래
        int left_down_maxDistance = xPos-1<yPos-1?xPos-1:yPos-1;
        //오른족 아래
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
        //만약 보드 안에 있고 우리 팀 기물이 없으면 ㄱㅊㄱㅊ
        //나중에 꼭 구현하기!!
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

        //앞으로 갈 때 앞에 기물이 없고, 1과 8사이의 값이면 갈 수 있다.
        if(yPos + direction <=8 && yPos + direction >= 1)
        {
            if (!BoardManager.isPieceExist(xPos, yPos + direction))
            {
                moveRange[xPos - 1, yPos - 1 + direction] = true;
            }
        }

        //만약 왼쪽 대각선과 오른쪽 대각선이 범위 내라면 공격범위와 이동범위에 추가한다.
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
