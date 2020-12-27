﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory {
    public class ItemCollection : MonoBehaviour {
        
        [SerializeField]
        private List<ItemStack> items = new List<ItemStack>();

        public void AddItems(string itemName, int amount = 1) {
            if(amount < 1) {
                Debug.LogWarning($"Can't add {amount} items!");
                return;
            }

            foreach(ItemStack i in items) {
                if(i.itemName.Equals(itemName)) {
                    i.itemsAmount += amount;
                    return;
                }
            }
            ItemStack newItemStack = new ItemStack(itemName, amount);
            items.Add(newItemStack);
        }

        public void RemoveItems(string itemName, int amount = 1) {
            if(amount < 1) {
                Debug.LogWarning($"Can't remove {amount} items!");
                return;
            }

            foreach(ItemStack i in items) {
                if(i.itemName.Equals(itemName)) {
                    if(i.itemsAmount < amount) Debug.Log($"Trying to remove {amount} items, but have only {i.itemsAmount}!");
                    i.itemsAmount -= amount;
                    if(i.itemsAmount < 1) items.Remove(i);
                    return;
                }
            }
        }

        public bool HasItems(string itemName, int amount = 1) {
            foreach(ItemStack i in items) {
                if(i.itemName.Equals(itemName)) {
                    return i.itemsAmount >= amount;
                }
            }
            return false;
        }

    }

    [System.Serializable]
    public class ItemStack {
        public string itemName;
        public int itemsAmount;
        public ItemStack(string itemName, int itemsAmount) {
            this.itemName = itemName;
            this.itemsAmount = itemsAmount;
        }

    }
}


