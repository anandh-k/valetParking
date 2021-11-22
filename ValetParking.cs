﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ValetParking
{
    public class ValetParking : IValetParking
    {
        // Maintain a sorted list to keep track of free available parking slots
        private SortedSet<int> freeParkingLotsCar;
        private SortedSet<int> freeParkingLotsMotorCycle;

        // Maintain a key-value pair to keep track of vehicle and it's parking details
        private Dictionary<string, (int lot, string vehicleType, UInt64 entryTime)> vehicleParkingInfo;

        /// <summary>
        /// This sub-routine will check if the given vehicle is car or motorcycle
        /// </summary>
        /// <param name="vehicleType">The vehicle type.</param>
        /// <returns> true for car, false for motorcycle, null if neither</returns>
        private bool? IsVehicleTypeCar(string vehicleType)
        {
            if (0 == string.Compare(vehicleType, "car", true))
                return true;
            else if (0 == string.Compare(vehicleType, "motorcycle", true))
                return false;
            else
                return null;
        }

        /// <summary>
        /// This sub-routine will find a suitable parking lot (lowest available)
        /// based on the vehicle type.
        /// </summary>
        /// <param name="vehicleType">The vehicle type.</param>
        /// <returns> Valid parking lot if available, -1 if not</returns>
        private int findFreeLot(string vehicleType)
        {
            SortedSet<int> vehicleFreeLots = null;

            switch (IsVehicleTypeCar(vehicleType))
            {
                case true:
                    vehicleFreeLots = freeParkingLotsCar;
                    break;

                case false:
                    vehicleFreeLots = freeParkingLotsMotorCycle;
                    break;

                default:
                    Console.WriteLine("Vehicle type not allowed for this parking garage");
                    return -1;
            }

            if (vehicleFreeLots?.Count > 0)
            {
                int freeVehicleLot = vehicleFreeLots.Min;
                vehicleFreeLots.Remove(freeVehicleLot);
                return freeVehicleLot;
            }
            else
                return -1;

        }

        /// <summary>
        /// This sub-routine will release a previously used parking lot which can be
        /// assigned to other vehicles.
        /// </summary>
        /// <param name="vehicleType">The vehicle type.</param>
        /// <param name="lotNumber">Unique identifier of the parking lot.</param>
        private void addFreeLot(string vehicleType, int lotNumber)
        {
            // Return the lot to corresponding vehicle type
            switch (IsVehicleTypeCar(vehicleType))
            {
                case true:
                    freeParkingLotsCar.Add(lotNumber);
                    break;

                case false:
                    freeParkingLotsMotorCycle.Add(lotNumber);
                    break;

                default:
                    Console.WriteLine("Vehicle type not allowed for this parking garage");
                    break;
            }
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="carLots">Total number of car lots in the parking garage.</param>
        /// <param name="motorcycleLots">Total number of motorcycle lots in the parking garage.</param>
        public ValetParking(UInt32 carLots, UInt32 motorcycleLots)
        {
            // Initialize free lots for car & motorcycle seperately
            freeParkingLotsCar = new SortedSet<int>();
            freeParkingLotsMotorCycle = new SortedSet<int>();

            vehicleParkingInfo = new Dictionary<string, (int lot, string vehicleType, UInt64 entryTime)>();

            // Parking lots start from 1
            for (int i = 1; i <= carLots; i++)
            {
                freeParkingLotsCar.Add(i);
            }

            for (int i = 1; i <= motorcycleLots; i++)
            {
                freeParkingLotsMotorCycle.Add(i);
            }
        }

        /// <summary>
        /// The function that will be invoked when a vehicle enters the parking garage. The vehicle
        /// will only be allowed in if there are any free lots available.
        /// </summary>
        /// <param name="vehicleType">The type of vehicle (either car or motorcycle).</param>
        /// <param name="vehicleNumber">The number of the vehicle.</param>
        /// <param name="timeStamp">The entry time.</param>
        public void Entry(string vehicleType, string vehicleNumber, UInt64 timeStamp)
        {
            // Call sub-routine to find suitable parking lot
            int parkingLot = findFreeLot(vehicleType);

            // If suitable lot is found, let in the vehicle
            if (parkingLot != -1)
            {
                // Add the parking lot and try time information
                vehicleParkingInfo.Add(vehicleNumber, (parkingLot, vehicleType, timeStamp));

                if (0 == string.Compare(vehicleType, "motorcycle"))
                    Console.WriteLine($"Accept MotorcycleLot{parkingLot}");
                else
                    Console.WriteLine($"Accept CarLot{parkingLot}");
            }
            else
            {
                // Deny entry
                Console.WriteLine("Reject");
            }
        }

        /// <summary>
        /// The function that will be invoked when a vehicle exits the parking garage. The lot
        /// that is released and the parking fee is displayed on the console.
        /// </summary>
        /// <param name="vehicleNumber">The number of the vehicle.</param>
        /// <param name="timeStamp">The exit time.</param>
        public void Exit(string vehicleNumber, UInt64 timeStamp)
        {
            // Fetch the parking info from vehicle number
            if (vehicleParkingInfo.TryGetValue(vehicleNumber, out var vehicleLotInfo))
            {
                // Add the parking lot back to free pool
                addFreeLot(vehicleLotInfo.vehicleType, vehicleLotInfo.lot);

                // Calculate parking time in hours (rounded up)
                UInt64 difference = (timeStamp - vehicleLotInfo.entryTime + (3600 - 1)) / 3600;

                switch (IsVehicleTypeCar(vehicleLotInfo.vehicleType))
                {
                    case true:
                        Console.WriteLine($"CarLot{vehicleLotInfo.lot} {difference * 2}");
                        break;

                    case false:
                        Console.WriteLine($"MotorcycleLot{vehicleLotInfo.lot} {difference}");
                        break;

                    default:
                        Console.WriteLine("Vehicle type not allowed for this parking garage");
                        break;
                }
            }
        }
    }
}
