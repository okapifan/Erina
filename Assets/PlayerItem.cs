using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem
{
    public int id; //Not implemented
    public int playerId;
    public Item item;
    public int amount;

    public PlayerItem(int player_id, int item_id, int amount)
    {
        this.playerId = player_id;
        this.item = DataHandler.items.Find(x => x.id.Equals(item_id));
        this.amount = amount;
    }
}
