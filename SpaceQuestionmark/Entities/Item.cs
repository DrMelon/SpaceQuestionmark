//
// @Author: J Brown (@DrMelon)
// 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// An Item is a kind of Entity which can be picked up and moved around by the player.
// It's pretty cool!

namespace SpaceQuestionmark.Entities 
{
    public class Item : Entity
    {
        // Fields
        public String myName = "Perfectly Generic Item";
        public String myDescription = "This is a Perfectly Generic Item, and it's Perfectly Boring! Wow!";
        public Speed mySpeed = new Speed(150);
        public List<Systems.Time.TimeGroup> timeGroups = new List<Systems.Time.TimeGroup>();

        public float myFriction = 0.92f;
        public float myBounciness = 0.95f;
        

        // Methods
        public Item(int x, int y)
        {
            X = x;
            Y = y;
            Layer = 30;

            AddCollider(new CircleCollider(8, (int)Global.ColliderTags.ITEM));
            Collider.CenterOrigin();

            timeGroups.Add(Systems.Time.TimeGroup.DEFAULT);
            timeGroups.Add(Systems.Time.TimeGroup.WORLDTHINK);
        }

        public bool IsTouchingFloor()
        {
            return IsCollideWith<Floor>(0, 0, (int)Global.GetColliderTagForType<Floor>());
        }
        
        public bool IsBounceCollide(float dx, float dy)
        {
            return (IsCollideWith<Wall>(dx, dy, (int)Global.GetColliderTagForType<Wall>()) || 
                    IsCollideWith<Machine>(dx, dy, (int)Global.GetColliderTagForType<Machine>()) ||
                    IsCollideWith<Living>(dx, dy, (int)Global.GetColliderTagForType<Living>()));
        }

        public void DoOnMoved(float prevX, float prevY, float newX, float newY)
        {

        }

        public void Move()
        {
            //If we're on the floor, make sure to have friction.
            float dt = Systems.Time.GetCumulativeDeltaTime(timeGroups);

            if (IsTouchingFloor())
            {
                mySpeed.X *= myFriction;
                mySpeed.Y *= myFriction;
            }   

            if (IsBounceCollide(mySpeed.X, 0))
            {
                mySpeed.X *= -myBounciness;
            }
            if (IsBounceCollide(0, mySpeed.Y))
            {
                mySpeed.Y *= -myBounciness;
            }

            // Check item collisions
            IsCollideWith<Item>(mySpeed.X, mySpeed.Y, (int)Global.GetColliderTagForType<Item>());

            X += mySpeed.X * dt;
            Y += mySpeed.Y * dt;

            DoOnMoved(X - (mySpeed.X * dt), Y - (mySpeed.Y * dt), X, Y);
        }
        
    }
}
