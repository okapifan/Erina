using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class LocationSpot
{
    public int id;
    public string name;
    public double longtitude;
    public double latitude;
    public int maxDistanceInKM;
    public Item item;
    public int item_amount;

    public LocationSpot(int id, string name, double latitude, double longtitude, int maxDistanceInKM, int item_id, int item_amount)
    {
        this.id = id;
        this.name = name;
        this.longtitude = longtitude;
        this.latitude = latitude;
        this.maxDistanceInKM = maxDistanceInKM;
        this.item = DataHandler.items.Find(x => x.id.Equals(item_id));
        this.item_amount = item_amount;
    }
}

