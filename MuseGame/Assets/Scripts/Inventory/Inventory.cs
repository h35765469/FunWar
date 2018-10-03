using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton

    public static Inventory instance;

	private void Awake()
	{
        if (instance != null){
            Debug.LogWarning("More than one instance of Inventory of found!");
            return;
        }
        instance = this;
	}

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 8;

    public List<Item> items = new List<Item>();


    public bool Add(Item equipment){
        if(!equipment.isDefaultItem){
            if(items.Count >= space){
                Debug.Log("Not enough room");
                return false;
            }
            items.Add(equipment);

            if(onItemChangedCallback != null){
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }

    public void Remove(Item item){
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
