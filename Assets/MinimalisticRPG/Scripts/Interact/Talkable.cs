using KG.Core;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace KG.Interact
{
    [RequireComponent(typeof(Usable))]
    [RequireComponent(typeof(StateSwitch))]
    public class Talkable : Interactable
    {

        private Usable usable;
        private StateSwitch stateSwitch;

        private void Awake()
        {
            usable = GetComponent<Usable>();
            stateSwitch = GetComponent<StateSwitch>();
        }

        protected override void Start()
        {
            base.Start();
            Debug.Log($"{name} : {transform.position} : {labelPosition.position}");
        }

        public override void Interact()
        {
            stateSwitch.CurrentState = State.DIALOG;
            usable.OnUseUsable();
            usable.gameObject.SendMessage("OnUse", transform, SendMessageOptions.DontRequireReceiver);
        }
    }
}

