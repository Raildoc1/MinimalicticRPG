using KG.Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    [RequireComponent(typeof(SphereCollider))]
    public class ItemPickUp : Interactable
    {
        public class ItemInstance {
            public string name;
            public int amount;
        }

        public List<ItemInstance> items = new List<ItemInstance>();

        public override void Interact()
        {
            Destroy(gameObject);
        }
    }
}

