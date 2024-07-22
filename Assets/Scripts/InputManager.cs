using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceClickEventArgs : EventArgs
{
    public int hittedPositionX;
    public int hittedPositionY;


    public int boardX;
    public int boardY;

    public PieceClickEventArgs(int hittedX, int hittedY, int boardX,int boardY)
    {
        this.hittedPositionX = hittedX;
        this.hittedPositionY = hittedY;
        this.boardX = boardX;  
        this.boardY = boardY;
    }
}

public class InputManager : MonoBehaviour
{
    public event EventHandler PieceClick;

    //눌린 pieces
    GameObject hittedPiece;
    public GameObject selectedObject
    {
        get=> hittedPiece;
    }
    //눌린 기물의 위치
    int hittedObjectX;
    int hittedObjectY;

    //눌린 보드의 위치
    int boardPositionX;
    int boardPositionY;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if(Physics.Raycast( ray, out hit , 17.0f))
        {
            //만약 무언가가 눌렸을 때
            if(hit.collider.gameObject != null && Input.GetMouseButtonDown(0))
            {
                //만약 현재 턴의 기물이 눌렷다면
                if (hit.collider.gameObject.tag.Contains(GameManager.gm.CurrentTurn.ToString()))
                {
                    //게임 오브젝트를 저장하고
                    hittedPiece = hit.collider.gameObject;
                    //눌린 위치를 arrayNumber로 저장하자
                    hittedObjectX = PositionConverter.ConvertToArrayNumber(hit.collider.transform.position.x);
                    hittedObjectY = PositionConverter.ConvertToArrayNumber(hit.collider.transform.position.z);
                }
                //만약 보드가 눌리거나 상대편 기물이 눌렸다면
                else
                {
                    //만약 그 전에 선택된 obj 가 있으면
                    if (hittedPiece != null)
                    {
                        //눌린 보드의 위치를 저장한다.
                        boardPositionX = PositionConverter.ConvertToArrayNumber(hit.point.x);
                        boardPositionY = PositionConverter.ConvertToArrayNumber(hit.point.z);
                        //값을 전달한다.
                        if (PieceClick != null) {
                            PieceClickEventArgs pieceClickEventArgs = new PieceClickEventArgs(hittedObjectX, hittedObjectY, boardPositionX, boardPositionY);
                            PieceClick(this, pieceClickEventArgs);
                        }
                    }
                    //선택된 기물을 없앤다
                    hittedPiece = null;
                }
            }
        }
    }
}
