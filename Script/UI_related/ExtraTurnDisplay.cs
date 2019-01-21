using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraTurnDisplay : MonoBehaviour {

    public GameManage gameManage;
    public Sprite extraturn;
    public Sprite nullsprite;

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
        if (gameManage.extraTurn) uiImage.sprite = extraturn;
        else uiImage.sprite = nullsprite;
	}
}
