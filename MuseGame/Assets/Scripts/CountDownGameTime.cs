using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownGameTime : MonoBehaviour {

    int timer = 900;
    [SerializeField]
    private Text gameTimeTxt;

    public float[] timeLimits;
    public float timeLeft;

    [SerializeField]
    bool counting = true;

    int stage = 0;

	// Use this for initialization
	void Start () {
        //StartCoroutine("CountDownTime");
        stage = 0;

        timeLimits[stage] += Time.time;
        timeLeft = timeLimits[stage];
	}
	
	// Update is called once per frame
    void Update () {
	    
        if(counting){
            timeLeft = timeLimits[stage] - Time.time;

            int min = (int)timeLeft / 60;
            int sec = (int)timeLeft % 60;

            gameTimeTxt.text = (min < 10 ? "0" + min : min + "") + ":" + (sec < 10 ? "0" + sec : sec + "");

            if(timeLeft < 0){
                timeLeft = 0;
                counting = false;
                stage++;
            }
        }


    }

    IEnumerator  CountDownTime(){
        while(true){
            timer--;
            if(timer == 0){
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
