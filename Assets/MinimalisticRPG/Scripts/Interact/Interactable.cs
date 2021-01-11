using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Interact
{
    public abstract class Interactable : MonoBehaviour
    {
        public static bool DEBUG_MODE = false;

        public string uniqueName = "no_name";
        public string Name { get; private set; } = "";
        public Transform labelPosition;

        protected virtual void Start()
        {
            Name = NameDatabase.Instance.GetName(uniqueName);
            if (DEBUG_MODE) Debug.Log($"{name} got name \"{Name}\"");
        }

        public abstract void Interact(Transform origin);
    }
}

