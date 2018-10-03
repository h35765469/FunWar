using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {

    [SerializeField]
    GameObject playerScoreBoardItem;

    [SerializeField]
    Transform playerScoreBoardList;

	private void OnEnable()
	{
        Player[] players = GameManager.GetAllPlayers();
        foreach(Player player in players){
            GameObject itemGo = (GameObject)Instantiate(playerScoreBoardItem, playerScoreBoardList);
            PlayerScoreBoardItem item = itemGo.GetComponent<PlayerScoreBoardItem>();
            if(item != null){
                item.SetUp(player.username, player.kills, player.deaths, player.points);
            }

        }
        //Loop through and set up a list item for each one
            //Setting the ui elements equals to the relevant data
         
	}

	private void OnDisable()
	{
        foreach (Transform child in playerScoreBoardList){
            Destroy(child.gameObject);
        }
	}

}
