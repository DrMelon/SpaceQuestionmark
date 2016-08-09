using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceQuestionmark.Systems.Chemistry.Recipes
{
    class WaterFreeze : Recipe
    {
        public WaterFreeze(Reagents.Water water, Reagents.Ice ice)
        {
            InputReagents = new Dictionary<Reagent, int>();
            OutputReagents = new Dictionary<Reagent, int>();

            InputReagents.Add(water, 1);
            OutputReagents.Add(ice, 1);
            TemperatureThreshold = 0;
            Freezes = true;
        }
    }
}
