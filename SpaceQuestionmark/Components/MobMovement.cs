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


        public Speed myVelocity = new Speed(1200);

        public float walkSpeed = 12;
        public float runSpeed = 18;
        public float slideSpeed = 60;
        public float fallSpeed = 120;
        public float boostSpeed = 120;

        public Speed myAcceleration = new Speed(32);

        public float myGroundFriction = 0.85f;
        public float mySpaceFriction = 1.0f;

        public Entities.EntityEx myEnt;

        public ControllerXbox360 myController;

        public MoveType currentMoveType = MoveType.WALK;

        public override void Added()
        {
            base.Added();

            myEnt = Entity as Entities.EntityEx;
        }

        public void HandleInput(float dt)
        {
            myAcceleration.X = myController.LeftStick.X;
            myAcceleration.Y = myController.LeftStick.Y;

        }

        public override void Update()
        {
            base.Update();
            float dt = Systems.Time.GetDeltaTime(myEnt.myTimeGroup);

            switch (currentMoveType)
            {
                case MoveType.WALK:
                    myVelocity.Max = walkSpeed;
                    break;

                case MoveType.RUN:
                    myVelocity.Max = runSpeed;
                    break;

                case MoveType.SLIDE:
                    myVelocity.Max = slideSpeed;
                    break;

                case MoveType.FALL:
                    myVelocity.Max = fallSpeed;
                    break;

                case MoveType.BOOSTED:
                    myVelocity.Max = boostSpeed;
                    break;
            }

            HandleInput(dt);

            myVelocity.X += myAcceleration.X * dt;
            myVelocity.Y += myAcceleration.Y * dt;

            if (Math.Abs(myAcceleration.X) < 0.01f && Math.Abs(myAcceleration.Y) < 0.01f)
            {
                switch (currentMoveType)
                { 
                    case MoveType.WALK:
                        myVelocity.X *= myGroundFriction;
                        myVelocity.Y *= myGroundFriction;
                    break;

                    case MoveType.RUN:
                        myVelocity.X *= myGroundFriction;
                        myVelocity.Y *= myGroundFriction;
                        break;

                    case MoveType.SLIDE:
                        myVelocity.X *= myGroundFriction * 0.5f;
                        myVelocity.Y *= myGroundFriction * 0.5f;
                        break;

                    case MoveType.FALL:
                        myVelocity.X *= mySpaceFriction;
                        myVelocity.Y *= mySpaceFriction;
                        break;

                    case MoveType.BOOSTED:
                        myVelocity.X *= mySpaceFriction;
                        myVelocity.Y *= mySpaceFriction;
                        break;
                }
            }

            myEnt.X += myVelocity.X;
            myEnt.Y += myVelocity.Y;
        }
    }
}
