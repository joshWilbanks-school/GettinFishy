
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    internal interface IHooker
    {
        void Cast(float power, float angle);
        void Reel(float power, GameObject start);
        void Initialize();
    }
}
