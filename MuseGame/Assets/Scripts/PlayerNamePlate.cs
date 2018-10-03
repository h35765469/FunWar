using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour {

    [SerializeField]
    private Text usernameTxt;

    [SerializeField]
    private RectTransform healthBarfill;

    [SerializeField]
    private Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        usernameTxt.text = player.username;
        healthBarfill.localScale = new Vector3(player.GetHealthPct(), 1f, 1f);
	}
}
