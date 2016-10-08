//
// @Author: J Brown (@DrMelon)
// 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace SpaceQuestionmark.Systems.Atmospherics
{
    public class GasMixture
    {
        public Dictionary<string, float> PresentGases = new Dictionary<string, float>();
        public Dictionary<string, float> DeferredGasChanges = new Dictionary<string, float>();

        public GasMixture NorthNeighbour = null;
        public GasMixture EastNeighbour = null;
        public GasMixture WestNeighbour = null;
        public GasMixture SouthNeighbour = null;
        float numNeighbours;
        public bool RecountNeighbours = true;
        public bool Void = false;

        // Temp in C
        public float Temperature = 0.0f;

        // Motion of gases
        public Vector2 GasMotion = new Vector2(0, 0);

        public float GetPressure()
        {
            if(Void)
            {
                return 0;
            }
            float pressure = 0.0f;

            foreach(var kvp in PresentGases)
            {
                pressure += kvp.Value;
            }

            return pressure;
        }

        public float GetGasPercentage(string gasToCheck)
        {
            float gasAmt = 0.0f;

            float amtInMix = 0.0f;

            if(PresentGases.TryGetValue(gasToCheck, out amtInMix))
            {
                gasAmt = amtInMix / GetPressure();
            }

            return gasAmt;
        }

        public void AddGasChange(string gastype, float amt)
        {
            float prevAmt = 0.0f;

            if(DeferredGasChanges.TryGetValue(gastype, out prevAmt))
            {
                DeferredGasChanges.AddOrUpdate(gastype, prevAmt + amt);
            }
            else
            {
                DeferredGasChanges.AddOrUpdate(gastype, amt);
            }
        }

        public void ProcessNeighbourCells(float dt)
        {
            dt *= 10.0f;

            // no neighbours
            if(numNeighbours < 1)
            {
                return;
            }

            // get our pressure
            GasMotion = new Vector2(0, 0);
            float CurrentPressure = GetPressure();

            // ok, now!! check the neighbours' pressure levels.
            if(NorthNeighbour != null)
            {
                float northPressure = NorthNeighbour.GetPressure();
                if (northPressure < CurrentPressure)
                {
                    float pressureDiff = ((CurrentPressure - northPressure));
                    //pressureDiff *= pressureDiff * 10;
                    foreach (var kvp in PresentGases)
                    {
                        NorthNeighbour.AddGasChange(kvp.Key, Math.Min(pressureDiff * GetGasPercentage(kvp.Key) * dt, CurrentPressure * GetGasPercentage(kvp.Key) * dt));
                        AddGasChange(kvp.Key, -pressureDiff * GetGasPercentage(kvp.Key) * dt);
                    }
                    GasMotion.Y -= pressureDiff;
                }
                if(Temperature > NorthNeighbour.Temperature)
                {
                    Temperature = (Temperature + (NorthNeighbour.Temperature / (float)numNeighbours) / 2);
                }
                
            }

            if (WestNeighbour != null)
            {
                float westPressure = WestNeighbour.GetPressure();
                if (westPressure < CurrentPressure)
                {
                    float pressureDiff = ((CurrentPressure - westPressure));
                    //pressureDiff *= pressureDiff * 10;
                    foreach (var kvp in PresentGases)
                    {
                        WestNeighbour.AddGasChange(kvp.Key, Math.Min(pressureDiff * GetGasPercentage(kvp.Key) * dt, CurrentPressure * GetGasPercentage(kvp.Key) * dt));
                        AddGasChange(kvp.Key, -pressureDiff * GetGasPercentage(kvp.Key) * dt);
                    }
                    GasMotion.X -= pressureDiff;
                }
                if (Temperature > WestNeighbour.Temperature)
                {
                    Temperature = (Temperature + (WestNeighbour.Temperature / (float)numNeighbours) / 2);
                }
            }

            if (EastNeighbour != null)
            {
                float eastPressure = EastNeighbour.GetPressure();
                if (eastPressure < CurrentPressure)
                {
                    float pressureDiff = ((CurrentPressure - eastPressure));
                    //pressureDiff *= pressureDiff * 10;
                    foreach (var kvp in PresentGases)
                    {
                        EastNeighbour.AddGasChange(kvp.Key, Math.Min(pressureDiff * GetGasPercentage(kvp.Key) * dt, CurrentPressure * GetGasPercentage(kvp.Key) * dt));
                        AddGasChange(kvp.Key, -pressureDiff * GetGasPercentage(kvp.Key) * dt);
                    }
                    GasMotion.X += pressureDiff;
                }
                if (Temperature > EastNeighbour.Temperature)
                {
                    Temperature = (Temperature + (EastNeighbour.Temperature / (float)numNeighbours) / 2);
                }
            }

            if (SouthNeighbour != null)
            {
                float southPressure = SouthNeighbour.GetPressure();
                if (southPressure < CurrentPressure)
                {
                    float pressureDiff = ((CurrentPressure - southPressure));
                    //pressureDiff *= pressureDiff * 10;
                    foreach (var kvp in PresentGases)
                    {
                        SouthNeighbour.AddGasChange(kvp.Key, Math.Min(pressureDiff * GetGasPercentage(kvp.Key) * dt, CurrentPressure * GetGasPercentage(kvp.Key) * dt));
                        AddGasChange(kvp.Key, -pressureDiff * GetGasPercentage(kvp.Key) * dt);
                    }
                    GasMotion.Y += pressureDiff;
                }
                if(Temperature > SouthNeighbour.Temperature)
                {
                    Temperature = (Temperature + (SouthNeighbour.Temperature / (float)numNeighbours) / 2);
                }

            }
        }

        public void Update(float dt)
        {
            if(DeferredGasChanges.Count > 0)
            {
                foreach(var kvp in DeferredGasChanges)
                {
                    float prevAmt = 0.0f;
                    if (PresentGases.TryGetValue(kvp.Key, out prevAmt))
                    {
                        PresentGases.AddOrUpdate(kvp.Key, kvp.Value + prevAmt);
                    }
                    else
                    {
                        PresentGases.AddOrUpdate(kvp.Key, kvp.Value);
                    }
                }

                DeferredGasChanges.Clear();
                return; // do not update further if deferred changes happened.
            }

            // count the neighbours
            if (RecountNeighbours)
            {
                numNeighbours = 0;
                if (NorthNeighbour != null)
                {
                    numNeighbours++;
                }
                if (EastNeighbour != null)
                {
                    numNeighbours++;
                }
                if (WestNeighbour != null)
                {
                    numNeighbours++;
                }
                if (SouthNeighbour != null)
                {
                    numNeighbours++;
                }

                RecountNeighbours = false;
            }

            // if this is a void, like space, steadily reduce gas magnitudes
            if (Void)
            {
                PresentGases.Clear();                
                return;
            }

            // No gas or no neighbours? don't bother updating then, loser
            if (PresentGases.Count < 1 || numNeighbours < 1)
            {
                return;
            }



            // Aiiiight we can carry on then
            ProcessNeighbourCells(dt);
        }
    }
}
