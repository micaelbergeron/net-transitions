using System;

namespace Transitions
{
    /// <summary>
    /// Manages transitions for int properties.
    /// </summary>
    internal class Int32ManagedType : ManagedType<int>
    {
		#region IManagedType Members

		/// <summary>
		/// Returns the value between the start and end for the percentage passed in.
		/// </summary>
        public override int GetIntermediateValue(int start, int end, double dPercentage)
		{
            return Utility.Interpolate(start, end, dPercentage);
		}

		#endregion
	}
}
