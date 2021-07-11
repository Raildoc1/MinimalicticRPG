using KG.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemsList : ScriptableObject
{
    public List<Item> items;

    public Item FindItemByHash(int hash)
    {
        foreach (var item in items)
        {
            if (item.hash == hash)
            {
                return item;
            }
        }

        return null;
    }

    public Item FindItemByName(string name)
    {
        return FindItemByHash(name.GetHashCode());
    }
}
