using UnityEngine;

namespace KG.Core
{
    [RequireComponent(typeof(InputHandler))]
    public class PlayerStateSwitch : StateSwitch
    {
        private InputHandler _inputHandler;

        private void OnEnable()
        {
            _inputHandler = GetComponent<InputHandler>();
            _inputHandler.OnDrawWeaponInput += DrawHideWeapon;
        }

        private void OnDisable()
        {
            _inputHandler.OnDrawWeaponInput -= DrawHideWeapon;
        }
    }
}