using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour {

    public GameManage gameManage;
    public DiceScript diceScript;

    public Chess[] penguins;        //AI will only control penguin because of the storyline(?)
    private bool thinking;

	// Use this for initialization
	void Start () {
        thinking = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameManage.multiplayer && gameManage.turn == "penguin" && thinking == false)
        {
            thinking = true;
            StartCoroutine(AIPlay());
        }
	}

    IEnumerator AIPlay()
    {
        yield return new WaitForSeconds(1);
        if (gameManage.turn == "penguin")
            diceScript.randomRoll();
        yield return new WaitForSeconds(1);
        gameManage.checkAllPossibleMove();

        int index = -1;
        for (int i = 0; i < penguins.Length; i++)
        {
            if (penguins[i].onBoard &&
                gameManage.checkMove("penguin", penguins[i].index, false))
            {
                index = i;
            }
        }

        if (index != -1) penguins[index].selectChess();

        yield return new WaitForSeconds(1);
        thinking = false;
    }
}
