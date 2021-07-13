using KG.Core;

namespace KG.CombatCore
{
    public class PlayerCombat : Combat
    {
        private InputHandler _inputHandler;

        private void OnEnable()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            _inputHandler.OnMainKeyInput += Attack;
            _inputHandler.DodgeOnKeyInput += Dodge;
        }

        private void OnDisable()
        {
            _inputHandler.OnMainKeyInput -= Attack;
            _inputHandler.DodgeOnKeyInput -= Dodge;
        }
    }
}