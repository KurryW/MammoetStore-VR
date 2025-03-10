namespace Utility
{
    /**
     * Checkable class
     * Tim Falken
     * Simple class with a generic inner value, which provides a convenient method to check if the inner value has been altered
     */

    class Checkable<T>
    {
        private T innerValue = default;
        private bool changed = false;

        public Checkable() { }
        public Checkable(T initialValue)
        {
            innerValue = initialValue;
        }

        public bool HasChanged
        {
            get
            {
                bool cha = changed;
                changed = false;
                return cha;
            }
        }

        public T Value
        {
            get
            {
                return innerValue;
            }

            set
            {
                if (!value.Equals(innerValue))
                    changed = true;

                innerValue = value;
            }
        }

        public static implicit operator T(Checkable<T> c)
        {
            return c.Value;
        }
    }
}
