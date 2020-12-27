using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KG.Inventory {
    public class ItemDatabaseAsset {
        public List<ItemPropery> items;

        [System.Serializable]
        public class ItemPropery {
            public string name;
            public ItemDescription ItemDescription;
        }

        [System.Serializable]
        public class ItemDescription {
            public string title;
            public string description;
        }
    }
}
