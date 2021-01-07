using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    [System.Serializable]
    public class Item
    {
        public string itemName;
        public int hash;
        public ItemType type;
        public List<Attribute> attributes;
        public GameObject gameObject;
        public int cost;
        public bool canBeSold = true;

        public int GetAttributeValue(AttributeType type)
        {
            return AttributesTools.GetAttributeValue(attributes, type);
        }

    }

    [System.Serializable]
    public class Attribute
    {
        public AttributeType type;
        public int value;
    }

    public static class AttributesTools
    {
        public static int GetAttributeValue(List<Attribute> attributes, AttributeType type)
        {
            var attribute = attributes.Find(i => i.type.Equals(type));

            if (attribute != null)
            {
                return attribute.value;
            }

            return 0;
        }
    }

    public enum ItemType
    {
        MELEE_WEAPON,
        BOW,
        SPELL,
        FOOD,
        KEY,
        BOOK,
        OTHER
    }

    public enum AttributeType
    {
        PHYSICAL_DAMAGE,
        REQUIRE_STRENGTH,
        REQUIRE_DEXTERITY,
        /*...*/
        CUSTOM
    }
}