//
// @Author: J Brown (@DrMelon)
// 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Humans!!
// they walk and they talk and they're kinda grubby i guess
// they each have:
// A Body
// An Inventory
// A Reagent Container

// they can Move, Use things, Equip things, etc


namespace SpaceQuestionmark.Entities
{
    class Human : Living
    {
        public string myName = "Normal Human";
        
        public string myDescription { get { return GetDescription(); } set { myDescription = value; } }

        // My organs!!
        public Systems.Body.Bodypart Torso;
        public Systems.Body.Bodypart Head;
        public Systems.Body.Bodypart LeftArm;
        public Systems.Body.Bodypart RightArm;
        public Systems.Body.Bodypart LeftLeg;
        public Systems.Body.Bodypart RightLeg;
        public Systems.Body.Bodypart LeftHand;
        public Systems.Body.Bodypart RightHand;
        public Systems.Body.Bodypart LeftFoot;
        public Systems.Body.Bodypart RightFoot;
        public Systems.Body.Bodypart Butt;
        public Systems.Body.Bodypart Eyes;
        public Systems.Body.Bodypart Mouth;
        public Systems.Body.Bodypart Brain;
        public Systems.Body.Bodypart Heart;
        public Systems.Body.Bodypart Lungs;
        public Systems.Body.Bodypart Guts;

        // Reagent container yooo
        public Systems.Chemistry.ReagentContainer myReagentContainer;

        public Human()
        {
            // Aw yea i got dat bod
            SetUpBody();

            // Set up reagent container!
            myReagentContainer = new Systems.Chemistry.ReagentContainer();
            myReagentContainer.Capacity = 100;

        }

        public override void Update()
        {
            base.Update();

            // Move about a bit
        }

        public string GetDescription()
        {
            // This will fetch a description of the character, including what they're wearing/wielding.
            string description = "";
            description += "A human being. Averagely-sized curious people.";

            // Get Inventory/Equip

            return description;
        }

        public void SetUpBody()
        {
            myBody = new Systems.Body.Body();
            myBody.Owner = this;

            // Define body parts - this is important, because many of the actions a human takes
            // depend on the status of one or more organs!!

            Torso = new Systems.Body.Bodypart();
            Torso.myName = "Torso";
            Torso.myDescription = "The chunky middle bit of a human.";
            Torso.bleedReagent = Systems.Chemistry.MasterList.HumanBlood;

            Head = new Systems.Body.Bodypart();
            Head.myName = "Head";
            Head.myDescription = "The knobbly bit on top of a human.";

            LeftLeg = new Systems.Body.Bodypart();
            LeftLeg.myName = "Left Leg";
            LeftLeg.myDescription = "The left leg of a human.";

            RightLeg = new Systems.Body.Bodypart();
            RightLeg.myName = "Right Leg";
            RightLeg.myDescription = "The right leg of a human.";

            LeftArm = new Systems.Body.Bodypart();
            LeftArm.myName = "Left Arm";
            LeftArm.myDescription = "The left arm of a human.";

            RightArm = new Systems.Body.Bodypart();
            RightArm.myName = "Right Arm";
            RightArm.myDescription = "The right arm of a human.";

            LeftHand = new Systems.Body.Bodypart();
            LeftHand.myName = "Left Hand";
            Torso.myDescription = "The weird appendage humans have that lets them hold things. This one is the left one.";

            RightHand = new Systems.Body.Bodypart();
            RightHand.myName = "Right Hand";
            RightHand.myDescription = "The weird appendage humans have that lets them hold things. This one is the right one.";

            LeftFoot = new Systems.Body.Bodypart();
            LeftFoot.myName = "Left Foot";
            LeftFoot.myDescription = "The bottom, walking bit of a human. This is the left one.";

            RightFoot = new Systems.Body.Bodypart();
            RightFoot.myName = "Right Foot";
            RightFoot.myDescription = "The bottom, walking bit of a human. This is the right one.";

            Eyes = new Systems.Body.Bodypart();
            Eyes.myName = "Eyes";
            Eyes.myDescription = "The two weird spheres that let a human like, see things or whatever.";
            Eyes.internalOrgan = true;

            Brain = new Systems.Body.Bodypart();
            Brain.myName = "Brain";
            Brain.myDescription = "Kinda important for humans. Dunno why.";
            Brain.internalOrgan = true;

            Mouth = new Systems.Body.Bodypart();
            Mouth.myName = "Mouth";
            Mouth.myDescription = "The loud bit of a human.";
            Mouth.internalOrgan = true;

            Heart = new Systems.Body.Bodypart();
            Heart.myName = "Heart";
            Heart.myDescription = "The blood pump thingy inside a human.";
            Heart.internalOrgan = true;

            Lungs = new Systems.Body.Bodypart();
            Lungs.myName = "Lungs";
            Lungs.myDescription = "The breathing things for a human.";
            Lungs.internalOrgan = true;

            Guts = new Systems.Body.Bodypart();
            Guts.myName = "Guts";
            Guts.myDescription = "The gurgly bits of a human.";
            Guts.internalOrgan = true;

            Butt = new Systems.Body.Bodypart();
            Butt.myName = "Butt";
            Butt.myDescription = "The farty bit of a human.";
            Butt.internalOrgan = true; // not strictly true, but we don't want it appearing on the equip descrip. ;0

            // Attach parts!
            Torso.AddBodypart(Head, 0.5f);
            Torso.AddBodypart(LeftArm, 0.2f);
            Torso.AddBodypart(RightArm, 0.2f);
            Torso.AddBodypart(LeftLeg, 0.2f);
            Torso.AddBodypart(RightLeg, 0.2f);
            Torso.AddBodypart(Butt, 0.2f);

            LeftArm.AddBodypart(LeftHand, 0.5f);
            RightArm.AddBodypart(RightHand, 0.5f);

            LeftLeg.AddBodypart(LeftFoot, 0.5f);
            RightLeg.AddBodypart(RightFoot, 0.5f);

            Torso.AddBodypart(Heart, 1.0f);
            Torso.AddBodypart(Lungs, 1.0f);
            Torso.AddBodypart(Guts, 1.0f);

            Head.AddBodypart(Eyes, 1.0f);
            Head.AddBodypart(Brain, 5.0f);

            // Define part abilities!
            LeftHand.canGrasp = true;
            RightHand.canGrasp = true;

            Torso.canEquip = true;
            LeftHand.canEquip = true;
            RightHand.canEquip = true;

            LeftFoot.canEquip = true;
            RightFoot.canEquip = true;

            Head.canEquip = true;
        }
    }
}
