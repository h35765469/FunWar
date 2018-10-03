using UnityEngine;
using System.Collections;

public class Api : MonoBehaviour {
    public static string baseUrl = "http://1.34.63.239/";
    public static string videoSampleUrl = "http://1.34.63.239/video_sample/";
    public static string videoContentUrl = "http://1.34.63.239/video_content/";


	private WWWForm Initialization(){
        WWWForm wWWForm = new WWWForm();
        wWWForm.AddField("", "");
        return wWWForm;
    }

    public static IEnumerator PutData(string url, WWWForm wWWForm, System.Action<bool> callBack){
        WWW www = new WWW(url, wWWForm);
        yield return www;

        Debug.Log(www.text);
    }

    public static IEnumerator GetData(string url, WWWForm wWWForm, System.Action<bool> callBack){
        WWW www = new WWW(url, wWWForm);
        yield return www;

        Debug.Log(www.text);
    }


}
