using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserAccount_Lobby : MonoBehaviour {

    public Text usernameTxt;

	private void Start()
	{
        if(UserAccountManager.IsLoggedIn){
            usernameTxt.text = UserAccountManager.loggedIn_Username;
        }
	}

    public void LogOut(){
        if(UserAccountManager.IsLoggedIn){
            UserAccountManager.instance.LogOut();
        }
    }
}
