using System;
using System.Drawing;

namespace Transitions
{
	/// <summary>
	/// Class that manages transitions for Color properties. For these we
	/// need to transition the R, G, B and A sub-properties independently.
	/// </summary>
    internal class ColorManagedType : ManagedType<Color>
	{
		#region IManagedType Members

		/// <summary>
		/// Returns a copy of the color object passed in.
		/// </summary>
        public override Color Copy(Color o)
		{
			return Color.FromArgb(o.ToArgb());
		}

		/// <summary>
		/// Creates an intermediate value for the colors depending on the percentage passed in.
		/// </summary>
        public override Color GetIntermediateValue(Color start, Color end, double dPercentage)
		{
			// We interpolate the R, G, B and A components separately...

            int newR = Utility.Interpolate(start.R, end.R, dPercentage);
			int newG = Utility.Interpolate(start.G, end.G, dPercentage);
			int newB = Utility.Interpolate(start.B, end.B, dPercentage);
			int newA = Utility.Interpolate(start.A, end.A, dPercentage);

			return Color.FromArgb(newA, newR, newG, newB);
		}

		#endregion
	}
}
