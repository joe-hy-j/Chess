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
    //public 변수는 team이름과 piece종류이다.
    public Team team
    {
        get { return team; }
    }
    
    public pieceType pieceType
    {
        get { return pieceType; }
    }

    //이동 속도
    public float speed = 3f;

    //현재 이동 상태인지 체크
    bool movement = false;
    //이동 목표를 설정
    Vector3 destinationTransformPosition;
    //방향 벡터
    Vector3 dir;

    //내 현재 배열에서 위치를 저장
    int xPosition;
    int yPosition;

    
    public void Start()
    {
        //현재 transform.x, transform.z 값을 배열의 x좌표,y좌표로 변환하고 싶다.
        // position값에 0.8을 곱하고, 3.5를 더한 값을 반올림하고 싶다.

        xPosition = PositionConverter.ConvertToArrayNumber(transform.position.x);
        yPosition = PositionConverter.ConvertToArrayNumber(transform.position.y);
    }

    public void Update()
    {
        //만약 움직일 때가 되면
        if (movement == true)
        {            
            //만약 현재 목표 지점까지 거리가 얼마 안 남았으면
            if(Vector3.Distance(destinationTransformPosition,transform.position) < 0.1f)
            {
                //바로 도착시키고
                transform.position = destinationTransformPosition;
                //움직임을 종료한다.
                movement = false;
            }
            //p=p0+vt를 써서 위치를 구하고
            transform.position += speed * dir * Time.deltaTime;
        }
    }


    public void Move(int destinationX, int destinationY) 
    {
        //만약 movementrange manager한테 물어서 이동 가능한 범위내에 있다면
        //목적지의 transform.position값을 구하고
        float destinationTransformX = PositionConverter.ConvertToTransform(destinationX);
        float destinationTransformY = PositionConverter.ConvertToTransform(destinationY);
        //목적지 Vector를 구하고
        destinationTransformPosition = new Vector3(destinationTransformX,0,destinationTransformY);
        //방향 벡터를 구하고
        dir = destinationTransformPosition - transform.position;
        dir.Normalize();
        //update에서 이동을 시작한다.
        movement = true;
        
    }

}
