using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour {

    //store the game object for chesses and blocks so that they can move around
    public Chess[] rabbits;      //(-5, 1, -0.7)
    public Chess[] penguins;     //(-4, 1, -0.85)
    public Block[] blocks;

    //to store the reference of the location for the chess
    [HideInInspector] public Chess[] chesses;

    //store the current state of the game
    [HideInInspector] public int diceValue;
    [HideInInspector] public string turn;
    [HideInInspector] public bool canDice;

    // Use this for initialization
    void Start() {
        //this is unnecessary, add this because I messed up the direction in the beginning
        Vector3 gravity = Physics.gravity;
        gravity.y = 0;
        gravity.z = 9.8f;
        Physics.gravity = gravity;

        chesses = new Chess[30];

        setupGame();
    }

    // Update is called once per frame
    void Update() {

    }

    public void changeTurn()
    {
        if (turn == "rabbit") turn = "penguin";
        else                  turn = "rabbit";
    }

    public bool checkMove(string type, int chess_index)
    {
        //if it is not your turn, then you can't move
        if (type != turn)
        {
            Debug.Log("Not your turn, sorry");
            return false;
        }
        //if you haven't rolled the dice, then you can't move
        if (canDice)
        {
            Debug.Log("You still haven't diced!");
            return false;
        }
        //don't know if it goals or just can't cross yet
        if (chess_index + diceValue > chesses.Length)
        {
            Debug.Log("go over the board");
            return false;
        }
        //if you have another chess here, then you can't move
        else if (chesses[chess_index + diceValue] != null && chesses[chess_index + diceValue].type == type)
        {
            Debug.Log("You got a chess over there");
            return false;
        }

        //if two blocks are there, then you can't swipe
        //but ignore that it is in the corner
        //let's just check the rule later
        //but if so just check the target index is 10, 20 or not
        if (chess_index + diceValue + 1 < chesses.Length)
        {
            if (chesses[chess_index + diceValue] != null && chesses[chess_index + diceValue + 1] != null)
            {
                if (chesses[chess_index + diceValue].type != type && chesses[chess_index + diceValue + 1].type != type)
                {
                    Debug.Log("Can't swipe with two chesses together!");
                    return false;
                }
            }
            else if (chesses[chess_index + diceValue] != null && chesses[chess_index + diceValue - 1] != null)
            {
                if (chesses[chess_index + diceValue].type != type && chesses[chess_index + diceValue - 1].type != type)
                {
                    Debug.Log("Can't swipe with two chesses together sorry!");
                    return false;
                }
            }
        }


        //if three blocks or more are there, then you can't pass
        int duplcated_count = 0;
        string current_chess = "";
        for (int i = chess_index; i < chess_index + diceValue; i++)
        {
            
        }

        //if the enemies are stepping on special shit, then again you can't swipe

        //check if there is any restriction in the game that prevent you from moving by index
        return true;
    }

    public void moveChess(int chess_index)
    {
  
        int target_index = chess_index + diceValue;
        
        //if it is over 30, just fucking leave
        if (chesses[chess_index + diceValue] == null)
            moveToBlock(chesses[chess_index], target_index);
        else if (chesses[chess_index + diceValue].type != chesses[chess_index].type)
            swipeToBlock(chesses[chess_index], target_index);
        else
            moveToBlock(chesses[chess_index], target_index);
        canDice = true;
        changeTurn();
    }

    //move the block to a certain position
    public void moveToBlock(Chess chess, int block_index)
    {
        Vector3 target_block = blocks[block_index].transform.position;
        chess.targetPosition = getTargetPosition(target_block, chess.type);

        //set up index information for all parties
        if (chess.index != -1)  chesses[chess.index] = null;
        chesses[block_index] = chess;
        chess.index = block_index;
    }

    //swipe the block instead of simply moving
    public void swipeToBlock(Chess chess, int block_index)
    {
        Vector3 target_block = blocks[block_index].transform.position;
        Vector3 my_block = blocks[chess.index].transform.position;

        chesses[block_index].targetPosition = getTargetPosition(my_block, chesses[block_index].type);
        chess.targetPosition = getTargetPosition(target_block, chess.type);

        int originIndex = chess.index;
        chesses[originIndex] = chesses[block_index];
        chesses[block_index] = chess;

        chesses[originIndex].index = originIndex;
        chesses[block_index].index = block_index;
    }

    public Vector3 getTargetPosition(Vector3 target_block, string type)
    {
        if (type == "rabbit")
            return new Vector3(target_block.x, target_block.y, -0.7f);
        else
            return new Vector3(target_block.x, target_block.y, -0.85f);
    }

    //for starting or restarting the game
    public void setupGame()
    {
        for (int i = 0; i < chesses.Length; i++) chesses[i] = null;
        //reset all the rotations for the chess
        for (int i = 0; i < rabbits.Length; i++) resetChess(rabbits[i]);
        for (int i = 0; i < penguins.Length; i++) resetChess(penguins[i]);
        //for setting all chesses into the gameboard
        for (int i = 0; i < rabbits.Length * 2; i += 2) moveToBlock(rabbits[i / 2], i);
        for (int i = 1; i < penguins.Length * 2; i += 2) moveToBlock(penguins[i / 2], i);

        turn = "rabbit";
        canDice = true;
        diceValue = 0;
    }

    //simply reset the rotation for the chess
    public static void resetChess(Chess chess)
    {
        chess.transform.eulerAngles = new Vector3(-270, 0, 180);
    }
}
