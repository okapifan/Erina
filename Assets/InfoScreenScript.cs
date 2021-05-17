using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScreenScript : MonoBehaviour
{
    public GameObject currentScreen;
    public GameObject inventoryScreen;

    public void returnButton()
    {
        currentScreen.SetActive(false);
        inventoryScreen.SetActive(true);
    }
}
