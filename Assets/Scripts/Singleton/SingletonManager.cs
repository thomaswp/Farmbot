using System.Collections;
using UnityEngine;

namespace Farmbot.Overworld.Buildings
{
    public class SingletonManager : MonoBehaviour
    {
        private static SingletonManager instance;

        // Use this for initialization
        void Awake()
        {
            instance = this;
        }

        public static T GetSingleton<T>() where T : MonoBehaviour
        {
            return instance.GetComponent<T>();
        }
    }
}