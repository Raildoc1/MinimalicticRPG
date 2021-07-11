using KG.Core;
using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;

namespace KG.Interact
{
    [RequireComponent(typeof(Usable))]
    [RequireComponent(typeof(StateSwitch))]
    public class Talkable : Interactable
    {

        [SerializeField] private bool _talkOnTrigger = false;

        public bool talkOnTrigger
        {
            get
            {
                return _talkOnTrigger;
            }
            set
            {
                if (_talkOnTrigger == value)
                {
                    return;
                }

                _talkOnTrigger = value;

                if (checkForTargetRoutine != null)
                {
                    StopCoroutine(checkForTargetRoutine);
                }

                if (_talkOnTrigger)
                {
                    checkForTargetRoutine = StartCoroutine(checkForDialogTargetCloseEnough());
                }
            }
        }

        public Transform player;
        public float maxTalkDistance;
        public float checkForTargetTime = 0.5f;

        private Coroutine checkForTargetRoutine;

        private Usable usable;
        private StateSwitch stateSwitch;

        private void Awake()
        {
            usable = GetComponent<Usable>();
            stateSwitch = GetComponent<StateSwitch>();
        }

        private IEnumerator checkForDialogTargetCloseEnough()
        {
            Debug.Log($"Start check for target!");
            while (true)
            {
                if (!talkOnTrigger)
                {
                    yield break;
                }

                Debug.Log($"{name} checks for target!");

                if (Vector3.Distance(transform.position, player.position) < maxTalkDistance)
                {
                    Interact(player);
                    talkOnTrigger = false;
                    yield break;
                }

                yield return new WaitForSeconds(checkForTargetTime);
            }
        }


        protected override void Start()
        {
            base.Start();

            if (talkOnTrigger)
            {
                checkForTargetRoutine = StartCoroutine(checkForDialogTargetCloseEnough());
            }

        }

        private void OnDisable()
        {
            if (checkForTargetRoutine != null)
            {
                StopCoroutine(checkForTargetRoutine);
            }
        }

        public override void Interact(Transform origin)
        {
            var originStateSwitch = origin.GetComponent<StateSwitch>();

            if (!originStateSwitch)
            {
                Debug.LogError("Something with no StateSwitch trying to start a dialog!");
                return;
            }

            originStateSwitch.StartDialog(stateSwitch, true);

            usable.OnUseUsable();
            usable.gameObject.SendMessage("OnUse", transform, SendMessageOptions.DontRequireReceiver);
        }

        public void TurnOnTrigger()
        {
            Debug.Log("TUEN ON");
            talkOnTrigger = true;
        }

        public void TurnOffTrigger()
        {
            talkOnTrigger = false;
        }
    }
}

