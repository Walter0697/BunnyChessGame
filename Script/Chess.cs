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
        if (gameManage.inGame && onBoard)
        {
            if (gameManage.checkMove(type, index))
            {
                gameManage.moveChess(index);
            }
        }
    }
}
