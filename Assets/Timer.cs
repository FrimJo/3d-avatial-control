using UnityEngine;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float timerSec = 60;
    public Text timerText;

    private float timerCounter;
    

	// Use this for initialization
	void Start () {
        timerCounter = timerSec;
        timerText = GetComponent<Text>();
        updateText();

    }

    // Update is called once per frame
    void Update()
    {
        timerCounter -= Time.deltaTime; // I need timer which from a particular time goes to zero
        Debug.Log(timerCounter);
        if (timerCounter > 0)
        {
            updateText();
        }

        if (Input.GetKeyDown("t")) // And then i can restart game: pressing restart.
        {
            timerCounter = timerSec;
        }
    }

    private void updateText()
    {
        timerText.text = timerCounter.ToString("F0");
    }
}
