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
	}

    public void rollDice()
    {
        if (gameManage.canDice)
        {
            diceValue = Random.Range(1, 7);
            uiText.text = "DICE VALUE : " + diceValue.ToString();
            gameManage.canDice = false;
            gameManage.diceValue = diceValue;
        }
        else
        {
        }
    }
}
