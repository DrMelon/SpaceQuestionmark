using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceQuestionmark.Systems.Chemistry
{
    public static class MasterList
    {

        // REAGENTS
        public static Reagent Water = new Reagents.Water();
        public static Reagent Ice = new Reagents.Ice();
        public static Reagent HumanBlood = new Reagents.HumanBlood();

        // RECIPES
        public static Recipe RecipeWaterFreeze;

        

        public static void PopulateRecipes()
        {
            RecipeWaterFreeze = new Recipes.WaterFreeze((Reagents.Water)Water, (Reagents.Ice)Ice);
        }

    }
}
