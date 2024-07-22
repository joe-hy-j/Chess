using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    White,
    Black
}

public enum pieceType
{
    King,
    Queen,
    Rook,
    Bishop,
    Knight,
    Pawn
}

public class Pieces : MonoBehaviour
{
    //public ������ team�̸��� piece�����̴�.
    public Team team
    {
        get { return team; }
    }
    
    public pieceType pieceType
    {
        get { return pieceType; }
    }

    //�̵� �ӵ�
    public float speed = 3f;

    //���� �̵� �������� üũ
    bool movement = false;
    //�̵� ��ǥ�� ����
    Vector3 destinationTransformPosition;
    //���� ����
    Vector3 dir;

    //�� ���� �迭���� ��ġ�� ����
    int xPosition;
    int yPosition;

    
    public void Start()
    {
        //���� transform.x, transform.z ���� �迭�� x��ǥ,y��ǥ�� ��ȯ�ϰ� �ʹ�.
        // position���� 0.8�� ���ϰ�, 3.5�� ���� ���� �ݿø��ϰ� �ʹ�.

        xPosition = PositionConverter.ConvertToArrayNumber(transform.position.x);
        yPosition = PositionConverter.ConvertToArrayNumber(transform.position.y);
    }

    public void Update()
    {
        //���� ������ ���� �Ǹ�
        if (movement == true)
        {            
            //���� ���� ��ǥ �������� �Ÿ��� �� �� ��������
            if(Vector3.Distance(destinationTransformPosition,transform.position) < 0.1f)
            {
                //�ٷ� ������Ű��
                transform.position = destinationTransformPosition;
                //�������� �����Ѵ�.
                movement = false;
            }
            //p=p0+vt�� �Ἥ ��ġ�� ���ϰ�
            transform.position += speed * dir * Time.deltaTime;
        }
    }


    public void Move(int destinationX, int destinationY) 
    {
        //���� movementrange manager���� ��� �̵� ������ �������� �ִٸ�
        //�������� transform.position���� ���ϰ�
        float destinationTransformX = PositionConverter.ConvertToTransform(destinationX);
        float destinationTransformY = PositionConverter.ConvertToTransform(destinationY);
        //������ Vector�� ���ϰ�
        destinationTransformPosition = new Vector3(destinationTransformX,0,destinationTransformY);
        //���� ���͸� ���ϰ�
        dir = destinationTransformPosition - transform.position;
        dir.Normalize();
        //update���� �̵��� �����Ѵ�.
        movement = true;
        
    }

}
