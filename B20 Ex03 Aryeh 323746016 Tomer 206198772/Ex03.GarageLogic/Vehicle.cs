﻿using System.Text;
using System.Collections.Generic;
using System;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private readonly string r_Model;
        private readonly string r_LicenseNumber;
        private readonly string r_NameOfOwner;
        private readonly string r_PhoneNumOfOwner;
        protected bool m_CanBeElectric;
        private eStatus m_StatusOfVehicle;
        protected List<Wheel> m_Wheels;
        protected Energy m_Energy;

        internal Vehicle(
            string i_Model,
            string i_LicenseNumber,
            string i_NameOfOwner,
            string i_PhoneNumOfOwner)
        {
            r_LicenseNumber = i_LicenseNumber;
            r_Model = i_Model;
            r_NameOfOwner = i_NameOfOwner;
            r_PhoneNumOfOwner = i_PhoneNumOfOwner;
            m_StatusOfVehicle = eStatus.InRepair;
            m_Wheels = new List<Wheel>();
            m_Energy = null;
        }
        
        public abstract Tuple<string, string[]>[] GetUniqueFeatureDescription();

        public abstract object ParseUniqueFeature(string i_UniqueFeature, string i_FeatureKey);

        public abstract void SetUniqueFeatures(params object[] i_UniqueFeatures);

        public abstract void SetWheels(string i_WheelManufacturer, float i_CurrentAirPressure);

        public abstract void SetEnergy(bool i_IsElectric, float i_CurrentEnergyLevel);
                        
        protected virtual void InitWheels(byte i_NumOfWheels, string i_WheelManufacturer, float i_CurrentAirPressure, float i_MaxAirPressure)
        {
            if (m_Wheels.Count == 0)
            {
                if (i_CurrentAirPressure > i_MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, i_MaxAirPressure, "airPressure");
                }

                for(int i = 0; i < i_NumOfWheels; i++)
                {
                    m_Wheels.Add(new Wheel(i_WheelManufacturer, i_MaxAirPressure, i_CurrentAirPressure));
                }
            }
        }

        protected virtual void InitEnergy(float i_CurrentEnergyLevel, float i_MaxEnergyCapacity, eEnergyType i_EnergyType)
        {
            if (i_CurrentEnergyLevel > i_MaxEnergyCapacity)
            { 
                throw new ValueOutOfRangeException(0, i_MaxEnergyCapacity, "energy");
            }

            m_Energy = new Energy(i_CurrentEnergyLevel, i_MaxEnergyCapacity, i_EnergyType);
        }

        internal void FillTires(bool i_FillAll, float i_AirToFill = 0)
        {
            if (m_Wheels.Count == 0)
            {
                throw new NullReferenceException("Wheels were not initialized. Use SetWheels() first");
            }
            
            foreach(Wheel wheel in m_Wheels)
            { 
                wheel.FillTire(i_FillAll, i_AirToFill);
            }
        } 
        
        internal void FillEnergy(float i_Energy, eEnergyType i_EnergyType)
        {
            if (m_Energy == null)
            {
                throw new NullReferenceException("energy was not initialized. Use SetEnergy() first");
            }

            m_Energy.FillEnergy(i_Energy, i_EnergyType);  
        }

        internal eStatus StatusOfVehicle  
        {
            get
            {
                return m_StatusOfVehicle;
            } 
            
            set
            {
                m_StatusOfVehicle = value;
            }  
        }

        internal eEnergyType GetEnergyType()
        {
            if (m_Energy == null)
            {
                throw new NullReferenceException("energy was not initialized. Use SetEnergy() first");
            }

            return m_Energy.EnergyType;
        }

        public bool CanBeElectric()
        {
            return m_CanBeElectric;
        }

        public override string ToString()
        {
            StringBuilder sbForWheels = new StringBuilder(string.Empty);
            int i = 0;
            foreach (Wheel wheel in m_Wheels)
            {
                sbForWheels.Append($"  wheel number {i}: {wheel}");
                sbForWheels.Append(Environment.NewLine);
                i++;
            }

            sbForWheels.Remove(sbForWheels.Length - 1, 1);

            string strToReturn = @"
# License Number: {0}
# Model: {1}
# Name of owner: {2}
# Owner phone number: {3}
# Status of vehicle: {4}
# Type of energy vehicle takes: {5}
# Energy left: {6}%
# Wheels info:
{7}";

            return string.Format(
                strToReturn,
                r_LicenseNumber, 
                r_Model,
                r_NameOfOwner,
                r_PhoneNumOfOwner,
                m_StatusOfVehicle,
                m_Energy.EnergyType,
                m_Energy.GetEnergyPercentage(),
                sbForWheels);
        }
    }
}