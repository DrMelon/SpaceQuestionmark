//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// This is used to let a human (or human-like creature) move and interact with the world.

namespace SpaceQuestionmark.Components
{
    class MobMovement : Component
    {
        public enum MoveType
        {
            WALK,
            RUN,
            SLIDE,
            FALL,
            BOOSTED
        }

        public Speed myMotion = new Speed(1200);
        public Speed myVelocity = new Speed(1200);

        public float walkSpeed = 20;
        public float runSpeed = 40;
        public float slideSpeed = 60;
        public float fallSpeed = 120;
        public float boostSpeed = 120;

        public Speed myAcceleration = new Speed(12);

        public float myGroundFriction = 0.80f;
        public float mySpaceFriction = 1.0f;

        public Entities.EntityEx myEnt;

        public ControllerXbox360 myController;

        public MoveType currentMoveType = MoveType.WALK;

        public override void Added()
        {
            base.Added();

            myEnt = Entity as Entities.EntityEx;
            myVelocity.HardClamp = true;

            
        }

        public void HandleInput(float dt)
        {
            float mvSpd = walkSpeed;
            
            if (myController.B.Down && (currentMoveType == MoveType.RUN || currentMoveType == MoveType.WALK))
            {
                currentMoveType = MoveType.RUN;
                mvSpd = runSpeed;
            }
            else if (currentMoveType == MoveType.RUN)
            {
                currentMoveType = MoveType.WALK;
            }

            if (Math.Abs(myController.LeftStick.X) > 0.1f)
            {
                myMotion.X = myController.LeftStick.X * mvSpd;
            }
            else
            {
                myMotion.X = 0.0f;
            }

            if (Math.Abs(myController.LeftStick.Y) > 0.1f)
            {
                myMotion.Y = myController.LeftStick.Y * mvSpd;
            }
            else
            {
                myMotion.Y = 0.0f;
            }

        }

        public override void Update()
        {
            base.Update();
            float dt = Systems.Time.GetDeltaTime(myEnt.myTimeGroup);

            switch (currentMoveType)
            {
                case MoveType.WALK:
                    myMotion.Max = walkSpeed;
                    break;

                case MoveType.RUN:
                    myMotion.Max = runSpeed;
                    break;

                case MoveType.SLIDE:
                    myMotion.Max = slideSpeed;
                    break;

                case MoveType.FALL:
                    myMotion.Max = fallSpeed;
                    break;

                case MoveType.BOOSTED:
                    myMotion.Max = boostSpeed;
                    break;
            }

            if(myEnt is Entities.Human)
            {
                Entities.Human myhuman = myEnt as Entities.Human;
                if(myhuman.IsAlive)
                {
                    HandleInput(dt);
                }
                else
                {
                    myMotion.X = 0;
                    myMotion.Y = 0;
                }

            }
            else
            {
                HandleInput(dt);
            }
            

            if(currentMoveType != MoveType.FALL)
            {
                myVelocity.X += myMotion.X * dt;
                myVelocity.Y += myMotion.Y * dt;
            }
            

            if (Math.Abs(myAcceleration.X) < 0.1f || Math.Abs(myAcceleration.Y) < 0.1f)
            {
                float alterFriction = 1.0f;
                switch (currentMoveType)
                { 
                    case MoveType.WALK:
                        alterFriction = myGroundFriction;
                    break;

                    case MoveType.RUN:
                        alterFriction = myGroundFriction;
                        break;

                    case MoveType.SLIDE:
                        alterFriction = myGroundFriction * 2.0f;
                        if (alterFriction > 1.0f)
                        {
                            alterFriction = 1.0f;
                        }
                        break;

                    case MoveType.FALL:
                        alterFriction = mySpaceFriction;
                        break;

                    case MoveType.BOOSTED:
                        alterFriction = mySpaceFriction;
                        break;
                }

                if(Math.Abs(myAcceleration.X) < 0.01f)
                {
                    myVelocity.X *= alterFriction;
                }
                if (Math.Abs(myAcceleration.Y) < 0.01f)
                {
                    myVelocity.Y *= alterFriction;
                }

            }

            if(myEnt.IsCollideWith<Entities.Floor>(myVelocity.X, myVelocity.Y, (int)Global.GetColliderTagForType<Entities.Wall>()))
            {
                if (currentMoveType != MoveType.FALL)
                {
                    if (myEnt.IsCollideWith<Entities.Floor>(myVelocity.X, 0, (int)Global.GetColliderTagForType<Entities.Wall>()))
                    {
                        myVelocity.X = 0.0f;
                    }
                    if (myEnt.IsCollideWith<Entities.Floor>(0, myVelocity.Y, (int)Global.GetColliderTagForType<Entities.Wall>()))
                    {
                        myVelocity.Y = 0.0f;
                    }
                }
                else
                {
                    // Do grip check. If grip, halt veloc and walk a little.


                    // If no grip, bounce off of the structure (and take damage?)
                    if (myEnt.IsCollideWith<Entities.Floor>(myVelocity.X, 0, (int)Global.GetColliderTagForType<Entities.Wall>()))
                    {
                        myVelocity.X *= -0.96f;
                    }
                    if (myEnt.IsCollideWith<Entities.Floor>(0, myVelocity.Y, (int)Global.GetColliderTagForType<Entities.Wall>()))
                    {
                        myVelocity.Y *= -0.96f;
                    }
                }
            }

            myEnt.X += myVelocity.X;
            myEnt.Y += myVelocity.Y;

            if(!myEnt.IsCollideWith<Entities.Floor>(0, 0, (int)Global.GetColliderTagForType<Entities.Floor>()))
            {
                currentMoveType = MoveType.FALL;
            }
            else if(currentMoveType != MoveType.WALK && currentMoveType != MoveType.RUN)
            {
                currentMoveType = MoveType.WALK;
            }

        }

        public override void Render()
        {
            base.Render();

            if(Global.debugMode)
            {
                // Render Sticks.

                // LS
                Draw.Circle(myEnt.X - 60, myEnt.Y + 60, 32, Color.None, Color.Custom("FaintBlue"), 1);
                Draw.Circle(myEnt.X - 60 + (32 * myController.LeftStick.X), myEnt.Y + 60 + (32 * myController.LeftStick.Y), 8, myController.LeftStickClick.Down ? Color.Custom("FaintYellow") : Color.None, Color.Custom("FaintYellow"), 1);

                // RS
                Draw.Circle(myEnt.X + 60, myEnt.Y + 60, 32, Color.None, Color.Custom("FaintBlue"), 1);
                Draw.Circle(myEnt.X + 60 + (32 * myController.RightStick.X), myEnt.Y + 60 + (32 * myController.RightStick.Y), 8, myController.RightStickClick.Down ? Color.Custom("FaintYellow") : Color.None, Color.Custom("FaintYellow"), 1);

                // A B X Y
                Draw.Circle(myEnt.X + 90, myEnt.Y + 30, 8, myController.X.Down? Color.Custom("FaintBlue") : Color.None, Color.Custom("FaintBlue"), 1);
                Draw.Circle(myEnt.X + 105, myEnt.Y + 45, 8, myController.A.Down ? Color.Custom("FaintGreen") : Color.None, Color.Custom("FaintGreen"), 1);
                Draw.Circle(myEnt.X + 120, myEnt.Y + 30, 8, myController.B.Down ? Color.Custom("FaintRed") : Color.None, Color.Custom("FaintRed"), 1);
                Draw.Circle(myEnt.X + 105, myEnt.Y + 15, 8, myController.Y.Down ? Color.Custom("FaintYellow") : Color.None, Color.Custom("FaintYellow"), 1);

                // Bumps n trigg
                Draw.Rectangle(myEnt.X - 80 - 8, myEnt.Y + 10 - 4, 16, 8, myController.LT.Down ? Color.Custom("FaintMagenta") : Color.None, Color.Custom("FaintMagenta"), 1);
                Draw.Rectangle(myEnt.X - 80 - 8, myEnt.Y + 20 - 4, 16, 8, myController.LB.Down ? Color.Custom("FaintCyan") : Color.None, Color.Custom("FaintCyan"), 1);

                Draw.Rectangle(myEnt.X + 80 - 8, myEnt.Y + 10 - 4, 16, 8, myController.RT.Down ? Color.Custom("FaintMagenta") : Color.None, Color.Custom("FaintMagenta"), 1);
                Draw.Rectangle(myEnt.X + 80 - 8, myEnt.Y + 20 - 4, 16, 8, myController.RB.Down ? Color.Custom("FaintCyan") : Color.None, Color.Custom("FaintCyan"), 1);
            }
        }
    }
}
