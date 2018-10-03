using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

    private WeaponManager weaponManager;
    public int selectedWeapon = 0;
    public List<PlayerWeapon> playerWeapons = new List<PlayerWeapon>();


	// Use this for initialization
	void Start () {
		PlayerWeapon baseWeapon = new PlayerWeapon();
		baseWeapon.type = GameConstants.BASE_WEAPON;
        baseWeapon.name = "Mace";
        baseWeapon.damage = 10;
        baseWeapon.range = 0;
        baseWeapon.fireRate = 0;
        baseWeapon.maxBullets = 0;
        baseWeapon.reloadTime = 0;
        baseWeapon.type = GameConstants.BASE_WEAPON;
        GameObject basePrefab = Resources.Load("Prefab/Mace") as GameObject;
        baseWeapon.graphics = basePrefab;

        PlayerWeapon playerWeapon = new PlayerWeapon();
        playerWeapon.name = "Heavy_08";
        playerWeapon.damage = 10;
        playerWeapon.range = 500;
        playerWeapon.fireRate = 20;
        playerWeapon.maxBullets = 10;
        playerWeapon.reloadTime = 2;
        playerWeapon.type = GameConstants.TIME_STOP;
        GameObject prefab = Resources.Load("Prefab/Heavy_08") as GameObject;
        playerWeapon.graphics = prefab;

        PlayerWeapon playerWeapon1 = new PlayerWeapon();
        playerWeapon1.name = "Pistol_01";
        playerWeapon1.damage = 30;
        playerWeapon1.range = 200;
        playerWeapon1.fireRate = 50;
        playerWeapon1.maxBullets = 3;
        playerWeapon1.reloadTime = 2;
        //playerWeapon1.type = GameConstants.PHONE_GRENADE;
        GameObject prefab1 = Resources.Load("Prefab/Pistol_01") as GameObject;
        playerWeapon1.graphics = prefab1;

        PlayerWeapon playerWeapon2 = new PlayerWeapon();
        playerWeapon2.name = "Sniper_04";
        playerWeapon2.damage = 15;
        playerWeapon2.range = 1000;
        playerWeapon2.fireRate = 40;
        playerWeapon2.maxBullets = 5;
        playerWeapon2.reloadTime = 3;
        playerWeapon2.type = GameConstants.PHONE_GRENADE;
        GameObject prefab2 = Resources.Load("Prefab/Sci-Fi Automatic") as GameObject;
        playerWeapon2.graphics = prefab2;

        playerWeapons.Add(baseWeapon);
        playerWeapons.Add(playerWeapon);
        playerWeapons.Add(playerWeapon1);
        playerWeapons.Add(playerWeapon2);

        weaponManager = GetComponent<WeaponManager>();
        //SelectedWeapon();
	}
	
	// Update is called once per frame
	void Update () {
        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f){
            if(selectedWeapon >= playerWeapons.Count - 1){
                selectedWeapon = 0;
            }else{
                selectedWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = playerWeapons.Count - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if(previousSelectedWeapon != selectedWeapon){
            SelectedWeapon();
        }
	}

    
    void SelectedWeapon(){
        int i = 0; 
        foreach(PlayerWeapon playerWeapon in playerWeapons){
            if (i == selectedWeapon){
                weaponManager.EquipWeapon(playerWeapon);
            }
            i++;
        }
    }
}
