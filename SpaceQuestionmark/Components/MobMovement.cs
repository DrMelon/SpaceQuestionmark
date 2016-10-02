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

        public float walkSpeed = 3;
        public float runSpeed = 12;
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
            if (Math.Abs(myController.LeftStick.X) > 0.1f)
            {
                myAcceleration.X = myController.LeftStick.X * 320;
            }
            else
            {
                myAcceleration.X = 0.0f;
            }

            if (Math.Abs(myController.LeftStick.Y) > 0.1f)
            {
                myAcceleration.Y= myController.LeftStick.Y * 320;
            }
            else
            {
                myAcceleration.Y = 0.0f;
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

            HandleInput(dt);

            myMotion.X += myAcceleration.X * dt;
            myMotion.Y += myAcceleration.Y * dt;

            myVelocity.X = myMotion.X * Math.Abs(myController.LeftStick.X);
            myVelocity.Y = myMotion.Y * Math.Abs(myController.LeftStick.Y);

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
                    myMotion.X *= alterFriction;
                }
                if (Math.Abs(myAcceleration.Y) < 0.01f)
                {
                    myMotion.Y *= alterFriction;
                }


            }

            while(myEnt.IsCollideWith<Entities.Wall>(myVelocity.X, myVelocity.Y, (int)Global.GetColliderTagForType<Entities.Wall>()))
            {
                myVelocity.X *= 0.5f;
                myVelocity.Y *= 0.5f;
            }

            myEnt.X += myVelocity.X;
            myEnt.Y += myVelocity.Y;
        }
    }
}
