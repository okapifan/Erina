
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;

public class LocationScript : MonoBehaviour
{
    public Text text;
    public Text closestLocText;
    public Image itemImage;
    public Button collectorsButton;
    public TMP_Text amountText;
    public Canvas game;
    public bool isUpdating;
    public bool blockedUpdating;

    //used for 5 sec timer
    private float timer = 0;
    private float timerMax = 0;

    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject inventoryScreen;
    [SerializeField] GameObject dexScreen;
    [SerializeField] GameObject errorPanel;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Text usernameCurrencyText;

    [SerializeField] GameObject popupPreset;

    private void Start()
    {
        DataHandler.Load();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update() {
        if(!this.isUpdating && !this.blockedUpdating) {
            StartCoroutine(GetLocation());
            this.isUpdating = !this.isUpdating;
        }
    }

    public IEnumerator displayError(string message, int seconds)
    {
        errorPanel.gameObject.SetActive(true);
        errorText.text = message;
        yield return new WaitForSeconds(seconds);
        errorPanel.gameObject.SetActive(false);
    }

    IEnumerator GetLocation()
    {
        //Get permission for location access
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield return new WaitForSeconds(3);

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            if (DataHandler.locationSpots.Count == 0)
            {
                print("No locations found");
                this.isUpdating = !this.isUpdating;
                yield break;
            } else
            {
                LocationSpot temp = null;
                double tempDis = 0;
                foreach (LocationSpot i in DataHandler.locationSpots)
                {
                    double dis = GetDistance(Input.location.lastData.longitude, Input.location.lastData.latitude, i.longtitude, i.latitude);
                    if (temp != null)
                    {
                        if (dis < tempDis)
                        {
                            temp = i;
                            tempDis = dis;
                        }
                    }
                    else { temp = i; tempDis = dis; }

                }
                // Access granted and location value could be retrieved
                DataHandler.currentLocation = temp;
                DataHandler.currentLocationDistance = tempDis;
                this.text.enabled = true;
                this.text.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude;
                this.itemImage.GetComponent<Image>().sprite = temp.item.image;
                this.closestLocText.enabled = true;
                this.closestLocText.text = temp.name + ": " + System.Math.Round(tempDis) + "M";

                this.amountText.enabled = false;
                if (temp.item_amount > 1)
                {
                    this.amountText.enabled = true;
                    this.amountText.text = "x" + temp.item_amount;
                }

                this.collectorsButton.interactable = true;
                this.usernameCurrencyText.text = DataHandler.player.username + " - " + DataHandler.player.currency + "G";
            }
        }

        // Stop service if there is no need to query location updates continuously

        yield return new WaitForSeconds(15);
        this.isUpdating = !this.isUpdating;
    }

    // =========================================================================================================================== //
    // =========================================================================================================================== //
    // =========================================================================================================================== //
    // =========================================================================================================================== //
    // =========================================================================================================================== //

    // ****************************************************************************** //
    // This is the function to call to calculate the distance between two points      //
    // ****************************************************************************** //

    public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
    {
        var d1 = latitude * (System.Math.PI / 180.0);
        var num1 = longitude * (System.Math.PI / 180.0);
        var d2 = otherLatitude * (System.Math.PI / 180.0);
        var num2 = otherLongitude * (System.Math.PI / 180.0) - num1;
        var d3 = System.Math.Pow(System.Math.Sin((d2 - d1) / 2.0), 2.0) + System.Math.Cos(d1) * System.Math.Cos(d2) * System.Math.Pow(System.Math.Sin(num2 / 2.0), 2.0);

        return 6376500.0 * (2.0 * System.Math.Atan2(System.Math.Sqrt(d3), System.Math.Sqrt(1.0 - d3)));
    }

    // =========================================================================================================================== //
    // =========================================================================================================================== //
    // =========================================================================================================================== //
    // =========================================================================================================================== //
    // =========================================================================================================================== //

    public void openInventory()
    {
        mainScreen.SetActive(false);
        inventoryScreen.SetActive(true);
        inventoryScreen.GetComponent<InventoryScript>().createInventorySlots();
    }

    public void openDex()
    {
        mainScreen.SetActive(false);
        dexScreen.SetActive(true);
        dexScreen.GetComponent<DexScript>().createInventorySlots();
    }

    public void collectItemByButton()
    {
        if (DataHandler.currentLocationDistance <= (DataHandler.currentLocation.maxDistanceInKM * 1000))
        {
            DataHandler.reloadCollectedLocationList();
            if (DataHandler.save.collectedLocationsList.Find(x => x.location.Equals(DataHandler.currentLocation.id)) == null)
            {
                StartCoroutine(InventoryScript.CollectItem(DataHandler.currentLocation.item, DataHandler.currentLocation.item_amount)); 
                DataHandler.addToCollectedLocationList(DataHandler.currentLocation);
                //Todo create pop-up
            }
            else
            {
                StartCoroutine(displayError("You need to wait untill " + DataHandler.save.collectedLocationsList.Find(x => x.location.Equals(DataHandler.currentLocation.id)).expiredTime + " before you can collect this item.", 3));
            }
            
        } else
        {
            StartCoroutine(displayError("You need to be in a distance of max " + DataHandler.currentLocation.maxDistanceInKM + " to claim this spot.", 3));
        }
    }
}