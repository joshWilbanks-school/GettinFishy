using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    internal static class Extensions
    {

        public static IEnumerable<Component> GetComponentsOfType<T>(this GameObject obj)
        {
            return obj.GetComponents<Component>().Where(x => x is T);
        }

        public static Component GetComponentOfType<T>(this GameObject obj)
        {
            var component = obj.GetComponents<Component>().FirstOrDefault(x => x is T);
            return component;
        }

        public static Component GetChildComponentOfType<T>(this GameObject obj)
        {
            Component c = obj.GetComponentsOfType<Component>().FirstOrDefault(x => x is T);

            return c; 
        }

        public static void RandomRange(ref this Vector2 obj, Vector2 min, Vector2 max)
        {
            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            obj = new Vector2 (x, y);
        }

    }
}
