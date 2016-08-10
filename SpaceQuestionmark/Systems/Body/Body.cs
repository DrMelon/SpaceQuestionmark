//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// A Body is made of Body Parts!
// Living things need Bodies. If the Body runs out of vitality, it dies! :(
// Vitality is calculated by checking the vitality of body parts, and their weights.

namespace SpaceQuestionmark.Systems.Body
{
    public class Body
    {
        public Entities.EntityEx Owner = null;
        public Dictionary<Bodypart, float> myBodyParts;

        public float currentVitality = 100.0f;
        public float maxVitality = 100.0f;
        public bool dead = false;

        public void CalculateVitality()
        {
            if (myBodyParts.Count < 1)
            {
                // DEAD O:
                currentVitality = 0.0f;
                Die();
                return;
            }

            float maxVitality = 0.0f;
            float runningVitalityTotal = 0.0f;

            foreach(KeyValuePair<Bodypart, float> kvp in myBodyParts)
            {
                runningVitalityTotal += kvp.Key.myVitality * kvp.Value;
                maxVitality += 100.0f * kvp.Value;
            }

            float overallVitality = runningVitalityTotal / maxVitality;

            currentVitality = overallVitality * 100.0f;
        }

        public void AddBodypart(Bodypart toadd, float weight)
        {
            myBodyParts.Add(toadd, weight);
            toadd.Owner = this;
        }

        public Dictionary<Bodypart, float> GetExternalBodyParts()
        {
            Dictionary<Bodypart, float> parts = new Dictionary<Bodypart, float>();

            foreach(KeyValuePair<Bodypart, float> part in myBodyParts)
            {
                if(part.Key.internalOrgan == false)
                {
                    parts.Add(part.Key, part.Value);
                }
            }

            return parts;
        }

        public Dictionary<Bodypart, float> GetInternalBodyParts()
        {
            Dictionary<Bodypart, float> parts = new Dictionary<Bodypart, float>();

            foreach (KeyValuePair<Bodypart, float> part in myBodyParts)
            {
                if (part.Key.internalOrgan == true)
                {
                    parts.Add(part.Key, part.Value);
                }
            }

            return parts;
        }

        public void Die()
        {
            dead = true;
        }

        public void CheckBleed()
        {
            foreach (KeyValuePair<Bodypart, float> kvp in myBodyParts)
            {
                kvp.Key.CheckBleed();
            }
        }

        public void DoBleed(Bodypart part)
        {
            // Create reagent in world at owner position
            if(part.bleedReagent != null)
            {
                // ChemistryUtils.CreateReagentInWorld(Owner.X, Owner.Y, bleedReagent)
            }
        }
    }
}
