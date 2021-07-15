using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    public class ItemCollection : MonoBehaviour
    {

        public ItemsList itemsList;

        [SerializeField] private long gold = 0;
        public List<ItemStack> items = new List<ItemStack>();

        private void Awake()
        {
            if (!itemsList)
            {
                Debug.LogError($"No ItemsList assigned on {name}!");
            }
        }

        public Item FindItemInDatabaseByName(string name)
        {
            return FindItemInDatabaseByHash(name.GetHashCode());
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

            var hash = itemName.GetHashCode();

            foreach (ItemStack stack in items)
            {
                if (stack.item.hash == hash)
                {
                    stack.amount += amount;
                    return;
                }
            }

            ItemStack newItemStack = new ItemStack(itemName, amount, itemsList);
            items.Add(newItemStack);

            Sort();
        }

        public void AddItems(string itemName, double amount)
        {
            AddItems(itemName, (int)amount);
        }

        public void RemoveItems(string itemName, int amount = 1)
        {
            if (amount < 1)
            {
                Debug.LogWarning($"Can't remove {amount} items!");
                return;
            }

            var hash = itemName.GetHashCode();

            foreach (ItemStack stack in items)
            {
                if (stack.item.hash == hash)
                {
                    if (stack.amount < amount) Debug.LogError($"Trying to remove {amount} items, but have only {stack.amount}!");
                    stack.amount -= amount;
                    if (stack.amount < 1) items.Remove(stack);
                    return;
                }
            }

            Sort();

        }

        public void RemoveItems(string itemName, double amount)
        {
            RemoveItems(itemName, (int)amount);
        }

        public void EquipItem(string itemName, bool equiped = true)
        {

            var hash = itemName.GetHashCode();

            foreach (ItemStack stack in items)
            {
                if (stack.item.hash == hash)
                {
                    stack.isEquiped = equiped;
                    //Debug.Log($"EquipItem({itemName}) successfully");
                    return;
                }
            }

            //Debug.Log($"EquipItem({itemName}) failed");
        }

        public bool HasItems(string itemName, int amount = 1)
        {

            var hash = itemName.GetHashCode();

            foreach (ItemStack stack in items)
            {
                if (stack.item.hash == hash)
                {
                    return stack.amount >= amount;
                }
            }
            return false;
        }

        public bool HasItems(string itemName, double amount)
        {
            return HasItems(itemName, (int)amount);
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

        public Item GetItemByIndex(int index)
        {
            if (index >= items.Count)
            {
                return null;
            }

            return items[index].item;
        }

        private void Sort()
        {
            if (items.Count < 2)
            {
                return;
            }

            items.Sort((a, b) =>
            {
                var temp = a.item.type.CompareTo(b.item.type);
                temp = temp == 0 ? a.item.itemName.CompareTo(b.item.itemName) : temp;
                return temp;
            });
        }

    }


    [System.Serializable]
    public class ItemStack
    {
        public string name; // Used by inspector
        public Item item;
        public int amount;
        public bool isEquiped = false;
        public ItemStack(string itemName, int itemsAmount, ItemsList itemsList)
        {
            name = itemName;
            item = itemsList.FindItemByName(itemName);
            amount = itemsAmount;
        }

    }
}


