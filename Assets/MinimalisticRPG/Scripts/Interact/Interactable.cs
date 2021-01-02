using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Interact
{
    public abstract class Interactable : MonoBehaviour
    {
        public string Name { get; private set; } = "";

        private void Start()
        {
            Debug.Log(name);
            Name = NameDB.Instance.GetName(name);
        }

        public abstract void Interact();
    }
}

