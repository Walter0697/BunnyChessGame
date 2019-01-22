using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour {

    //store the game object for chesses and blocks so that they can move around
    public Chess[] rabbits;      //(-5, 1, -0.7)
    public Chess[] penguins;     //(-4, 1, -0.85)
    public Block[] blocks;

    public Transform[] winPosition; //store the position they should be in when they left the board
    private int winRabbit, winPenguin;

    //to store the reference of the location for the chess
    [HideInInspector] public Chess[] chesses;

    //store the current state of the game
    [HideInInspector] public bool inGame;           //check if it is in game, or in menu
    [HideInInspector] public bool canDice;          //check if you diced
    
    //variable for extra features
    [HideInInspector] public bool forward;          //check NO MOVE: if no chess can move forward, then you have to move backward
    [HideInInspector] public bool firstMove;        //check FIRST MOVE: only 10th chess can move

    [HideInInspector] public bool multiplayer;      //if it is not multiplayer, then you are playing with AI
    public GameObject ai_enemy;

    [HideInInspector] public string turn;           //rabbit or penguin

    //dice: movement for the character
    [HideInInspector] public int diceValue;
    [HideInInspector] public bool extraTurn;

    public ButtonScript inGameCanvas;
    public ButtonScript menuCanvas;
    [HideInInspector] public string message;
    [HideInInspector] public string winning_message;

    // Use this for initialization
    void Start() {
        //this is unnecessary, add this because I messed up the direction in the beginning
        Vector3 gravity = Physics.gravity;
        gravity.y = 0;
        gravity.z = 9.8f;
        Physics.gravity = gravity;

        chesses = new Chess[30];
        inGame = false;
        inGameCanvas.hideButton();
        message = "";
        winning_message = "BunnySenet";
        firstMove = false;
        extraTurn = false;
        multiplayer = false;

        ai_enemy.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
    }

    public void changeTurn()
    {
        if (turn == "rabbit") turn = "penguin";
        else                  turn = "rabbit";
    }

    //if there are no possible move for the chess, then you will have to move backward
    public void checkAllPossibleMove()
    {
        forward = true;
        bool backward = true;
        if (turn == "rabbit")
        {
            for (int i = 0; i < rabbits.Length; i++)
            {
                if (rabbits[i].onBoard)
                {
                    if (checkMove(turn, rabbits[i].index, false))
                    {
                        backward = false;
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < penguins.Length; i++)
            {
                if (penguins[i].onBoard)
                {
                    if (checkMove(turn, penguins[i].index, false))
                    {
                        backward = false;
                        break;
                    }
                }
            }
        }
        forward = !backward;
        if (!forward) message = "NO MOVE: You have to move backward since you have no possible moves";
    }

    //check if it is a valid move
    public bool checkMove(string type, int chess_index, bool check_single)
    {
        int target_index;
        if (forward) target_index = chess_index + diceValue;
        else target_index = chess_index - diceValue;

        //if it is not your turn, then you can't move
        if (type != turn)
        {
            if (check_single) message = "it isn't your turn yet!";
            return false;
        }
        //if you haven't rolled the dice, then you can't move
        if (canDice)
        {
            if (check_single) message = "you haven't diced!";
            return false;
        }
        //don't know if it goals or just can't cross yet
        if (target_index >= chesses.Length || target_index <= 0)
        {
            if (check_single) message = "that will go over board";
            return false;
        }
        //if you have another chess here, then you can't move
        else if (chesses[target_index] != null && chesses[target_index].type == type)
        {
            if (check_single) message = "you got another chess of yours here";
            return false;
        }

        if (!forward)
        {
            if (chesses[target_index] != null)
            {
                if (check_single) message = "there is a block right here";
                return false;
            }
        }
        else
        {
            //if two blocks are there, then you can't swipe
            //but ignore that it is in the corner
            //let's just check the rule later
            //but if so just check the target index is 10, 20 or not
            if (chess_index + diceValue + 1 < chesses.Length)
            {
                if (chesses[target_index] != null && chesses[target_index + 1] != null)
                {
                    if (chesses[target_index].type != type && chesses[target_index + 1].type != type)
                    {
                        if (check_single) message = "DEFENSE: Can't swipe with two chesses together!";
                        return false;
                    }
                }
                else if (chesses[target_index] != null && chesses[target_index - 1] != null)
                {
                    if (chesses[target_index].type != type && chesses[target_index - 1].type != type)
                    {
                        if (check_single) message = "DEFENSE: Can't swipe with two chesses together!";
                        return false;
                    }
                }
            }

            //if three blocks or more are there, then you can't pass
            int duplicated_count = 0;
            for (int i = chess_index; i < target_index; i++)
            {
                if (i == 9 || i == 10 || i == 19 || i == 20) break;

                Debug.Log(i);
                if (chesses[i] == null) duplicated_count = 0;
                else if (chesses[i].type != type) duplicated_count++;
                else if (chesses[i].type == type) duplicated_count = 0;

                if (duplicated_count == 3)
                {
                    if (check_single) message = "BLOCKADE: Can't go across three chesses";
                    return false;
                }
            }
            if (chesses[target_index] != null)
            {
                if (chesses[target_index].type != type)
                {
                    //special block should be in 15, 26, 28, 29 but it should decrease by one since it starts from 0
                    if (target_index == 14 || target_index == 25 || target_index == 27 || target_index == 28)
                    {
                        if (check_single) message = "SAFETY: this block cannot be attacked";
                        return false;
                    }
                }
            }
        }

        //check if there is any restriction in the game that prevent you from moving by index
        return true;
    }

    public void moveChess(int chess_index, bool forward_move)
    {
        message = "";
        int target_index;
        if (forward_move) target_index = chess_index + diceValue;
        else target_index = chess_index - diceValue;

        //if player steps in 27, it will return to 15 
        if (target_index == 26)
        {
            target_index = 14;
            while (chesses[target_index] != null) target_index--;
            message = "TRAP: land on this block will teleport you to other block";
        }

        //if it is over 30, just fucking leave
        if (chesses[target_index] == null)
            moveToBlock(chesses[chess_index], target_index);
        else if (chesses[target_index].type != chesses[chess_index].type)
            swipeToBlock(chesses[chess_index], target_index);
        else
            moveToBlock(chesses[chess_index], target_index);
        canDice = true;

        if (target_index == 29)
            finishChess();

        //check if you won the game
        if (winRabbit == 5 || winPenguin == 5)
        {
            if (winRabbit == 5) winning_message = "RABBIT WINS!";
            else winning_message = "PENGUIN WINS!";
            endGame();
        }
        else
        {
            diceValue = 0;
            if (!extraTurn)
            {
                changeTurn();
            }
        }
    }

    //remove the chess one it is in 30
    public void finishChess()
    {
        //to be honest this will be checked in last function but just to make sure here 
        if (chesses[29] != null)
        {
            chesses[29].onBoard = false;
            chesses[29].index = -1;
            chesses[29].transform.rotation = winPosition[winPenguin + winRabbit].rotation;
            chesses[29].targetPosition = winPosition[winPenguin + winRabbit].position;
            if (chesses[29].type == "rabbit")
                winRabbit++;
            else
                winPenguin++;
            chesses[29] = null;
        }
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

    

    /// 
    /// SETTING UP OR ENDING GAME BELOW
    /// 

    //for starting or restarting the game
    public void setupGame(bool multi)
    {
        for (int i = 0; i < chesses.Length; i++) chesses[i] = null;
        //reset all the rotations for the chess
        for (int i = 0; i < rabbits.Length; i++) resetChess(rabbits[i]);
        for (int i = 0; i < penguins.Length; i++) resetChess(penguins[i]);
        //for setting all chesses into the gameboard
        for (int i = 0; i < rabbits.Length * 2; i += 2) moveToBlock(penguins[i / 2], i);
        for (int i = 1; i < penguins.Length * 2; i += 2) moveToBlock(rabbits[i / 2], i);

        inGame = true;
        turn = "rabbit";
        canDice = true;
        diceValue = 0;
        message = "Game Start";
        forward = true;
        firstMove = true;
        multiplayer = multi;
        ai_enemy.SetActive(!multi);

        winRabbit = 0;
        winPenguin = 0;

        inGameCanvas.showButton();
        menuCanvas.hideButton();
    }

    //when the game ends, call this function
    public void endGame()
    {
        inGame = false;
        inGameCanvas.hideButton();
        menuCanvas.showButton();
    }

    //simply reset the rotation for the chess
    public static void resetChess(Chess chess)
    {
        chess.onBoard = true;
        chess.transform.eulerAngles = new Vector3(-270, 0, 180);
    }

    // get the height for the type of the chess
    public Vector3 getTargetPosition(Vector3 target_block, string type)
    {
        if (type == "rabbit")
            return new Vector3(target_block.x, target_block.y, -0.7f);
        else
            return new Vector3(target_block.x, target_block.y, -0.85f);
    }
}
