﻿using System;

namespace Ex03.GarageLogic
{
    public class VehicleBuilder
    {
        private readonly Vehicle r_Vehicle;
        private int m_NumOfSetMethodsCalled;
        private const int k_NumOfMethodsThatMustBeCalledToFinnishVehicle = 3;

        internal VehicleBuilder(Vehicle i_Vehicle)
        {
            r_Vehicle = i_Vehicle;
            m_NumOfSetMethodsCalled = 0;
        }

        public Tuple<string, string[]>[] GetUniqueFeatureDescription()
        {
            return r_Vehicle.GetUniqueFeatureDescription();
        }

        public object ParseUniqueFeature(string i_UniqueFeature, string i_FeatureKey)
        {
            return r_Vehicle.ParseUniqueFeature(i_UniqueFeature, i_FeatureKey);
        }

        public void SetUniqueFeatures(params object[] i_UniqueFeatures)
        {
            m_NumOfSetMethodsCalled++;
            r_Vehicle.SetUniqueFeatures(i_UniqueFeatures);
        }

        public void SetWheels(string i_WheelManufactor, float i_CurrentAirPressure)
        {
            m_NumOfSetMethodsCalled++;
            r_Vehicle.SetWheels(i_WheelManufactor, i_CurrentAirPressure);
        }

        public void SetEnergy(bool i_IsElectric, float i_CurrentEnergyLevel)
        {
            m_NumOfSetMethodsCalled++;
            r_Vehicle.SetEnergy(i_IsElectric, i_CurrentEnergyLevel);
        }

        public bool CanBeElectric()
        {
            return r_Vehicle.CanBeElectric();
        }

        public bool TryGetFinnishedVehicle(out Vehicle i_Vehicle)
        {
            bool succeededBuildingCar = false;
            i_Vehicle = null;
            if (m_NumOfSetMethodsCalled >= k_NumOfMethodsThatMustBeCalledToFinnishVehicle)
            {
                i_Vehicle = r_Vehicle;
                succeededBuildingCar = true;
            }

            return succeededBuildingCar;
        }
    }
}