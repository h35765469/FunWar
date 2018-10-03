using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreBoardItem : MonoBehaviour {

    [SerializeField]
    Text usernameTxt;

    [SerializeField]
    Text killsTxt;

    [SerializeField]
    Text deathsTxt;

    [SerializeField]
    Text pointsTxt;

	
    public void SetUp(string username, int kills, int deaths, int points){
        usernameTxt.text = username;
        killsTxt.text = "Kills: " + kills;
        deathsTxt.text = "Deaths: " + deaths;
        pointsTxt.text = "Points: " + points; 
    }
}
