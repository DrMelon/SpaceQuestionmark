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
// An Atmospherics Processor

// they can Move, Use things, Equip things, etc


namespace SpaceQuestionmark.Entities
{
    class Human : Living
    {
        public string myName = "Normal Human";

        public Dictionary<string, string> myCostume = Assets.GFX_PLAYER_DEBUG_COSTUME;

        public Otter.Spritemap<string> myLegsSprite;
        public Otter.Spritemap<string> myTorsoSprite;
        public Otter.Spritemap<string> myArmsSprite;
        public Otter.Spritemap<string> myHeadSprite;

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

        // Atmospherics!!
        

        public Human()
        {
            // Aw yea i got dat bod
            SetUpBody();

            // Set up reagent container!
            myReagentContainer = new Systems.Chemistry.ReagentContainer();
            myReagentContainer.Capacity = 100;

            // Set up my clothes. Don't wanna be nakes!! (or invisible. lol)
            UpdateClothing();
        }

        public void UpdateClothing()
        {
            if (myLegsSprite != null)
            {
                myLegsSprite.SetTexture(myCostume["legs"]);
            }
            else
            {
                myLegsSprite = new Otter.Spritemap<string>(myCostume["legs"], 32, 32);
                myLegsSprite.Add("idle", new Otter.Anim(new int[] { 0 }, new float[] { 60f }));
                myLegsSprite.Add("run", new Otter.Anim(new int[] { 0, 1, 2, 3, 4, 5 }, new float[] { 6f, 6f, 6f, 6f, 6f, 6f }));
                myLegsSprite.Add("dead", new Otter.Anim(new int[] { 6 }, new float[] { 60f }));
                myLegsSprite.CenterOrigin();
                AddGraphic(myLegsSprite);
            }

            if (myTorsoSprite != null)
            {
                myTorsoSprite.SetTexture(myCostume["torso"]);
            }
            else
            {
                myTorsoSprite = new Otter.Spritemap<string>(myCostume["torso"], 32, 32);
                myTorsoSprite.Add("idle", new Otter.Anim(new int[] { 0 }, new float[] { 60f }));
                myTorsoSprite.Add("run", new Otter.Anim(new int[] { 0, 1, 2, 3, 4, 5 }, new float[] { 6f, 6f, 6f, 6f, 6f, 6f }));
                myTorsoSprite.Add("dead", new Otter.Anim(new int[] { 6 }, new float[] { 60f }));
                myTorsoSprite.CenterOrigin();
                AddGraphic(myTorsoSprite);
            }

            if (myArmsSprite != null)
            {
                myArmsSprite.SetTexture(myCostume["arms"]);
            }
            else
            {
                myArmsSprite = new Otter.Spritemap<string>(myCostume["arms"], 32, 32);
                myArmsSprite.Add("idle", new Otter.Anim(new int[] { 0 }, new float[] { 60f }));
                myArmsSprite.Add("run", new Otter.Anim(new int[] { 0, 1, 2, 3, 4, 5 }, new float[] { 6f, 6f, 6f, 6f, 6f, 6f }));
                myArmsSprite.Add("dead", new Otter.Anim(new int[] { 6 }, new float[] { 60f }));
                myArmsSprite.CenterOrigin();
                AddGraphic(myArmsSprite);
            }

            if (myHeadSprite != null)
            {
                myHeadSprite.SetTexture(myCostume["head"]);
            }
            else
            {
                myHeadSprite = new Otter.Spritemap<string>(myCostume["head"], 32, 32);
                myHeadSprite.Add("idle", new Otter.Anim(new int[] { 0 }, new float[] { 60f }));
                myHeadSprite.Add("run", new Otter.Anim(new int[] { 0, 1, 2, 3, 4, 5 }, new float[] { 6f, 6f, 6f, 6f, 6f, 6f }));
                myHeadSprite.Add("dead", new Otter.Anim(new int[] { 6 }, new float[] { 60f }));
                myHeadSprite.CenterOrigin();
                AddGraphic(myHeadSprite);
            }
        }

        public override void Update()
        {
            base.Update();

            // Do body stuff. gross
            myBody.CalculateVitality();

            if(!CanBreathe())
            {
                // Can't hold breath for that long..!
                Lungs.Hurt(Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.WORLDTHINK) * 0.25f);
            }

            if(Lungs.GetVitality() < 10)
            {
                // Lungs are dead. Can't live long like this!
                Torso.Hurt(Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.WORLDTHINK) * 0.5f);
            }

            if (Heart.GetVitality() < 10)
            {
                // Heart is dead. Can't live long like this!
                Torso.Hurt(Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.WORLDTHINK) * 0.5f);
            }

            if (Guts.GetVitality() < 10)
            {
                // Guts are dead. Can't live long like this!
                Torso.Hurt(Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.WORLDTHINK) * 0.5f);
            }

            if (Brain.GetVitality() < 10)
            {
                // Brain is dead. We're pretty much insta-dead.
                Torso.Hurt(Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.WORLDTHINK) * 5.0f);
            }

            // Move about a bit
            if (!myBody.dead)
            {
                if (Global.controllerPlayerOne.RightStick.Position.Length > 0.1f)
                {
                    myLegsSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.RightStick.Position.Y, Global.controllerPlayerOne.RightStick.Position.X)) - 90;
                    myArmsSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.RightStick.Position.Y, Global.controllerPlayerOne.RightStick.Position.X)) - 90;
                    myHeadSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.RightStick.Position.Y, Global.controllerPlayerOne.RightStick.Position.X)) - 90;
                    myTorsoSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.RightStick.Position.Y, Global.controllerPlayerOne.RightStick.Position.X)) - 90;
                }
                else
                {
                    if (Global.controllerPlayerOne.LeftStick.Position.Length > 0.1f)
                    {
                        myLegsSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.LeftStick.Position.Y, Global.controllerPlayerOne.LeftStick.Position.X)) - 90;
                        myArmsSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.LeftStick.Position.Y, Global.controllerPlayerOne.LeftStick.Position.X)) - 90;
                        myHeadSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.LeftStick.Position.Y, Global.controllerPlayerOne.LeftStick.Position.X)) - 90;
                        myTorsoSprite.Angle = Otter.MathHelper.ToDegrees((float)Math.Atan2(-Global.controllerPlayerOne.LeftStick.Position.Y, Global.controllerPlayerOne.LeftStick.Position.X)) - 90;
                    }
                }

                if (Global.controllerPlayerOne.LeftStick.Position.Length > 0.1f)
                {
                    myLegsSprite.Play("run", false);
                    myArmsSprite.Play("run", false);
                    myHeadSprite.Play("run", false);
                    myTorsoSprite.Play("run", false);
                }
                else
                {
                    myLegsSprite.Play("idle", false);
                    myArmsSprite.Play("idle", false);
                    myHeadSprite.Play("idle", false);
                    myTorsoSprite.Play("idle", false);
                }
            }
        }

        public string GetDescription()
        {
            // This will fetch a description of the character, including what they're wearing/wielding.
            string description = "";
            description += "A human being. Averagely-sized curious people.";

            // Get Inventory/Equip

            return description;
        }

        public bool CanBreathe()
        {
            // check atmos processing
            return true && (Lungs.GetVitality() > 10);
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

            Torso.AddBodypart(Heart, 10.0f);
            Torso.AddBodypart(Lungs, 10.0f);
            Torso.AddBodypart(Guts, 10.0f);

            Head.AddBodypart(Eyes, 1.0f);
            Head.AddBodypart(Brain, 50.0f);

            myBody.AddBodypart(Torso, 1.0f);

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

        public override void Die()
        {
            myLegsSprite.Play("dead", false);
            myTorsoSprite.Play("dead", false);
            myArmsSprite.Play("dead", false);
            myHeadSprite.Play("dead", false);
            IsAlive = false;
        }
    }
}
