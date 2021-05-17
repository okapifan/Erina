using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewScroll", menuName = "Erina/ItemScroll", order = 3)]
public class RoleScrollItem : Item
{
    public string raceName;

    public RoleScrollItem(int nid, string name, string description, string image, int rarity_id, int maxHp, int speed, int attack, int armor, int isConsumable, string raceName) : base(nid, name, description, image, rarity_id, maxHp, speed, attack, armor, 1)
    {
        this.raceName = raceName;
    }

    public void Use()
    {
        // Todo create Use() function
    }
}
