using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();
    private NetworkManager networkManager;

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent ;

	private void Start()
	{
        networkManager = NetworkManager.singleton;

        if(networkManager.matchMaker == null){
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
	}

    public void RefreshRoomList(){
        ClearRoomList();

        if(networkManager.matchMaker == null){
            networkManager.StartMatchMaker();
        }
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData)
    {
        status.text = "";

        if (!success || responseData == null)
        {
            status.text = "Couldn`t get matches";
            return;
        }

        foreach (MatchInfoSnapshot match in responseData)
        {
            GameObject _roomListItemGameObject = Instantiate(roomListItemPrefab);
            _roomListItemGameObject.transform.SetParent(roomListParent);

            RoomListItem roomListItem = _roomListItemGameObject.GetComponent<RoomListItem>();
            if(roomListItem != null){
                roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGameObject);
        }

        if(roomList.Count == 0){
            status.text = "No rooms at the moment";
        }

    }

    void ClearRoomList(){
        for (int i = 0; i < roomList.Count; i++){
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match){
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin(){
        ClearRoomList();

        int countDown = 5;
        while(countDown > 0){
            status.text = "JOINING.... (" + countDown + ")";

            yield return new WaitForSeconds(1);

            countDown--;
        }

        // Failed to connect
        status.text = "Failed to connect";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = networkManager.matchInfo;
        if(matchInfo != null){
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }

        RefreshRoomList();
    }
}
