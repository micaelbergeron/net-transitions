using System;

namespace Transitions
{
	/// <summary>
	/// Manages transitions for double properties.
	/// </summary>
    internal class DoubleManagedType : ManagedType<double>
	{
		#region IManagedType Members

		/// <summary>
		/// Returns the value between start and end for the percentage passed in.
		/// </summary>
        public override double GetIntermediateValue(double start, double end, double dPercentage)
		{
            return Utility.Interpolate(start, end, dPercentage);
		}

		#endregion
	}
}
