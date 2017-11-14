using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour {

	void Start () {
        Text myTxt = GetComponent<Text>();
        myTxt.text = "Final Score: " + ScoreKeeper.score.ToString();
        ScoreKeeper.Reset();
    }
	
}
