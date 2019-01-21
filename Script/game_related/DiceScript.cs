using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour {

    [HideInInspector] public bool[] whiteUp;
    [HideInInspector] public int diceValue;
    private Text uiText;

    public GameManage gameManage;

    void Awake()
    {
        uiText = GetComponent<Text>();
    }

    // Use this for initialization
    void Start() {
        diceValue = 0;
        whiteUp = new bool[4];
        for (int i = 0; i < 4; i++) whiteUp[i] = false;
    }

    // Update is called once per frame
    void Update() {
        if (gameManage.diceValue != 0)
            uiText.text = gameManage.diceValue.ToString();
        else
            uiText.text = "?";
    }

    public void rollDice()
    {
        if (gameManage.canDice)
        {
            //true => white
            //false => black
            int totalWhite = 0;
            for (int i = 0; i < 4; i++)
            {
                whiteUp[i] = randomBoolean();
                if (whiteUp[i]) totalWhite++;
            }

            //1 white side up = move 1 square and throw again
            //2 white sides up = move 2 squares
            //3 white sides up = move 3 squares
            //4 white sides up = move 4 squares and throw again
            //4 black sides up = move 6 squares and throw again

            if (totalWhite == 0)
            {
                diceValue = 6;
                gameManage.extraTurn = true;
            }
            else if (totalWhite == 1)
            {
                diceValue = 1;
                gameManage.extraTurn = true;
            }
            else if (totalWhite == 2)
            {
                diceValue = 2;
                gameManage.extraTurn = false;
            }
            else if (totalWhite == 3)
            {
                diceValue = 3;
                gameManage.extraTurn = false;
            }
            else if (totalWhite == 4)
            {
                diceValue = 4;
                gameManage.extraTurn = true;
            }

            gameManage.diceValue = diceValue;
            gameManage.canDice = false;
            gameManage.checkAllPossibleMove();
        }
    }

    public bool randomBoolean()
    {
        return Random.Range(0, 2) == 0;
    }
}
