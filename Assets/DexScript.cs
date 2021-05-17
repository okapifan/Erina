using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DexScript : MonoBehaviour
{
    public GameObject DexScreen;
    public GameObject mainScreen;

    public GameObject slotPrefab;
    public GameObject slotHolder;

    public void createInventorySlots()
    {
        this.clearSlots();
        foreach (Item Item in DataHandler.items) {
            GameObject newSlot = Instantiate(this.slotPrefab) as GameObject;
            newSlot.SetActive(true);
            
            newSlot.GetComponent<SlotData>().isUnlocked = DataHandler.playerItems.Find(x => x.item.Equals(Item)) != null ? true : false;

            GameObject image = newSlot.transform.GetChild(0).gameObject;
            GameObject rarity = newSlot.transform.GetChild(1).gameObject;

            image.GetComponent<Image>().sprite = Item.image;
            rarity.GetComponent<Image>().sprite = Item.rarity.image;

            newSlot.transform.SetParent(slotHolder.transform, false);
        }
        unhideCollected();
    }

    public void clearSlots()
    {
        Component[] slotsToDestroy = slotHolder.GetComponentsInChildren(typeof(Button));
        foreach(Component child in slotsToDestroy)
        {
            Destroy(child.gameObject);
        }
    }

    public void returnButton()
    {
        mainScreen.SetActive(true);
        DexScreen.SetActive(false);
    }

    public void unhideCollected()
    {
        int totalSlots = this.slotHolder.transform.childCount;
        for (int i = 0; i < totalSlots; i++)
        {
            if (this.slotHolder.transform.GetChild(i).GetComponent<SlotData>().isUnlocked)
            {
                this.slotHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
            }
        }
    }
}
