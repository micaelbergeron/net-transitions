using System;

namespace Transitions
{
    internal class FloatManagedType : ManagedType<float>
    {
        #region IManagedType Members


        /// <summary>
        /// Returns the interpolated value for the percentage passed in.
        /// </summary>
        public override float GetIntermediateValue(float start, float end, double dPercentage)
        {
            return Utility.Interpolate(start, end, dPercentage);
        }

        #endregion
    }
}
