using UnityEngine;

public class PhoneGrenade : MonoBehaviour {

    public float delay = 3f;
    public float radius = 5f;
    public float force = 700;
    public int damage = 30;

    public GameObject explosionEffect;

    public float countDown;
    public bool hasExploded = false;



	// Use this for initialization
	void Start () {
        countDown = delay;
	}
	
	// Update is called once per frame
	void Update () {
        countDown -= Time.deltaTime;
        if(countDown <= 0 && !hasExploded){
            Explore();
            hasExploded = true;
        }
	}

    void Explore(){
        //Show effect
        Instantiate(explosionEffect, transform.position, transform.rotation);


        //Get nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider nearbyObject in colliders){
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null){
                rb.AddExplosionForce(force, transform.position, radius);
            }

            if(nearbyObject.tag == GameConstants.PLAYER_TAG){
                Debug.Log(nearbyObject.name + " has been Boomed.");

                Player _player = GameManager.GetPlayer(nearbyObject.name);
                _player.RpcTakeDamage(damage, transform.name);
            }
        }
        //add force
        //Damage


        //remove phone grenade
        Destroy(gameObject);
            
    }

}
