using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    GameObject selectedPieces;
    // Start is called before the first frame update
    void Start()
    {
        selectedPieces = null;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if(Physics.Raycast( ray, out hit , 17.0f))
        {
            // 만약 말이 눌렀으면
            if ( Input.GetMouseButtonDown(0) && hit.transform != null && hit.transform.gameObject.tag.Equals("Piece"))
            {
                //만약 같은 말을 눌렀으면
                if(hit.transform.gameObject == selectedPieces)
                {
                    //선택해제하자
                    selectedPieces.GetComponent<PieceSelected>()?.Deselected();
                    selectedPieces = null;
                }
                //다른 말을 눌렀으면
                else
                {
                    //이전 선택 상태의 말을 해제하자
                    selectedPieces?.GetComponent<PieceSelected>()?.Deselected();

                    //현재 선택 상태의 말을 정하자
                    selectedPieces = hit.collider.gameObject;

                    //현재 선택 상태의 말을 선택하자.
                    selectedPieces.GetComponent<PieceSelected>()?.Selected();
                }

            }

            // 보드를 눌렀으면
            if (Input.GetMouseButtonDown(0) && hit.transform!=null && hit.collider.tag.Equals("Board"))
            {
                // 말이 선택 상태이고 이동 가능한 곳이라면 
                if (selectedPieces != null)
                {
                    // 보드의 위치를 구해야 한다.
                    //print(hit.point);
                    //print(XLocation.GetPosition(hit.point.x)+" "+YLocation.GetPosition(hit.point.z));
                    // 말을 보드의 위치로 이동시키자.
                    selectedPieces?.GetComponent<PieceMove>().Move(XLocation.GetPosition(hit.point.x), YLocation.GetPosition(hit.point.z));

                    //그 다음에 선택해제하자
                    selectedPieces?.GetComponent<PieceSelected>().Deselected();
                    selectedPieces = null;
                }
            }

        }
    }
}
