using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Core
{
    public class GameTime : MonoBehaviour
    {

        #region Singleton

        public static GameTime instance;

        private void InitSingleton()
        {
            if (instance)
            {
                Debug.LogError("More than one instance of GameTime found!");
                Destroy(this);
                return;
            }

            instance = this;
        }

        #endregion

        public long minutes { get; private set; } = 0;
        public long hour => (minutes / 60) % 24;
        public long day => minutes / (60 * 24);

        public float realSecondsInMinute = 2f;

        private float deltaTime = 0;

        private void Update()
        {
            deltaTime += Time.deltaTime;

            if (deltaTime > realSecondsInMinute)
            {
                minutes++;
                deltaTime -= realSecondsInMinute;
            }
        }

    }
}

