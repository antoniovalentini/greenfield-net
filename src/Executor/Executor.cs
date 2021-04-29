using System;

namespace Executor
{
    public class Executor<T>
    {
        private Exception _ex;
        public T Value { get; }

        public Executor(T arg = default(T))
        {
            Value = arg;
        }

        public Executor<T> Then(Action task)
        {
            if (_ex != null) return this;
            try
            {
                task();
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught Exception. Chain interrupted.");
                _ex = e;
                return null;
            }

            return new Executor<T>(Value);
        }

        public Executor<TConverted> Then<TConverted>(Func<T, TConverted> task)
        {
            if (_ex != null) return new Executor<TConverted>();
            TConverted converted;
            try
            {
                converted = task(Value);

            }
            catch (Exception e)
            {
                Console.WriteLine("Caught Exception. Chain interrupted.");
                _ex = e;
                return null;
            }

            return new Executor<TConverted>(converted);
        }

        // When Catch is omitted exceptions go silently unhandled.
        public Executor<T> Catch(Action<Exception> catchAction)
        {
            if (_ex != null) catchAction(_ex);
            return this;
        }
    }
}
