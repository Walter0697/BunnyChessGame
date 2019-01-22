using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour {

    //save the chess information
    public string type;
    [HideInInspector] public int index;
    [HideInInspector] public bool onBoard;

    //for moving the chess
    public Vector3 targetPosition;
    Vector3 velocity;
    float smoothTime = 0.25f;
    float smoothDistance = 0.01f;

    private static GameManage gameManage = null;

    void Awake()
    {
        if (gameManage == null) gameManage = GameObject.FindGameObjectWithTag("GameManage").GetComponent<GameManage>();
        index = -1;
        targetPosition = this.transform.position;
    }
    // Use this for initialization
    void Start () {
        onBoard = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(this.transform.position, targetPosition) > smoothDistance)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, smoothTime);
        }
	}

    void OnMouseUp()
    {
        //if you are playing against computer, then you will have to wait
        if (!gameManage.multiplayer && gameManage.turn == "penguin")
        {
            gameManage.message = "not your turn yet, human";
            return;
        }

        selectChess();
    }

    public void selectChess()
    {
        //check if the game started yet
        if (gameManage.inGame && onBoard)
        {
            if (gameManage.forward && gameManage.checkMove(type, index, true))
            {
                if (gameManage.firstMove && index != 9) gameManage.message = "FIRST MOVE: has to move the first chess";
                else
                {
                    if (gameManage.firstMove) gameManage.firstMove = false;
                    gameManage.moveChess(index, true);
                }
            }
            //if no possible move, then you have to move backward
            else if (gameManage.forward == false)
            {
                gameManage.moveChess(index, false);
            }
        }
    }
}
