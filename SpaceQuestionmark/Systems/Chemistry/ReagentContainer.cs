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
//
// Containers hold stuff! Wow!
// They're part of items, usually.
// They can resolve recipes! And stuff!
//
//
//
//
//

namespace SpaceQuestionmark.Systems.Chemistry
{
    class ReagentContainer
    {
        public int Capacity;
        public int CurrentFluidAmt;
        public Dictionary<Reagent, int> CurrentReagents;

        public int GetReagentAmount(Reagent reagent)
        {
            if(CurrentReagents.ContainsKey(reagent))
            {
                return CurrentReagents[reagent];
            }

            return 0;
        }

        public void AddReagent(Reagent reagent, int amount)
        {
            if(amount >= 0)
            {
                if(amount > Capacity - CurrentFluidAmt)
                {
                    amount = Capacity - CurrentFluidAmt;
                }
                else
                {
                    // Couldn't put any in! Maybe tell user.
                }

                int reagentAmount = GetReagentAmount(reagent);
                if(reagentAmount > 0)
                {
                    CurrentReagents[reagent] += amount;
                    CurrentFluidAmt += amount;
                }
                else
                {
                    CurrentReagents.Add(reagent, amount);
                    CurrentFluidAmt += amount;
                }         
                      
            }
        }

        public void RemoveReagent(Reagent reagent, int amount)
        {
            if (amount >= 0)
            {
                int reagentAmount = GetReagentAmount(reagent);
                if (reagentAmount > 0)
                {
                    if(amount > reagentAmount)
                    {
                        amount = reagentAmount;
                    }

                    CurrentReagents[reagent] -= amount;
                    CurrentFluidAmt -= amount;

                    if (CurrentReagents[reagent] <= 0)
                    {
                        CurrentReagents.Remove(reagent);
                    }
                }
            }
        }

        public void AddMultiple(Dictionary<Reagent, int> input)
        {
            foreach(KeyValuePair<Reagent, int> reagentAdd in input)
            {
                AddReagent(reagentAdd.Key, reagentAdd.Value);
            }
        }

        public void RemoveMultiple(Dictionary<Reagent, int> input)
        {
            foreach (KeyValuePair<Reagent, int> reagentAdd in input)
            {
                RemoveReagent(reagentAdd.Key, reagentAdd.Value);
            }
        }

        // Takes x units of fluid, taking from overall composition
        public ReagentContainer TakeSome(int amount, bool justSampling = true)
        {
            ReagentContainer outReagents = new ReagentContainer();

            // Get the proportion of total fluid each reagent contributes, rounding up.
            // Then take that much from the container and put it into the output, if not sampling
            foreach(KeyValuePair<Reagent, int> currentReagent in CurrentReagents)
            {
                float currentReagentProportion = (float)currentReagent.Value / (float)CurrentFluidAmt;
                currentReagentProportion = (float)Math.Ceiling(currentReagentProportion);

                // Add to outgoing list
                outReagents.AddReagent(currentReagent.Key, (int)currentReagentProportion);

                // Destroy if not sampling
                if(!justSampling)
                {
                    RemoveReagent(currentReagent.Key, (int)currentReagentProportion);
                }
            }

            return outReagents;
        }

        public void TransferSome(int amount, ReagentContainer targetContainer)
        {
            ReagentContainer taken = TakeSome(amount, false);

            targetContainer.AddMultiple(taken.CurrentReagents);            
        }

        public bool SatisfiesRecipe(Recipe recipeTarget)
        {
            // Check we have the requisite reagents
            bool hasAllReagents = true;
            bool hasEnoughReagents = true;
            foreach(KeyValuePair<Reagent, int> recipeReagent in recipeTarget.InputReagents)
            {
                if(!CurrentReagents.ContainsKey(recipeReagent.Key))
                {
                    hasAllReagents = false;
                }
                else
                {
                    if(CurrentReagents[recipeReagent.Key] >= recipeReagent.Value)
                    {
                        hasEnoughReagents = false;
                    }
                }
            }

            if(hasEnoughReagents && hasAllReagents)
            {
                return true;
            }

            return false;
        }

        // Double, double toil and trouble
        // Fire burn, and cauldron bubble
        public void ResolveRecipe(Recipe recipeTarget)
        {
            while (SatisfiesRecipe(recipeTarget))
            {
                RemoveMultiple(recipeTarget.InputReagents);
                AddMultiple(recipeTarget.OutputReagents);
            }
        }


    }
}
