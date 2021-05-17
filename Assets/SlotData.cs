using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotData : MonoBehaviour
{
    public GameObject currentScreen;
    public GameObject infoScreen;

    public Item item;
    public bool isUnlocked;

    public SlotData(Item i, bool isUnlocked)
    {
        this.item = i;
        this.isUnlocked = isUnlocked;
    }

    public void onCheckInfo()
    {
        infoScreen.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.image;
        infoScreen.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = item.name;
        infoScreen.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = item.rarity.image;
        infoScreen.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = item.rarity.name;
        infoScreen.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = item.description;
        infoScreen.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Max Hp:" + item.maxHp;
        infoScreen.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "Speed:" + item.speed;
        infoScreen.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "Attack:" + item.attack;
        infoScreen.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = "Armor:" + item.armor;
        infoScreen.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = item.getTags();
        infoScreen.transform.GetChild(9).GetComponent<Button>().gameObject.SetActive(item.isConsumable);

        currentScreen.SetActive(false);
        infoScreen.SetActive(true);
    }
}