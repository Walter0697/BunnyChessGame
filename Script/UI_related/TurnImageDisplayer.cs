using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnImageDisplayer : MonoBehaviour {

    public Sprite rabbit_image;
    public Sprite penguin_image;

    public GameManage gameManage;

    private Image ui_sprite;

    void Awake()
    {
        ui_sprite = GetComponent<Image>();    
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameManage.turn == "rabbit") ui_sprite.sprite = rabbit_image;
        else ui_sprite.sprite = penguin_image;
            
	}
}
