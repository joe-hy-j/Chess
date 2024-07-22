using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public InputManager InputManager;

    //InputManager로 부터 받는 현재 선택된 기물
    GameObject currentSelectedObject;

    private Team currentTurn;
    public Team CurrentTurn {
        get=>currentTurn;
    }

    private void Awake()
    {
        if (gm == null)
            gm = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        currentTurn = Team.White;
        InputManager.PieceClick += new EventHandler(piece_Click);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void piece_Click(object sender, EventArgs e)
    {
        //InputManager로부터 현재 입력값을 받아온다.
        try
        {
            PieceClickEventArgs pieceClickEventArgs = e as PieceClickEventArgs;

            print("눌러진 객체: " + pieceClickEventArgs.hittedPositionX + ", " + pieceClickEventArgs.hittedPositionY);
            print("보드 위치: " + pieceClickEventArgs.boardX + ", " + pieceClickEventArgs.boardY);
        }
        catch (Exception)
        {
            throw;
        }
        //MoveManager한테 이동할 수 있는 값인지 물어본다 (지금 체크 상태도 보내주어야 하나?)
        //이동 가능하다면
            //MoveManager한테 기물을 이동시키라고 한다.
            //CheckManager한테 지금 check인지, checkmate인지 확인 시킨다.
                //check이면, check상태를 확인
                //chekcmate이면, 게임을 끝낸다.
        //이동이 불가능하다면
            //걍 무시한다.
    }

    //MoveManager는 어떻게 이동가능한지 판단하는가
    //일단 보드 정보를 가지고 있어야 한다. 이차원 배열로 정보를 가지고 있는 것이 올바른 접근법일 거 같다.
    //GameManager한테 보드의 좌표를 받는다. 그러면 이차원 배열에서 보드의 좌표에 있는 기물에게 접근한다.
    //그 기물의 종류를 알아야 한다. GetComponent를 쓰는 것보다는, 그냥 piece라는 객체를 만들고 그 객체에 값을 저장해놓을까?
    //그럼 가상의 보드와 실제 보드의 객체를 어떻게 연결시키지?
    //그럼 piece라는 객체에 GameObject와 Piece라는 컴포넌트의 주솟값을 받아놓자.
    //그 보드의 기물에 접근한다음, 저장해놓은 움직임 벡터에 따라 움직일 수 있는 곳을 파악해보자.
    //킹은 상대방의 공격범위로 이동하지 못한다.
    //핀이 걸린 기물은 움직이지 못한다.
}
