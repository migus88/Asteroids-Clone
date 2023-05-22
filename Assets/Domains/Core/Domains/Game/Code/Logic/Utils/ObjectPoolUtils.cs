using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Migs.Asteroids.Game.Logic.Utils
{
    public static class ObjectPoolUtils
    {
        public static void PreloadObject<T>(ObjectPool<T> pool, int amount) where T : class
        {
            for (var i = 0; i < amount; i++)
            {
                var obj = pool.Get();
                pool.Release(obj);
            }
        }

        public static void OnObjectReleased<T>(T obj) where T : MonoBehaviour
        {
            if (!obj)
            {
                return;
            }
            
            obj.gameObject.SetActive(false);
        }

        public static void OnObjectRetrieved<T>(T obj) where T : MonoBehaviour
        {
            if (!obj)
            {
                return;
            }
            
            obj.gameObject.SetActive(true);
        }

        public static T CreateObject<T>(T prefab, int ordinal) where T : MonoBehaviour
        {
            if (!prefab)
            {
                throw new Exception($"Objects of type {typeof(T)} are not loaded");
            }
            
            var obj = Object.Instantiate(prefab);
            obj.name = obj.name.Replace("Clone", ordinal.ToString());
            return obj;
        }
    }
}