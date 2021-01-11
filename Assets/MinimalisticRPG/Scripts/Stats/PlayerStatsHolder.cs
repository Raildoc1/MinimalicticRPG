using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Stats
{
    public class PlayerStatsHolder : StatsHolder
    {

        #region Singleton

        public static PlayerStatsHolder instance;

        private void InitSingleton()
        {
            if (instance)
            {
                Debug.LogError("More than one instance of PlayerInventory found!");
                Destroy(this);
                return;
            }

            instance = this;
        }

        #endregion

        public int Experience
        {
            get
            {
                return _experience;
            }
        }

        protected int _experience = 0;

        protected override void Awake()
        {
            InitSingleton();
        }

        public void AddExperience(double value)
        {
            AddExperience((int)value);
        }

        public void AddExperience(int value)
        {
            _experience += value;
        }


        #region Lua

        private void OnEnable()
        {
            Lua.RegisterFunction("AddExperience", this, SymbolExtensions.GetMethodInfo(() => AddExperience((double)0)));
        }

        private void OnDisable()
        {
            Lua.UnregisterFunction("AddExperience");
        }

        #endregion

    }
}

