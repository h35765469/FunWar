using UnityEngine;
using UnityEngine.Networking;

public class ItemPickUp : Interactable {

    public Item item;

	public override void OnCollisionEnter(Collision collision)
	{
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag == "Player")
        {
            CmdPickUp(collision.gameObject.name);
        }
	}

    [Command]
	void CmdPickUp(string playerID){
        Debug.Log("Picking up " + item.name);
        if(item.name == "PhoneGrenade"){
            bool wasPickedUp = Inventory.instance.Add(item);
            if (wasPickedUp)
            {
                NetworkServer.Destroy(gameObject);
            }
        }else if(item.name == "QuestionMark"){
            Player player = GameManager.GetPlayer(playerID);
            player.RpcAddPoint(1);
        }
    }
}
