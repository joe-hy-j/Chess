using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    private GameObject[,] board;
    // Start is called before the first frame update
    void Start()
    {
        board = new GameObject[8, 8];
        print(board[0, 0] == null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
