using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightChange : MonoBehaviour {

    private Light light_source;
    private int timer;

	// Use this for initialization
	void Awake () {
        light_source = gameObject.transform.GetComponent<Light>();
	}

    void Start()
    {
        timer = 0;     
    }

    // Update is called once per frame
    void Update () {
        timer++;
        if (timer > 5) //10 should be default
        {
            light_source.transform.rotation = light_source.transform.rotation * Quaternion.Euler(0, 0.05f, 0);
            timer = 0;
        }
     }
}
