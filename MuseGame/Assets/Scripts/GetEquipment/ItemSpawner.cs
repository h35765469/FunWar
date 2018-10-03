using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner: NetworkBehaviour  {

    public GameObject itemPrefab;
    public Transform spawnPoint;

	// Use this for initialization
	void Start () {
        if(!isLocalPlayer){
            return;
        }
	}

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.H))
        {
            CmdSpawnItem();
        }
	}

	[Command]
    void CmdSpawnItem(){
        GameObject gameObject = Instantiate(itemPrefab, spawnPoint);
        NetworkServer.Spawn(gameObject);
        Debug.Log("spawn my item");
    }
}
