using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

    public GameObject instruction;
    public GameObject backstory;


    // Use this for initialization
    void Start() {
        instruction.SetActive(false);
        backstory.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void openInstruction()
    {
        instruction.SetActive(true);
    }

    public void closeInstruction()
    {
        instruction.SetActive(false);
    }

    public void openBackstory()
    {
        backstory.SetActive(true);
    }

    public void closeBackstory()
    {
        backstory.SetActive(false);
    }
}
