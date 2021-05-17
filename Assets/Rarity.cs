using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rarity
{
    public int id;
    public string name;
    public Sprite image;

    public Rarity(int id, string name, string icon)
    {
        this.id = id;
        this.name = name;
        this.image = Resources.Load<Sprite>(icon);
    }
}
