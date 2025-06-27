using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    internal interface IReeler
    {
        public void SetIsCatching(bool isCatching);
        public void ResetForNextCast();
        public void SetIsCasted(bool isCasted);
    }
}
