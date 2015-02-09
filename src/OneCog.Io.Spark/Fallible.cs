using System;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public static class Fallible
    {
        public static Fallible<T> FromValue<T>(T value)
        {
            return new Fallible<T>(value);
        }

        public static Fallible<T> FromException<T>(Exception exception)
        {
            return new Fallible<T>(exception);
        }

        public static Fallible<T> FromOperation<T>(Func<T> operation)
        {
            try
            {
                return Fallible.FromValue<T>(operation());
            }
            catch (Exception exception)
            {
                return Fallible.FromException<T>(exception);
            }
        }

        public static async Task<Fallible<T>> FromOperationAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                T value = await operation();

                return Fallible.FromValue<T>(value);
            }
            catch (Exception exception)
            {
                return Fallible.FromException<T>(exception);
            }
        }

        public static Fallible<TDest> Map<TSource,TDest>(Fallible<TSource> source, Func<TSource, TDest> map)
        {
            if (source.HasValue)
            {
                return Fallible.FromOperation(() => map(source.Value));
            }
            else
            {
                return Fallible.FromException<TDest>(source.Exception);
            }
        }
    }

    public class Fallible<T>
    {
        public Fallible(T value)
        {
            Value = value;
            HasValue = true;
        }

        public Fallible(Exception exception)
        {
            Exception = exception;
            HasFailed = true;
        }

        public T Value { get; private set; }

        public Exception Exception { get; private set; }

        public bool HasValue { get; private set; }

        public bool HasFailed { get; private set;}
    }
}
