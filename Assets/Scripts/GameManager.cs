using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public InputManager InputManager;

    //InputManager�� ���� �޴� ���� ���õ� �⹰
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
        //InputManager�κ��� ���� �Է°��� �޾ƿ´�.
        try
        {
            PieceClickEventArgs pieceClickEventArgs = e as PieceClickEventArgs;

            print("������ ��ü: " + pieceClickEventArgs.hittedPositionX + ", " + pieceClickEventArgs.hittedPositionY);
            print("���� ��ġ: " + pieceClickEventArgs.boardX + ", " + pieceClickEventArgs.boardY);
        }
        catch (Exception)
        {
            throw;
        }
        //MoveManager���� �̵��� �� �ִ� ������ ����� (���� üũ ���µ� �����־�� �ϳ�?)
        //�̵� �����ϴٸ�
            //MoveManager���� �⹰�� �̵���Ű��� �Ѵ�.
            //CheckManager���� ���� check����, checkmate���� Ȯ�� ��Ų��.
                //check�̸�, check���¸� Ȯ��
                //chekcmate�̸�, ������ ������.
        //�̵��� �Ұ����ϴٸ�
            //�� �����Ѵ�.
    }

    //MoveManager�� ��� �̵��������� �Ǵ��ϴ°�
    //�ϴ� ���� ������ ������ �־�� �Ѵ�. ������ �迭�� ������ ������ �ִ� ���� �ùٸ� ���ٹ��� �� ����.
    //GameManager���� ������ ��ǥ�� �޴´�. �׷��� ������ �迭���� ������ ��ǥ�� �ִ� �⹰���� �����Ѵ�.
    //�� �⹰�� ������ �˾ƾ� �Ѵ�. GetComponent�� ���� �ͺ��ٴ�, �׳� piece��� ��ü�� ����� �� ��ü�� ���� �����س�����?
    //�׷� ������ ����� ���� ������ ��ü�� ��� �����Ű��?
    //�׷� piece��� ��ü�� GameObject�� Piece��� ������Ʈ�� �ּڰ��� �޾Ƴ���.
    //�� ������ �⹰�� �����Ѵ���, �����س��� ������ ���Ϳ� ���� ������ �� �ִ� ���� �ľ��غ���.
    //ŷ�� ������ ���ݹ����� �̵����� ���Ѵ�.
    //���� �ɸ� �⹰�� �������� ���Ѵ�.
}
