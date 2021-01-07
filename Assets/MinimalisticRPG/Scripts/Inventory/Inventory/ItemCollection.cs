using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    public class ItemCollection : MonoBehaviour
    {

        public ItemsList itemsList;

        [SerializeField] private long gold = 0;
        [SerializeField] private List<ItemStack> items = new List<ItemStack>();

        private void Awake()
        {
            if (!itemsList)
            {
                Debug.LogError($"No ItemsList assigned on {name}!");
            }
        }

        public Item FindItemInDatabaseByName(string name)
        {
            return FindItemInDatabaseByHash(Animator.StringToHash(name));
        }

        public Item FindItemInDatabaseByHash(int hash)
        {
            return itemsList.FindItemByHash(hash);
        }

        public void AddItems(string itemName, int amount = 1)
        {
            if (amount < 1)
            {
                Debug.LogWarning($"Can't add {amount} items!");
                return;
            }

            foreach (ItemStack i in items)
            {
                if (i.itemName.Equals(itemName))
                {
                    i.itemsAmount += amount;
                    return;
                }
            }
            ItemStack newItemStack = new ItemStack(itemName, amount);
            items.Add(newItemStack);
        }

        public void RemoveItems(string itemName, int amount = 1)
        {
            if (amount < 1)
            {
                Debug.LogWarning($"Can't remove {amount} items!");
                return;
            }

            foreach (ItemStack i in items)
            {
                if (i.itemName.Equals(itemName))
                {
                    if (i.itemsAmount < amount) Debug.Log($"Trying to remove {amount} items, but have only {i.itemsAmount}!");
                    i.itemsAmount -= amount;
                    if (i.itemsAmount < 1) items.Remove(i);
                    return;
                }
            }
        }

        public bool HasItems(string itemName, int amount = 1)
        {
            foreach (ItemStack i in items)
            {
                if (i.itemName.Equals(itemName))
                {
                    return i.itemsAmount >= amount;
                }
            }
            return false;
        }
        public void EarnGold(double amount)
        {
            gold += (long)amount;
        }

        public bool SpendGold(double amount)
        {
            if (gold < amount)
            {
                return false;
            }

            gold -= (long)amount;

            return true;
        }

    }


    [System.Serializable]
    public class ItemStack
    {
        public string itemName;
        public int itemsAmount;
        public ItemStack(string itemName, int itemsAmount)
        {
            this.itemName = itemName;
            this.itemsAmount = itemsAmount;
        }

    }
}


