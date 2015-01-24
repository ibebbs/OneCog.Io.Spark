using System;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Test
{
    public static class TaskEx
    {
        public static Task<T> FromException<T>(Exception exception)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            tcs.SetException(exception);
            return tcs.Task;
        }
    }
}
