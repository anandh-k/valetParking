using System;
using System.Collections.Generic;
using System.Text;

namespace ValetParking
{
    public interface IValetParking
    {
        /// <summary>
        /// The function that will be invoked when a vehicle enters the parking garage. The vehicle
        /// will only be allowed in if there are any free lots available.
        /// </summary>
        /// <param name="vehicleType">The type of vehicle (either car or motorcycle).</param>
        /// <param name="vehicleNumber">The number of the vehicle.</param>
        /// <param name="timeStamp">The entry time.</param>
        void Entry(string vehicleType, string vehicleNumber, UInt64 timeStamp);

        /// <summary>
        /// The function that will be invoked when a vehicle exits the parking garage. The lot
        /// that is released and the parking fee is displayed on the console.
        /// </summary>
        /// <param name="vehicleNumber">The number of the vehicle.</param>
        /// <param name="timeStamp">The exit time.</param>
        void Exit(string vehicleNumber, UInt64 timeStamp);
    }
}
