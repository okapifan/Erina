using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "NewItem", menuName = "Erina/Item", order = 2)]
public class Item : ScriptableObject
{
    public int id;
    public int nid;
    //public string name;
    public string description;
    public Sprite image;
    public Rarity rarity;
    public int maxHp;
    public int speed;
    public int attack;
    public int armor;
    public bool isConsumable;
    public List<Tag> tags = new List<Tag>();

    public Item(int nid, string name, string description, string image, int rarity_id, int maxHp, int speed, int attack, int armor, int isConsumable)
    {
        this.nid = nid;
        this.name = name;
        this.description = description;
        this.image = Resources.Load<Sprite>(image);
        this.rarity = DataHandler.rarities.Find(x => x.id.Equals(rarity_id));
        this.maxHp = maxHp;
        this.speed = speed;
        this.attack = attack;
        this.armor = armor;
        this.isConsumable = (isConsumable == 1) ? true : false;

        
    }

    public void setTag(Tag tag)
    {
        this.tags.Add(tag);
    }

    public string getTags()
    {
        List<string> tagNames = new List<string>();
        foreach(Tag tag in tags)
        {
            tagNames.Add(tag.name);
        }
        return String.Join(", ", tagNames);
    }

    public void OnValidate()
    {
        if (this.id == 0)
        {
            DataHandler.items.Add(this);
            this.id = DataHandler.items.Max(x => x.id) + 1;
        }
    }
}
