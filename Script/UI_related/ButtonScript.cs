﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void hideButton()
    {
        gameObject.SetActive(false);
    }

    public void showButton()
    {
        gameObject.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
