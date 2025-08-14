using System;
using System.Threading;

namespace Mod.CuongLe
{
    public static class TaskCompat
    {
        public static void Run(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            ThreadPool.QueueUserWorkItem(_ => action());
        }
    }
}
