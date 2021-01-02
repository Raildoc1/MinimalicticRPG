using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace KG.Interact
{
    [RequireComponent(typeof(Usable))]
    public class Talkable : Interactable
    {

        private Usable usable;

        private void Awake()
        {
            usable = GetComponent<Usable>();
        }

        public override void Interact()
        {
            usable.OnUseUsable();
            usable.gameObject.SendMessage("OnUse", transform, SendMessageOptions.DontRequireReceiver);
        }
    }
}

