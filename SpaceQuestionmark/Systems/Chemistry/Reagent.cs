//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using SpaceQuestionmark.Entities;

// IIIIIT'S CHEMISTRY TIME
//
// ok so: reagents can react with eachother based on recipes
// they can also react while being processed when they're touching
// items, living, walls, or floors
// or have been ingested by living, or airbone on living
//




namespace SpaceQuestionmark.Systems.Chemistry
{

    class Reagent
    {
        public static String myName = "Genericium";
        public static String myDescription = "The most inert chemical in the universe.";

        public void ProcessExternal(Entity other)
        {

        }

        public void ProcessIngested(Living other)
        {

        }

        public void ProcessAirborne(Living other)
        {

        }

    }
}
