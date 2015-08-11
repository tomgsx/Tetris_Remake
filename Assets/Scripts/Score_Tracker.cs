using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score_Tracker : MonoBehaviour {

    private Text scoreText;

    public int score;

	// Use this for initialization
	void Awake () {
        scoreText = GameObject.Find("Canvas/ActualScore").GetComponent<Text>();
        score = 0;
        scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Called by Block_Spawner when new block is instantiated
    public void NewBlock()
    {
        score += 1;
        scoreText.text = score.ToString();
    }

    // Called by Row_Scanner when a row is completed
    public void NewRow()
    {
        score += 16;
        scoreText.text = score.ToString();
    }
}
