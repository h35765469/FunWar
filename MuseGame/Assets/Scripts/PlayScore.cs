using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayScore : MonoBehaviour {

    int lastKills = 0;
    int lastDeaths = 0;

    Player player;

	private void Start()
	{
        player = GetComponent<Player>();
        StartCoroutine(SyncScoreLoop());
	}

	private void OnDestroy()
	{
        if(player != null){
            SyncNow();
        }
	}

	IEnumerator SyncScoreLoop(){
        while(true){
            yield return new WaitForSeconds(5f);

            SyncNow();
        }
    }

    void SyncNow(){
        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.GetData(OnDataReceived);
        }
    }

    void OnDataReceived(string data){

        if(player.kills <= lastKills && player.deaths <= lastDeaths){
            return;
        }
        int killSinceLast = player.kills - lastKills;
        int deathsSinceLast = player.deaths - lastDeaths;

        if (killSinceLast == 0 && deathsSinceLast == 0)
        {
            return;
        }

        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);

        int newKills = killSinceLast + kills;
        int newDeaths = deathsSinceLast + deaths;

        string newData = DataTranslator.ValuesToData(newKills, newDeaths);

        Debug.Log("Syncing: " + newData);

        lastKills = player.kills;
        lastDeaths = player.deaths;

        UserAccountManager.instance.SendData(newData);
    }
}
