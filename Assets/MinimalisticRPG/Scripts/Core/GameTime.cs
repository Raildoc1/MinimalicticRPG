using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Core
{
    public class GameTime : MonoBehaviour
    {

        private static GameTime _instance;
        public static GameTime instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<GameTime>();
                }

                return _instance;
            }
        }

        [SerializeField] private long _minutes = 0;
        public long minutes
        {
            get { return _minutes; }
            private set { _minutes = value; }
        }
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

