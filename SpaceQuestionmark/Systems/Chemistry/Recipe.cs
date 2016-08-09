//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// IIIIIT'S CHEMISTRY TIME
//
// RECIPES YO!!
//
// A recipe knows what reagents it needs to create another one
// and in what amounts ye ye
//
// all actual processing of recipes is done by containers
//

namespace SpaceQuestionmark.Systems.Chemistry
{
    class Recipe
    {
        public Dictionary<Reagent, int> InputReagents;
        public Dictionary<Reagent, int> OutputReagents;
        public bool Boils = false;
        public bool Freezes = false;
        public float TemperatureThreshold = 0;

        public void OnRecipeComplete()
        {

        }
    }
}
