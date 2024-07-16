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
            // ���� ���� ��������
            if ( Input.GetMouseButtonDown(0) && hit.transform != null && hit.transform.gameObject.tag.Equals("Piece"))
            {
                //���� ���� ���� ��������
                if(hit.transform.gameObject == selectedPieces)
                {
                    //������������
                    selectedPieces.GetComponent<PieceSelected>()?.Deselected();
                    selectedPieces = null;
                }
                //�ٸ� ���� ��������
                else
                {
                    //���� ���� ������ ���� ��������
                    selectedPieces?.GetComponent<PieceSelected>()?.Deselected();

                    //���� ���� ������ ���� ������
                    selectedPieces = hit.collider.gameObject;

                    //���� ���� ������ ���� ��������.
                    selectedPieces.GetComponent<PieceSelected>()?.Selected();
                }

            }

            // ���带 ��������
            if (Input.GetMouseButtonDown(0) && hit.transform!=null && hit.collider.tag.Equals("Board"))
            {
                // ���� ���� �����̰� �̵� ������ ���̶�� 
                if (selectedPieces != null)
                {
                    // ������ ��ġ�� ���ؾ� �Ѵ�.
                    //print(hit.point);
                    //print(XLocation.GetPosition(hit.point.x)+" "+YLocation.GetPosition(hit.point.z));
                    // ���� ������ ��ġ�� �̵���Ű��.
                    selectedPieces?.GetComponent<PieceMove>().Move(XLocation.GetPosition(hit.point.x), YLocation.GetPosition(hit.point.z));

                    //�� ������ ������������
                    selectedPieces?.GetComponent<PieceSelected>().Deselected();
                    selectedPieces = null;
                }
            }

        }
    }
}
