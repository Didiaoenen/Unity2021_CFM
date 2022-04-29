using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assembly_CSharp.Assets.Script.Simple.Execution
{
    class MainThreadExecutor : MonoBehaviour
    {

    }

    public class Executors
    {
        private static object syncLock = new object();

        private static bool disposed = false;

        private static MainThreadExecutor executor;

        private static Thread mainThread;

#if UNITY_EDITOR
        private static Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
#endif

        public static void RunOnMainThread(Action action, bool waitForExecution = false)
        {

        }
    }
}

