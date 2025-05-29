using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public static class EntityExtension
    {
        private static int s_SerialId = 0;
        
        public static int GenerateSerialId()
        {
            return ++s_SerialId;
        }
    }
}
