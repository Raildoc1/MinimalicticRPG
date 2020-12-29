using UnityEngine;

namespace KG.Inventory
{
    [CreateAssetMenu(menuName = "Item/Food")]
    public class Food : Item
    {
        public int cureHealthAmount = 0;
    }
}
