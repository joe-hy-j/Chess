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

    //���� pieces
    GameObject hittedPiece;
    public GameObject selectedObject
    {
        get=> hittedPiece;
    }
    //���� �⹰�� ��ġ
    int hittedObjectX;
    int hittedObjectY;

    //���� ������ ��ġ
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
            //���� ���𰡰� ������ ��
            if(hit.collider.gameObject != null && Input.GetMouseButtonDown(0))
            {
                //���� ���� ���� �⹰�� ���Ǵٸ�
                if (hit.collider.gameObject.tag.Contains(GameManager.gm.CurrentTurn.ToString()))
                {
                    //���� ������Ʈ�� �����ϰ�
                    hittedPiece = hit.collider.gameObject;
                    //���� ��ġ�� arrayNumber�� ��������
                    hittedObjectX = PositionConverter.ConvertToArrayNumber(hit.collider.transform.position.x);
                    hittedObjectY = PositionConverter.ConvertToArrayNumber(hit.collider.transform.position.z);
                }
                //���� ���尡 �����ų� ����� �⹰�� ���ȴٸ�
                else
                {
                    //���� �� ���� ���õ� obj �� ������
                    if (hittedPiece != null)
                    {
                        //���� ������ ��ġ�� �����Ѵ�.
                        boardPositionX = PositionConverter.ConvertToArrayNumber(hit.point.x);
                        boardPositionY = PositionConverter.ConvertToArrayNumber(hit.point.z);
                        //���� �����Ѵ�.
                        if (PieceClick != null) {
                            PieceClickEventArgs pieceClickEventArgs = new PieceClickEventArgs(hittedObjectX, hittedObjectY, boardPositionX, boardPositionY);
                            PieceClick(this, pieceClickEventArgs);
                        }
                    }
                    //���õ� �⹰�� ���ش�
                    hittedPiece = null;
                }
            }
        }
    }
}
