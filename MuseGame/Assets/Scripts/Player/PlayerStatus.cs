using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

    public Text killCountTxt;
    public Text deathCountTxt;

	// Use this for initialization
	void Start () {
        if(UserAccountManager.IsLoggedIn){
            UserAccountManager.instance.GetData(OnReceiveData);
        }
	}

    void OnReceiveData(string data){
        if(killCountTxt == null || deathCountTxt == null){
            return;
        }
        killCountTxt.text = DataTranslator.DataToKills(data).ToString() + " KILLS";
        deathCountTxt.text = DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
    }
	
}
