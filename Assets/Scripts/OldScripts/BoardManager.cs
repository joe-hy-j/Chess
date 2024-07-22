using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TestTools;
using static PieceMove;

public class BoardManager : MonoBehaviour
{

    private static GameObject[,] board;

    private static bool[,] whiteAttackRange;
    private static bool[,] blackAttackRange;

    public static bool isWhiteKingChecked = false;
    public static bool isBlackKingChecked = false;

    public static bool[,] checkedTeamMoveInfo;

    private static List<GameObject> checkingPieces = new List<GameObject>();

    public static void AddCheckPieces(GameObject pieces,Team team)
    {
        print("Hello");
        MoveType moveType= pieces.GetComponent<PieceMove>().moveType;

        int piecesX = pieces.GetComponent<PieceMove>().xPos;
        int piecesY = pieces.GetComponent<PieceMove>().yPos;

        int kingX = team == Team.White ? kingPosition[1].x : kingPosition[0].x;
        int kingY = team == Team.Black ? kingPosition[1].y : kingPosition[0].y;

        checkingPieces.Add(pieces);
        if (team == Team.White)
            isBlackKingChecked = true;
        else
            isWhiteKingChecked = true;

        //만약 폰이 check를 걸었으면
        //이동 가능 범위에 폰만 넣으면 됨
        if(moveType == MoveType.Pawn || moveType == MoveType.Knight)
            checkedTeamMoveInfo[piecesX-1,piecesY-1] = true;
        //만약 나이트가 check 걸었으면
        //이동 가능 범위에 나이트만 넣으면 됨

        //만약 룩, 퀸, 비숍이 체크를 걸었으면
        else
        {
            //만약 x값이 같으면
            if (piecesX == kingX)
            {
                int i = piecesY;
                while (i != kingY)
                {
                    checkedTeamMoveInfo[piecesX, i - 1] = true;
                    if (piecesY < kingY)
                        i++;
                    else
                        i--;
                }
            }
            //만약 y값이 같으면
            else if (piecesY == kingY)
            {
                int i = piecesX;
                while (i != kingX)
                {
                    checkedTeamMoveInfo[i - 1, piecesY - 1] = true;
                    if (piecesX < kingX)
                        i++;
                    else
                        i--;
                }
            }
            //만약 대각선이라면
            else
            {
                int xDirection = piecesX > kingX ? -1 : 1;
                int yDirection = piecesY > kingY ? -1 : 1;

                int i = piecesX;
                int j = piecesY;

                while (i != kingX)
                {
                    checkedTeamMoveInfo[i - 1, j - 1] = true;
                    i += xDirection;
                    j += yDirection;
                }

            }
        }

        PrintCheckedInfo();
    }

    private static void PrintCheckedInfo()
    {
        string log = "";
        for (int j = checkedTeamMoveInfo.GetLength(1) - 1; j >= 0; j--)
        {
            for (int i = 0; i < checkedTeamMoveInfo.GetLength(0); i++)
            {
                if (checkedTeamMoveInfo[i, j])
                    log += "o";
                else
                    log += "x";
            }
            log += "\n";
        }
    }
    public static void DeleteCheckPieces()
    {
        checkingPieces.Clear();
        checkedTeamMoveInfo = new bool[8,8];

        isWhiteKingChecked = false;
        isBlackKingChecked = false;
    }

    public struct kingPos
    {
        public int x ;
        public int y;

        public kingPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static kingPos[] kingPosition = new kingPos[2];
    
    public static void SetKingPosition(int x, int y, Team team)
    {
        if(team == Team.White)
        {
            kingPosition[0].x = x;
            kingPosition[0].y = y;
        }
        else
        {
            kingPosition[1].x = x;
            kingPosition[1].y = y;
        }
    }

    public static bool GetAttackRange(int x, int y,Team team)
    {
        if (team == Team.White)
        {
            return blackAttackRange[x-1, y-1]; 
        }
        else
        {
            return whiteAttackRange[x-1, y-1];
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        board = new GameObject[8, 8];
        whiteAttackRange = new bool[8, 8];
        blackAttackRange = new bool[8, 8];
        checkedTeamMoveInfo = new bool[8, 8];

        kingPosition[0] = new kingPos(4,1);
        kingPosition[1] = new kingPos(4,8);

        print("Awake");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void SetPieceInBoard(GameObject piece,int x, int y)
    {
        board[x - 1, y - 1] = piece;
    }

    public static void SetPieceInBoard(GameObject piece, int oriX, int oriY,int x, int y)
    {
        board[oriX - 1, oriY - 1] = null;
        board[x - 1, y - 1] = piece;
    }

    public static void SetAttackRange()
    {
        //먼저 초기화를 시켜준다.
        for (int i = 0; i < whiteAttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < whiteAttackRange.GetLength(1); j++)
            {
                whiteAttackRange[i, j] = false;
            }
        }

        for (int i = 0; i < blackAttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < blackAttackRange.GetLength(1); j++)
            {
                blackAttackRange[i, j] = false;
            }
        }
        /**
        //모든
        for (int i =0; i<board.GetLength(0); i++)
        {
            for (int j =0; j<board.GetLength(1); j++)
            {
                if (board[i, j] != null)
                {
                    PieceMove piece = board[i, j].GetComponent<PieceMove>();
                    if (piece.team == Team.White)
                    {
                        piece.SetRange();
                        for (int k = 0; k < whiteAttackRange.GetLength(0); k++)
                        {
                            for (int l = 0; l < whiteAttackRange.GetLength(1); l++)
                            {
                                if (piece.GetAttackRange(k + 1, l + 1))
                                    whiteAttackRange[k, l] = true;
                            }
                        }
                    }
                    else if (piece.team == Team.Black)
                    {
                        piece.SetRange();
                        for (int k = 0; k < blackAttackRange.GetLength(0); k++)
                        {
                            for (int l = 0; l < blackAttackRange.GetLength(1); l++)
                            {
                                if (piece.GetAttackRange(k + 1, l + 1))
                                    blackAttackRange[k, l] = true;
                            }
                        }
                    }
                }
            }
        }
        PrintAttackRange();
        **/
    }

    public static void PrintAttackRange()
    {
        string log = "white \n";
        for (int j = blackAttackRange.GetLength(1) - 1; j >= 0; j--)
        {
            for (int i = 0; i < whiteAttackRange.GetLength(0); i++)
            {
                if (whiteAttackRange[i, j])
                    log += "o";
                else
                    log += "x";
            }
            log += "\n";
        }
        print(log);
        log = "Black \n";
        for (int j = blackAttackRange.GetLength(1) - 1; j >= 0; j--)
        {
            for (int i = 0; i < whiteAttackRange.GetLength(0); i++)
            {
                if (blackAttackRange[i, j])
                    log += "o";
                else
                    log += "x";
            }
            log += "\n";
        }
        print(log);

    }

    public static bool isEnemyPieceExist(int x, int y, PieceMove.Team team)
    {
        if (board[x - 1, y - 1] == null)
            return false;
        else if (board[x - 1, y - 1].GetComponent<PieceMove>().team == team)
            return false;
        else if (board[x - 1, y - 1].GetComponent<PieceMove>().moveType == MoveType.King)
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
    /**
    public static void CapturePiece(int x, int y,Team team)
    {
        //만약 빈자리가 아니고, 상대방이고, 킹이 아니면(킹은 못 움직이게 구현 되어야 한다)
        if (isEnemyPieceExist(x, y, team))
        {
            //만약 board에 특정 위치에 존재하면
            // 그위치에 존재하는 것을 destroy
            Destroy(board[x - 1, y - 1]);
            // 배열에서도 없앤다
            board[x - 1, y - 1] = null;
        }
    }

    public static bool IsCheckState(Team team)
    {
        if(team == Team.White)
            return isWhiteKingChecked;
        else
            return isBlackKingChecked;


    }
    **/
}
