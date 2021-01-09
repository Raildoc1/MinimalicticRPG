using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KG.UI
{
    [RequireComponent(typeof(Image))]
    public class InventorySlotUI : MonoBehaviour
    {

        public int index = 0;
        public Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }


    }
}

