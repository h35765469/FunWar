using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private NetworkManager networkManager;

	private void Start()
	{
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null){
            networkManager.StartMatchMaker();
        }
	}

	public void SetRoomName(string _name){
        roomName = _name;
        Debug.Log(roomName);
    }

    public void CreateRoom(){
        if(roomName != "" && roomName != null){
            Debug.Log("create Room " + roomName + "with room for  " + roomSize + " .Players");
            networkManager.matchMaker.CreateMatch(roomName, 4, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}
