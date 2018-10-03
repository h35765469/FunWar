using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask layerMask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

	// Use this for initialization
	void Start () {
        if(camera == null){
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
	}
	
	// Update is called once per frame
	void Update () {
        currentWeapon = weaponManager.GetCurrentWeapon ();

        if(PauseMenu.IsOn){
            return;
        }
        if(currentWeapon.type == GameConstants.BASE_WEAPON){
            UseBaseWeapon();
        }else if(currentWeapon.type == GameConstants.TIME_STOP){
            UseTimeStop();
        }else if(currentWeapon.type == GameConstants.PHONE_GRENADE){
            UsePhoneGrenade();
        }else{
            UseWeapon();
        }


	}

    //Is called on the server when a player shoots 
    [Command]
    void CmdOnShot(){
        RpcDoShootEffect();
    }

    /*Is called on client when we need to do a shoot effect
    */
    [ClientRpc]
    void RpcDoShootEffect(){
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal){
        RpcDoHitEffect(_pos, _normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage, string _sourceID)
    {
        Debug.Log(_playerID + " has been shot.");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }

    [Client]
    void WieldBaseWeapon()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Attack");
    }

    [Client]
    void Shoot(){
        //Debug.Log("Shoot!");
        if(!isLocalPlayer || weaponManager.isReloading){
            return;
        }

        if(currentWeapon.bullets <= 0){
            weaponManager.Reload();
            return;
        }

        currentWeapon.bullets--;
        Debug.Log("Remaining bullets: " + currentWeapon.bullets);

        CmdOnShot();
        RaycastHit _hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out _hit, currentWeapon.range, layerMask))
        {
            Debug.Log("we hit " + _hit.collider.name);

            if(_hit.collider.tag == PLAYER_TAG){
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage, transform.name);
            }

            CmdOnHit(_hit.point, _hit.normal);
        }

        if(currentWeapon.bullets <= 0){
            weaponManager.Reload();
        }
    }

    [Client]
    private void ShootTimeStop(){
        Debug.Log("Shoot time stop");
        if (!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }

        if (currentWeapon.bullets <= 0)
        {
            weaponManager.Reload();
            return;
        }

        currentWeapon.bullets--;
        Debug.Log("Remaining bullets: " + currentWeapon.bullets);


    }

    [Client]
    void ShootPhoneGrenade(){
        Debug.Log("Shoot Phone Grenade!");
        if (!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }

        if (currentWeapon.bullets <= 0)
        {
            weaponManager.Reload();
            return;
        }

        currentWeapon.bullets--;
        Debug.Log("Remaining bullets: " + currentWeapon.bullets);
        CmdShootPhoneGrenade();
    }

    private void UseWeapon(){
        
        if (currentWeapon.bullets < currentWeapon.maxBullets)
        {
            if (Input.GetButtonDown("Reload"))
            {
                weaponManager.Reload();
                return;
            }
        }

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }


    }

    //基本武器
    private void UseBaseWeapon(){
        WieldBaseWeapon();
    }

    //時間暫停
    private void UseTimeStop(){
        ReloadWeapon();

        if (Input.GetButtonDown("Fire1"))
        {
            ShootTimeStop();
        }
    }

    //手機炸彈
    private void UsePhoneGrenade(){
        ReloadWeapon();

        if (Input.GetButtonDown("Fire1"))
        {
            ShootPhoneGrenade();
        }

    }

    //重新裝填武器
    private void ReloadWeapon(){
        if (currentWeapon.bullets < currentWeapon.maxBullets)
        {
            if (Input.GetButtonDown("Reload"))
            {
                weaponManager.Reload();
                return;
            }
        }
    }

    [Command]
    private void CmdShootTimeStop(){
        Player[] allPlayer = GameManager.GetAllPlayers();
        for (int i = 0; i < allPlayer.Length; i++){
            
        }
    }

    [Command]
    private void CmdShootPhoneGrenade(){
        GameObject phoneGrenadePrefab = Resources.Load("Modals/PhoneGrenade/PhoneGrenade") as GameObject;
        GameObject phoneGrenade = Instantiate(phoneGrenadePrefab, transform.position, transform.rotation);
        Rigidbody rigidbody = phoneGrenade.GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * 40f);
        NetworkServer.Spawn(phoneGrenade);
        
    }
}
