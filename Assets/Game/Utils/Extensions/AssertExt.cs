using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Utils
{
    public static class AssertExt
    {
        public static void Init()
        {
            Assert.raiseExceptions = Debug.isDebugBuild;
        }
        
        public static void Fail(string msg)
        {
            Assert.IsFalse(true, msg);
        }
    }
}