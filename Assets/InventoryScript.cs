using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryScript : MonoBehaviour
{
    private GameObject[] slots;

    public GameObject inventoryScreen;
    public GameObject mainScreen;

    public GameObject slotPrefab;
    public GameObject slotHolder;

    public void createInventorySlots()
    {
        this.clearSlots();
        foreach (PlayerItem playerItem in DataHandler.playerItems) {
            if (playerItem.amount > 0)
            {
                GameObject newSlot = Instantiate(this.slotPrefab) as GameObject;
                newSlot.SetActive(true);

                GameObject image = newSlot.transform.GetChild(0).gameObject;
                GameObject amount = newSlot.transform.GetChild(1).gameObject;
                GameObject rarity = newSlot.transform.GetChild(2).gameObject;

                newSlot.GetComponent<SlotData>().item = playerItem.item;
                image.GetComponent<Image>().sprite = playerItem.item.image;
                rarity.GetComponent<Image>().sprite = playerItem.item.rarity.image;
                amount.GetComponent<Text>().text = playerItem.amount.ToString();

                newSlot.transform.SetParent(slotHolder.transform, false);
            }
        }
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
        inventoryScreen.SetActive(false);
    }

    public void refreshInventory()
    {
        SceneManager.LoadScene(1);
    }

    public static IEnumerator CollectItem(Item i, int amount)
    {
        string url = DataHandler.url + "collectItem.php";
        WWWForm form = new WWWForm();

        PlayerItem y = DataHandler.playerItems.Find(x => x.item.Equals(i));
        if (y != null)
        {
            //Fills up to max if you receive more than max amount
            if ((y.amount + amount) > 10000)
            {
                amount = 10000 - y.amount;
            }

            form.AddField("option", "updateAmount");
            form.AddField("player_id", DataHandler.player.id.ToString());
            form.AddField("item_id", i.id.ToString());
            form.AddField("amount", (y.amount + amount).ToString());

            WWW w = new WWW(url, form);
            yield return w;

            if (w.error != null)
            {
                Debug.Log("<color=red>" + w.text + "</color>"); //error
            }
            else
            {
                if (w.isDone)
                {
                    if (w.text.Contains("error"))
                    {
                        Debug.Log("<color=red>" + w.text + "</color>"); //error
                    }
                    else
                    {
                        Debug.Log("<color=green>" + w.text + "</color>");
                        y.amount += amount;
                    }
                }
            }

            w.Dispose();
        }
        else
        {
            form.AddField("option", "newAmount");
            form.AddField("player_id", DataHandler.player.id.ToString());
            form.AddField("item_id", i.id.ToString());
            form.AddField("amount", amount.ToString());

            WWW w = new WWW(url, form);
            yield return w;

            if (w.error != null)
            {
                Debug.Log("<color=red>" + w.text + "</color>"); //error
            }
            else
            {
                if (w.isDone)
                {
                    if (w.text.Contains("error"))
                    {
                        Debug.Log("<color=red>" + w.text + "</color>"); //error
                    }
                    else
                    {
                        Debug.Log("<color=green>" + w.text + "</color>"); 
                        PlayerItem newItem = new PlayerItem(DataHandler.player.id, i.id, amount);
                        DataHandler.playerItems.Add(newItem);
                    }
                }
            }
            w.Dispose();
        }
    }
}
