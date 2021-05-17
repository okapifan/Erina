using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public static class DataHandler
{
    public static string url = "http://192.168.1.73/unity/erina/";

    public static PlayerData player;
    public static LocationSpot currentLocation;
    public static double currentLocationDistance;

    public static SaveData save; 
    public static string file = "save.txt";

    public static List<LocationSpot> locationSpots = new List<LocationSpot>();
    public static List<Tag> tags = new List<Tag>();
    public static List<Item> items = new List<Item>();
    public static List<PlayerItem> playerItems = new List<PlayerItem>();
    public static List<Rarity> rarities = new List<Rarity>();

    public static IEnumerator GetLocationSpots()
    {
        DataHandler.locationSpots.Clear();
        string url = DataHandler.url + "getLocations.php";
        WWW w = new WWW(url);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>"); // Error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("<color=red>" + w.text + "</color>"); // Error
                }
                else
                {
                    Debug.Log("<color=green>" + w.text + "</color>"); // Location spots exist
                    LocationSpot[] data = JsonConvert.DeserializeObject<LocationSpot[]>(w.text);

                    DataHandler.locationSpots.AddRange(data);
                }
            }
        }
        w.Dispose();
    }

    public static IEnumerator GetItems() // Unused
    {
        DataHandler.items.Clear();
        string url = DataHandler.url + "getItems.php";
        WWW w = new WWW(url);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>"); // Error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("<color=red>" + w.text + "</color>"); // Error
                }
                else
                {
                    Debug.Log("<color=green>" + w.text + "</color>");// Items exist
                    Item[] data = JsonConvert.DeserializeObject<Item[]>(w.text);

                    DataHandler.items.AddRange(data);
                    DataHandler.items.Sort(SortByNationalId);
                }
            }
        }
        w.Dispose();
    }

    public static IEnumerator GetPlayerItems()
    {
        DataHandler.playerItems.Clear();
        string url = DataHandler.url + "getPlayerItems.php";
        WWWForm form = new WWWForm();
        form.AddField("player_id", DataHandler.player.id.ToString());

        WWW w = new WWW(url, form);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>"); // Error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("<color=red>" + w.text + "</color>"); // Error
                }
                else
                {
                    Debug.Log("<color=green>" + w.text + "</color>"); // Player-Items exist
                    PlayerItem[] data = JsonConvert.DeserializeObject<PlayerItem[]>(w.text);

                    DataHandler.playerItems.AddRange(data);
                }
            }
        }
        w.Dispose();
    }

    public static IEnumerator GetRarities()
    {
        DataHandler.rarities.Clear();
        string url = DataHandler.url + "getRarities.php";
        WWW w = new WWW(url);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>"); // Error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("<color=red>" + w.text + "</color>"); // Error
                }
                else
                {
                    Debug.Log("<color=green>" + w.text + "</color>");// Rarities exist
                    Rarity[] data = JsonConvert.DeserializeObject<Rarity[]>(w.text);

                    DataHandler.rarities.AddRange(data);
                }
            }
        }

        w.Dispose();
    }

    public static IEnumerator GetTags()
    {
        DataHandler.tags.Clear();
        string url = DataHandler.url + "getTags.php";
        WWW w = new WWW(url);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>"); // Error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("<color=red>" + w.text + "</color>"); // Error
                }
                else
                {
                    Debug.Log("<color=green>" + w.text + "</color>");// Tags exist
                    Tag[] data = JsonConvert.DeserializeObject<Tag[]>(w.text);

                    DataHandler.tags.AddRange(data);
                }
            }
        }

        w.Dispose();
    }

    public static IEnumerator GetItemTags()
    {
        string url = DataHandler.url + "getitemTags.php";
        WWW w = new WWW(url);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>"); // Error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("<color=red>" + w.text + "</color>"); // Error
                }
                else
                {
                    Debug.Log("<color=green>" + w.text + "</color>");// Item-Tags exist
                    ItemTag[] data = JsonConvert.DeserializeObject<ItemTag[]>(w.text);
                    foreach (ItemTag it in data)
                    {
                        Item i = DataHandler.items.Find(x => x.id.Equals(it.itemId));
                        i.setTag(DataHandler.tags.Find(x => x.id.Equals(it.tagId)));
                    }
                }
            }
        }

        w.Dispose();
    }

    private static void writeFile(string fileName, string json)
    {
        string path = getFilePath(fileName);
        FileStream stream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(stream))
        {
            writer.Write(json);
        }
    }

    private static string readFromFile(string fileName)
    {
        string path = getFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("No save found");
            return "";
        }
    }

    private static string getFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(save);
        writeFile(file, json);
    }

    public static void Load()
    {
        save = new SaveData();
        string json = readFromFile(file);
        JsonUtility.FromJsonOverwrite(json, save);
    }

    public static void reloadCollectedLocationList()
    {
        foreach (CollectedLocation cl in save.collectedLocationsList.ToArray())
        {
            if (DateTime.Parse(cl.expiredTime) < DateTime.Now)
            {
                save.collectedLocationsList.Remove(cl);
            }
        }
        DataHandler.Save();
    }

    public static void addToCollectedLocationList(LocationSpot location)
    {
        save.collectedLocationsList.Add(new CollectedLocation(){location = location.id, expiredTime = DateTime.Now.AddMinutes(5).ToString()});
        DataHandler.Save();
    }

    static int SortByNationalId(Item i1, Item i2)
    {
        return i1.nid.CompareTo(i2.nid);
    }
}
