using System.Collections;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour {

    public static UserAccountManager instance;

	public void Awake()
	{
        if(instance != null){
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
	}

    public static string loggedIn_Username { get; protected set; } //stores username once logged in
    private string loggedIn_Password = "";//stores password once logged in

    public static bool IsLoggedIn { get; protected set; }

    public string loggedInSceneName = "Lobby";
    public string loggedOutSceneName = "LoginMenu";

    public delegate void OnDataReceivedCallback(string data);


    public void LogOut(){
        loggedIn_Username = "";
        loggedIn_Password = "";

        IsLoggedIn = false;

        Debug.Log("User Logged out");

        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void LogIn (string username, string password)
    {
        loggedIn_Username = username;
        loggedIn_Password = password;

        IsLoggedIn = true;

        Debug.Log("User Logged in " + loggedIn_Username);

        SceneManager.LoadScene(loggedInSceneName);
    }

    public void SendData(string data)
    { //called when the 'Send Data' button on the data part is pressed
        if (IsLoggedIn == true)
        {
            //ready to send request
            StartCoroutine(sendSendDataRequest(loggedIn_Username, loggedIn_Password, data)); //calls function to send: send data request
        }
    }

    IEnumerator sendSendDataRequest(string username, string password, string data)
    {
        IEnumerator eee = DCF.SetUserData(username, password, data);
        while (eee.MoveNext())
        {
            yield return eee.Current;
        }
        string returneddd = eee.Current as string;
        if (returneddd == "ContainsUnsupportedSymbol")
        {
            //One of the parameters contained a - symbol
            Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
        if (returneddd == "Error")
        {
            //Error occurred. For more information of the error, DC.Login could
            //be used with the same username and password
            Debug.Log("Data Upload Error: Contains Unsupported Symbol '-'");
        }
    }

    public void GetData(OnDataReceivedCallback onDataReceived)
    { //called when the 'Get Data' button on the data part is pressed

        if (IsLoggedIn == true)
        {
            //ready to send request
            StartCoroutine(sendGetDataRequest(loggedIn_Username, loggedIn_Password, onDataReceived)); //calls function to send get data request
        }
    }

    IEnumerator sendGetDataRequest(string username, string password, OnDataReceivedCallback onDataReceived)
    {
        string data = "ERROR";

        IEnumerator eeee = DCF.GetUserData(username, password);
        while (eeee.MoveNext())
        {
            yield return eeee.Current;
        }
        string returnedddd = eeee.Current as string;
        if (returnedddd == "Error")
        {
            //Error occurred. For more information of the error, DC.Login could
            //be used with the same username and password
            Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
        else
        {
            if (returnedddd == "ContainsUnsupportedSymbol")
            {
                //One of the parameters contained a - symbol
                Debug.Log("Get Data Error: Contains Unsupported Symbol '-'");
            }
            else
            {
                //Data received in returned.text variable
                string DataRecieved = returnedddd;
                data = DataRecieved;
            }
        }

        if(onDataReceived != null){
            onDataReceived.Invoke(data);
        }
    }
}
