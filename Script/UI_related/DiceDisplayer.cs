using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceDisplayer : MonoBehaviour {

    public DiceScript diceScript;
    public int index;
    public Sprite whiteSprite;
    public Sprite blackSprite;
    public Sprite unknownSprite;

    private Image uiImage;

    void Awake()
    {
        uiImage = GetComponent<Image>();    
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (diceScript.gameManage.canDice) uiImage.sprite = unknownSprite;
        else if (diceScript.whiteUp[index]) uiImage.sprite = whiteSprite;
        else uiImage.sprite = blackSprite;
	}
}
