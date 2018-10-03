using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killfeed : MonoBehaviour {

    [SerializeField]
    GameObject killfeedItemPrefab;

	// Use this for initialization
	void Start () {
        GameManager.instance.onPlayerKilledCallback += OnKill;
	}

    public void OnKill(string player, string source){
        GameObject gameObject = Instantiate(killfeedItemPrefab, this.transform);
        gameObject.GetComponent<KillfeedItem>().Setup(player, source);

        Destroy(gameObject, 4f);
    }
}
