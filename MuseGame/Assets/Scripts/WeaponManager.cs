using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    public bool isReloading = false;
    // Use this for initialization
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    [Command]
    public void CmdSwitchWeapon(){
        RpcEquipWeapon();
    }

    [ClientRpc]
    public void RpcEquipWeapon()
    {
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }

        GameObject _weaponIns = (GameObject)Instantiate(currentWeapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);
        //NetworkServer.Spawn(_weaponIns);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.LogError("No weaponGraphics component on the object" + currentWeapon.name);
        }

        if (isLocalPlayer)
        {
            Util.setLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        CmdSwitchWeapon();
    }

    //public void EquipWeapon(PlayerWeapon _weapon)
    //{
    //    currentWeapon = _weapon;
    //    foreach (Transform child in weaponHolder)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
    //    _weaponIns.transform.SetParent(weaponHolder);

    //    currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
    //    if (currentGraphics == null)
    //    {
    //        Debug.LogError("No weaponGraphics component on the object" + _weapon.name);
    //    }

    //    if (isLocalPlayer)
    //    {
    //        Util.setLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
    //    }
    //}

    public void Reload(){
        if(isReloading){
            return;
        }

        StartCoroutine(Reload_Coroutine());
    }

    private IEnumerator Reload_Coroutine(){
        Debug.Log("Reloading...");

        isReloading = true;

        CmdOnReload();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.bullets = currentWeapon.maxBullets;
        isReloading = false;
    }

    [Command]
    void CmdOnReload(){
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload(){
        Animator anim = currentGraphics.GetComponent<Animator>();

        if(anim != null){
            anim.SetTrigger("Reload");
        }
    }


}
