﻿using System;
using System.Collections.Generic;

namespace Transitions.TransitionTypes
{
    /// <summary>
    /// This class allows you to create user-defined transition types. You specify these
    /// as a list of TransitionElements. Each of these defines: 
    /// End time , End value, Interpolation method
    /// 
    /// For example, say you want to make a bouncing effect with a decay:
    /// 
    /// EndTime%    EndValue%   Interpolation
    /// --------    ---------   -------------
    /// 50          100         Acceleration 
    /// 75          50          Deceleration
    /// 85          100         Acceleration
    /// 91          75          Deceleration
    /// 95          100         Acceleration
    /// 98          90          Deceleration
    /// 100         100         Acceleration
    /// 
    /// The time values are expressed as a percentage of the overall transition time. This 
    /// means that you can create a user-defined transition-type and then use it for transitions
    /// of different lengths.
    /// 
    /// The values are percentages of the values between the start and end values of the properties
    /// being animated in the transitions. 0% is the start value and 100% is the end value.
    /// 
    /// The interpolation is one of the values from the InterpolationMethod enum.
    /// 
    /// So the example above accelerates to the destination (as if under gravity) by
    /// t=50%, then bounces back up to half the initial height by t=75%, slowing down 
    /// (as if against gravity) before falling down again and bouncing to decreasing 
    /// heights each time.
    /// 
    /// </summary>
    public class UserDefined : ITransitionType
    {
        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserDefined()
        {
        }

        /// <summary>
        /// Constructor. You pass in the list of TransitionElements and the total time
        /// (in milliseconds) for the transition.
        /// </summary>
        public UserDefined(IList<TransitionElement> elements, int transitionTime)
        {
            Setup(elements, transitionTime);
        }

        /// <summary>
        /// Sets up the transitions. 
        /// </summary>
        public void Setup(IList<TransitionElement> elements, int transitionTime)
        {
            _elements = elements;
            _transitionTime = transitionTime;

            // We check that the elements list has some members...
            if (elements.Count == 0)
            {
                throw new Exception("The list of elements passed to the constructor of TransitionType_UserDefined had zero elements. It must have at least one element.");
            }
        }

        #endregion

        #region ITransitionMethod Members

        /// <summary>
        /// Called to find the value for the movement of properties for the time passed in.
        /// </summary>
        public bool OnTimer(int time, out double percentage)
        {
            double transitionTimeFraction = time / _transitionTime;

            // We find the information for the element that we are currently processing...
            double elementStartTime;
            double elementEndTime;
            double elementStartValue;
            double elementEndValue;
            InterpolationMethod eInterpolationMethod;
            GetElementInfo(transitionTimeFraction, out elementStartTime, out elementEndTime, out elementStartValue, out elementEndValue, out eInterpolationMethod);

            // We find how far through this element we are as a fraction...
            double elementInterval = elementEndTime - elementStartTime;
            double elementElapsedTime = transitionTimeFraction - elementStartTime;
            double elementTimeFraction = elementElapsedTime / elementInterval;

            // We convert the time-fraction to an fraction of the movement within the
            // element using the interpolation method...
            double dElementDistance;
            switch (eInterpolationMethod)
            {
                case InterpolationMethod.Linear:
                    dElementDistance = elementTimeFraction;
                    break;
                case InterpolationMethod.Accleration:
                    dElementDistance = Utility.ConvertLinearToAcceleration(elementTimeFraction);
                    break;
                case InterpolationMethod.Deceleration:
                    dElementDistance = Utility.ConvertLinearToDeceleration(elementTimeFraction);
                    break;
                case InterpolationMethod.EaseInEaseOut:
                    dElementDistance = Utility.ConvertLinearToEaseInEaseOut(elementTimeFraction);
                    break;
                default:
                    throw new NotSupportedException("Interpolation method not handled: " + eInterpolationMethod);
            }

            // We now know how far through the transition we have moved, so we can interpolate
            // the start and end values by this amount...
            percentage = Utility.Interpolate(elementStartValue, elementEndValue, dElementDistance);

            // Has the transition completed?
            if (time >= _transitionTime)
            {
                // The transition has completed, so we make sure that
                // it is at its final value...
                percentage = elementEndValue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the element info for the time-fraction passed in. 
        /// </summary>
        private void GetElementInfo(double timeFraction, out double startTime, out double endTime, out double startValue, out double endValue, out InterpolationMethod interpolationMethod)
        {
            // We need to return the start and end values for the current element. So this
            // means finding the element for the time passed in as well as the previous element.

            // We hold the 'current' element as a hint. This was in fact the 
            // element used the last time this function was called. In most cases
            // it will be the same one again, but it may have moved to a subsequent
            // on (maybe even skipping elements if enough time has passed)...
            int iCount = _elements.Count;
            for (; _currentElement < iCount; ++_currentElement)
            {
                TransitionElement element = _elements[_currentElement];
                double dElementEndTime = element.EndTime / 100.0;
                if (timeFraction < dElementEndTime)
                {
                    break;
                }
            }

            // If we have gone past the last element, we just use the last element...
            if (_currentElement == iCount)
                _currentElement = iCount - 1;

            // We find the start values. These come from the previous element, except in the
            // case where we are currently in the first element, in which case they are zeros...
            startTime = 0.0;
            startValue = 0.0;
            if (_currentElement > 0)
            {
                var previousElement = _elements[_currentElement - 1];
                startTime = previousElement.EndTime / 100.0;
                startValue = previousElement.EndValue / 100.0;
            }

            // We get the end values from the current element...
            var currentElement = _elements[_currentElement];
            endTime = currentElement.EndTime / 100.0;
            endValue = currentElement.EndValue / 100.0;
            interpolationMethod = currentElement.InterpolationMethod;
        }

        #endregion

        #region Private data

        // The collection of elements that make up the transition...
        private IList<TransitionElement> _elements;

        // The total transition time...
        private double _transitionTime;

        // The element that we are currently in (i.e. the current time within this element)...
        private int _currentElement;

        #endregion
    }
}
