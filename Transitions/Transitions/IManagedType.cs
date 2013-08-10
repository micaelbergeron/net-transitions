using System;

namespace Transitions
{
    /// <summary>
    /// Interface for all types we can perform transitions on. 
    /// Each type (e.g. int, double, Color) that we can perform a transition on 
    /// needs to have its own class that implements this interface. These classes 
    /// tell the transition system how to act on objects of that type.
    /// </summary>
    internal abstract class ManagedType<T>
    {

        /// <summary>
        /// Returns the Type that the instance is managing.
        /// </summary>
        public Type GetManagedType()
        {
            return typeof (T);
        }

        /// <summary>
        /// Returns a deep copy of the object passed in. (In particular this is 
        /// needed for types that are objects.)
        /// </summary>
        public virtual T Copy(T o)
        {
            return o;
        }

        /// <summary>
        /// Returns an object holding the value between the start and end corresponding
        /// to the percentage passed in. (Note: the percentage can be less than 0% or
        /// greater than 100%.)
        /// </summary>
        public abstract T GetIntermediateValue(T start, T end, double dPercentage);

    }
}
