using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    public class SingletonTemplate<T> : MonoBehaviour where T : class
    {
        private static T instance = default(T);
        
        public static T GetInstance()
        {
            return instance;
        }
        // Use this for initialization

        protected virtual void Awake()
        {
            if (EqualityComparer<T>.Default.Equals(instance, default(T)))
                instance = this as T;
            else if (!(EqualityComparer<T>.Default.Equals(instance, default(T))))
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }
}