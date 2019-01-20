using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour {

    public int diceValue;
    private Text uiText;

    public GameManage gameManage;

    void Awake()
    {
        uiText = GetComponent<Text>();    
    }

    // Use this for initialization
    void Start () {
        diceValue = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameManage.diceValue != 0)
            uiText.text = "DICE VALUE : " + gameManage.diceValue.ToString();
        else
            uiText.text = "DICE VALUE : ?";
    }

    public void rollDice()
    {
        if (gameManage.canDice)
        {
            diceValue = Random.Range(1, 7);
            gameManage.canDice = false;
            gameManage.diceValue = diceValue;
            gameManage.checkAllPossibleMove();
        }
    }
}
