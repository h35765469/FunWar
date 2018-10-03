using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEquipment : MonoBehaviour {

    public float rotateSpeed;
    public float destroyTime = 15f;

	// Use this for initialization
	void Start () {
        //Destroy(gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
        //旋轉自己
        this.GetComponent<Transform>().Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
	}
}
