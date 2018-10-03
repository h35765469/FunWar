using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    RectTransform healthBarFill;

    [SerializeField]
    Text AmmoTxt;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreBoard;

    private Player player;
    private PlayerController controller;
    private WeaponManager weaponManager;

    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;

    public void SetPlayer(Player _player){
        player = _player;
        controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

	private void Start()
	{
        PauseMenu.IsOn = false;
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	private void Update()
	{
        SetFuelAmount(controller.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealthPct());
        SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets);

        if(Input.GetKeyDown(KeyCode.Escape)){
            TogglePauseMenu();
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            scoreBoard.SetActive(true);
        }else if(Input.GetKeyUp(KeyCode.Tab)){
            scoreBoard.SetActive(false);
        }
	}

    public void TogglePauseMenu(){
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }

	void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetHealthAmount(float _amount){
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetAmmoAmount(int _amount){
        AmmoTxt.text = _amount.ToString();
    }

    void UpdateUI(){
        for (int i = 0; i < slots.Length; i++){
            if(i < inventory.items.Count){
                slots[i].AddEquipment(inventory.items[i]);
            }else{
                slots[i].ClearSlot();
            }
        }
    }
}
