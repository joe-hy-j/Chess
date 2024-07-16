using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelected : MonoBehaviour
{
    private bool isSelected = false;

    private Color originalColor;
    public void Selected()
    {
        isSelected = true;

    }
    public void Deselected()
    {
        isSelected = false; 
    }

    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            //선택되었으면 색을 바꾼다.
            renderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            //선택되었지 않으면 원래 색으로 바꾼다.
            renderer.material.SetColor("_Color", originalColor);
        }
    }
}
