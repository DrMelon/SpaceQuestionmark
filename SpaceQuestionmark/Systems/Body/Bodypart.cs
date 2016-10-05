//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceQuestionmark.Systems.Body
{
    public class Bodypart
    {
        public Body Owner = null;
        public Bodypart PartOwner = null;
        public string myName = "Body Part";
        public string myDescription = "This body part does the stuff. The good stuff!";
        public float thisVitality = 100.0f;
        public float myVitality { get { return GetVitality(); } set { thisVitality = value; } }
        public bool internalOrgan = false;
        public bool bleeding = false;
        public bool canEquip = false; // can we wear stuff on it?
        public bool canGrasp = false; // can it be used to get stuff?
        public Systems.Chemistry.Reagent bleedReagent = null;

        // body parts can have more parts inside them
        public Dictionary<Bodypart, float> myBodyParts = new Dictionary<Bodypart, float>();

        public float GetVitality()
        {
            if (myBodyParts.Count > 0)
            {
                float maxVitality = 0.0f;
                float runningVitalityTotal = 0.0f;

                foreach (KeyValuePair<Bodypart, float> kvp in myBodyParts)
                {
                    runningVitalityTotal += kvp.Key.myVitality * kvp.Value;
                    maxVitality += 100.0f * kvp.Value;
                }

                runningVitalityTotal += thisVitality;
                maxVitality += 100.0f;

                float overallVitality = runningVitalityTotal / maxVitality;

                return overallVitality * 100.0f;
            }
            else
            {
                return thisVitality;
            }
        }

        public void Hurt(float amt)
        {
            if (myBodyParts.Count > 0)
            {
                foreach (KeyValuePair<Bodypart, float> kvp in myBodyParts)
                {
                    kvp.Key.Hurt(amt * kvp.Value);
                    Global.DebugLog(myName + " Hurt: " + amt.ToString() + " ::: " + thisVitality.ToString() + " left");
                }
            }

            if(thisVitality > 0)
            {
                thisVitality -= amt;
                Global.DebugLog(myName + " Hurt: " + amt.ToString() + " ::: " + thisVitality.ToString() + " left");
            }
            if(thisVitality < 0)
            {
                thisVitality = 0;
            }
            

            
        }

        public void CheckBleed()
        {
            foreach (KeyValuePair<Bodypart, float> kvp in myBodyParts)
            {
                kvp.Key.CheckBleed();
            }

            if (bleeding)
            {
                if(myBodyParts.Count > 0)
                {
                    foreach (KeyValuePair<Bodypart, float> kvp in myBodyParts)
                    {
                        kvp.Key.Hurt(kvp.Value * 1.0f);
                    }
                }
                else
                {
                    Hurt(1.0f);
                }
                Owner.DoBleed(this);
            }
        }

        public void AddBodypart(Bodypart toadd, float weight)
        {
            myBodyParts.Add(toadd, weight);
            toadd.PartOwner = this;
            toadd.Owner = this.Owner;
        }
    }
}
