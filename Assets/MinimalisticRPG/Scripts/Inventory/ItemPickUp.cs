using KG.Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    [RequireComponent(typeof(SphereCollider))]
    public class ItemPickUp : Interactable
    {
        [System.Serializable]
        public class ItemInstance {
            public string name;
            public int amount;
        }

        public List<ItemInstance> items = new List<ItemInstance>();

        public override void Interact(Transform origin)
        {

            var inventory = origin.GetComponent<ItemCollection>();

            if (!inventory)
            {
                Debug.LogError("Something with no inventory trying to pixk up an item!");
                return;
            }

            foreach (var item in items)
            {
                inventory.AddItems(item.name, item.amount);
            }

            Destroy(gameObject);
        }
    }
}

