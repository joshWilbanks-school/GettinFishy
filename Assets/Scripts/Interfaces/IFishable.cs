using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    internal interface IFishable
    {
        void Initialize(float xMin, float xMax, float moveSpeed, float fleeSpeed, float maxFleeSpeed, float value, float weight);
        void Swim();
        void Flee();
        float GetValue();
        float GetWeight();
        Size GetSize();
    }
}
