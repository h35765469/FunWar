using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetUp))]
public class Player : NetworkBehaviour {

    [SerializeField]
    public GameObject pointToFall;

    private Vector3 fallPosition;
    private float fallRadius = 1f; 

    [SyncVar]
    private bool _isDead = false;
    private bool isDead
    {
        get { return _isDead; }
        set { _isDead = value; }
    }

    [SyncVar]
    private int currentHealth;

    public float GetHealthPct(){
        return (float)currentHealth / maxHealth;
    }

    [SyncVar]
    public string username = "Loading....";
    [SyncVar]
    public int kills;
    [SyncVar]
    public int deaths;
    [SyncVar]
    public int points = 0;

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public void Setup()
	{
        if(isLocalPlayer){
            //Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetUp>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
	}

    [Command]
    private void CmdBroadCastNewPlayerSetup(){
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(firstSetup){
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }


	[ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourceID){

        if(_isDead){
            return;
        }
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has" + currentHealth + "health");

        if(currentHealth <= 0){
            Die(_sourceID);
        }
    }

    [ClientRpc]
    public void RpcAddPoint(int point){
        points += point;
        Debug.Log(transform.name + " now points " + points); 
    }

    [ClientRpc]
    public void RpcActivateStopTime(){
        
    }

    private void Die(string _sourceID){
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if(sourcePlayer != null){
            sourcePlayer.kills++;
            sourcePlayer.points++; //增加點數當你殺死敵人
        }

        /*掉落點數、裝備*/
        CmdFallPoints();
       

        GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        deaths++;//增加死亡數
        points = 0;//死亡後分數歸0

        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++){
            disableOnDeath[i].enabled = false;
        }

        //Diable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        //Disable thie collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        //spawn a death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        //Switch cameras
        if(isLocalPlayer){
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetUp>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + "is dead");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(){
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint .position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        Setup();

        Debug.Log(transform.name +  " respawn");
    }

    public void SetDefaults(){
        isDead = false;

        currentHealth = maxHealth;

        //enable the component
        for (int i = 0; i < disableOnDeath.Length; i++){
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //set GameObject active
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider _col = GetComponent<Collider>();
        if(_col != null){
            _col.enabled = true;
        }

        //create spawn effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

    [Command]
    private void CmdFallPoints(){
        //RpcFallPoints();

        for (int j = 0; j < points; j++){
            float xDir = Random.Range(-.5f, .5f);
            float zDir = Random.Range(-.5f, .5f);
            float x = 0;
            float z = 0;

            for (int i = 0; i < Random.Range(1, 10); i++)
            {
                x += xDir;
                z += zDir;
            }

            Vector3 dropLocation = new Vector3(x, 0, z);
            GameObject questionMarkPrefab = Resources.Load("Modals/QuestionMark/QuestionMark") as GameObject;
            GameObject dropped = Instantiate(questionMarkPrefab, dropLocation, Quaternion.identity);
            NetworkServer.Spawn(dropped);
        }
    }

    [Command]
    private void CmdFallEquipments(){
        
    }

    //   private void Update()
    //   {
    //       if (!isLocalPlayer){
    //           return;
    //       }

    //       if(Input.GetKeyDown(KeyCode.K)){
    //           RpcTakeDamage(9999);
    //       }
    //}

    //[ClientRpc]
    //private void RpcFallPoints(){
    //    GameObject questionMarkPrefab = Resources.Load("Modals/QuestionMark/QuestionMark") as GameObject;
    //    for (int i = 0; i < points; i++)
    //    {
    //        fallPosition = transform.position + Random.insideUnitSphere * fallRadius;
    //        //Instantiate(pointToFall, fallPosition, Quaternion.identity);
    //        //Instantiate(questionMarkPrefab, fallPosition, Quaternion.identity);
    //        GameObject questionMark = Instantiate(questionMarkPrefab, fallPosition, Quaternion.identity);
    //        Rigidbody rigidbody = questionMark.GetComponent<Rigidbody>();
    //        rigidbody.AddForce(transform.forward * 5f);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //       switch (collision.gameObject.tag){
    //           case "Point":
    //               Debug.Log("You get point");
    //               Destroy(collision.gameObject);
    //               points += 1;
    //               break;
    //           case "QuestionMark":
    //               Debug.Log("You get QuestionMark");
    //               Destroy(collision.gameObject);
    //               GetRandomWeapon();
    //               break;
    //       }

    //}
}
