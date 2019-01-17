using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnDisplayer : MonoBehaviour {

    public GameManage gameManage;

    private Text uiText;

    void Awake()
    {
        uiText = GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        uiText.text = gameManage.turn + "'s turn";
	}
}
