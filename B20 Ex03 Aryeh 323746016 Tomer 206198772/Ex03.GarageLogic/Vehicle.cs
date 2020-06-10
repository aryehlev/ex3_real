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
            string i_PhoneNumOfOwner
            )
        {
            r_LicenseNumber = i_LicenseNumber;
            r_Model = i_Model;
            r_NameOfOwner = i_NameOfOwner;
            r_PhoneNumOfOwner = i_PhoneNumOfOwner;
            m_StatusOfVehicle = eStatus.InRepair;
            m_Wheels = new List<Wheel>();
            m_Energy = null;
        }
        
        public abstract Tuple<string, string[]>[] GetSpecificFeatureDescription();

        public abstract object ParseSpecificFeature(string i_SpecificFeature, string i_FeatureKey);

        public abstract void SetUniqueParamaters(params object[] i_SpecificFeatures);

        public abstract void SetWheels(string i_WheelManufactor, float i_CurrentAirPressure);

        public abstract void SetEnergy(bool i_IsElectric, float i_CurrentEnergyLevel);
        
        public bool CanBeElectric()
        {
            return m_CanBeElectric;
        }
        
        protected virtual void InitWheels(byte i_NumOfWheels, string i_WheelManufactor, float i_CurrentAirPressure, float i_MaxAirPressure)
        {
            if (m_Wheels.Count == 0)
            {
                if (i_CurrentAirPressure > i_MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, i_MaxAirPressure, "airPressure");
                }

                for(int i = 0; i < i_NumOfWheels; i++)
                {
                    m_Wheels.Add(new Wheel(i_WheelManufactor, i_MaxAirPressure, i_CurrentAirPressure));
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
            
            foreach(Wheel wheel in m_Wheels)
            { 
                wheel.FillTire(i_FillAll, i_AirToFill);
            }
            
        } 
        
        internal void FillEnergy(float i_Energy, eEnergyType i_EnergyType)
        {
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
            return m_Energy.EnergyType;
        }

        //public float GetMaxEnergyCapacity()
        //{
        //    return m_Energy.EnergyCapacity;
        //}

        //public float GetMaxAirCapacity()
        //{
        //    return m_Wheels[0].MaxPressure;
        //}

        //public float GetCurrentEnergyAmount()
        //{
        //    return m_Energy.CurrentEnergyAmount;
        //}

        //public float GetCurrentAirInWheelsAmount()
        //{
        //    return m_Wheels[0].CurrentPressure;
        //}
        
        public virtual string AdvancesToStringAfterFeaturesWhereSet()
        {
            StringBuilder sbForWheels = new StringBuilder("");
            int i = 0;
            foreach (Wheel wheel in m_Wheels)
            {
                sbForWheels.Append(string.Format("wheel number {0}: {1}", i, wheel));
                i++;
            }

            return string.Format("{0}, type Of energy car takes: {1}\n, percentage left in car {2}:\n wheel info \n {3}",
                ToString(), 
                GetEnergyType(),
                m_Energy.GetEnergyPercentage(),
                sbForWheels);
        }

        public override string ToString()
        {
            return string.Format(
                "Licence Number: {0}\n, Model {1}\n, name of owner: {2}\n, owner phone number {3}, status of vehicle {4}\n",
                r_LicenseNumber,
                r_Model,
                r_NameOfOwner,
                r_PhoneNumOfOwner,
                StatusOfVehicle);
        }
    }
}