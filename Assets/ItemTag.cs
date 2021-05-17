using UnityEngine;
using System.Collections;

public class ItemTag : MonoBehaviour
{
    public int itemId;
    public int tagId;

    public ItemTag(int item_id, int tag_id)
    {
        itemId = item_id;
        tagId = tag_id;
    }
}
