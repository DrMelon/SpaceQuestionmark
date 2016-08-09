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
    class Item : Entity
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
            return IsFloorCollide(X, Y);
        }

        public bool IsLivingCollide(float dx, float dy)
        {
            bool didCollide = false;
            didCollide = Overlap(X + dx, Y + dy, (int)Global.ColliderTags.LIVING);

            if(didCollide)
            {
                DoCollideWithLiving((Living)Overlapped);
                ((Living)Overlapped).DoItemCollided(this);
            }
            

            return didCollide;
        }

        public bool IsMachineCollide(float dx, float dy)
        {
            bool didCollide = false;
            didCollide = Overlap(X + dx, Y + dy, (int)Global.ColliderTags.MACHINE);

            if (didCollide)
            {
                DoCollideWithMachine((Machine)Overlapped);
                ((Machine)Overlapped).DoItemCollided(this);
            }

            return didCollide;
        }

        public bool IsWallCollide(float dx, float dy)
        {
            bool didCollide = false;
            didCollide = Overlap(X + dx, Y + dy, (int)Global.ColliderTags.WALL);

            if (didCollide)
            {
                DoCollideWithWall((Wall)Overlapped);
                ((Wall)Overlapped).DoItemCollided(this);
            }

            return didCollide;
        }

        public bool IsItemCollide(float dx, float dy)
        {
            bool didCollide = false;
            didCollide = Overlap(X + dx, Y + dy, (int)Global.ColliderTags.ITEM);
            didCollide = (Overlapped != this);

            if (didCollide)
            {
                DoCollideWithItem((Item)Overlapped);
                ((Item)Overlapped).DoItemCollided(this);
            }

            return didCollide;
        }

        public bool IsFloorCollide(float dx, float dy)
        {
            bool didCollide = false;
            didCollide = Overlap(X + dx, Y + dy, (int)Global.ColliderTags.FLOOR);
            didCollide = (Overlapped != this);

            if (didCollide)
            {
                DoCollideWithFloor((Floor)Overlapped);
                ((Floor)Overlapped).DoItemCollided(this);
            }

            return didCollide;
        }


        public bool IsBounceCollide(float dx, float dy)
        {
            return (IsLivingCollide(dx, dy) || IsWallCollide(dx, dy) || IsMachineCollide(dx, dy) || IsItemCollide(dx, dy));
        }

        public void DoCollideWithLiving(Living other)
        {

        }

        public void DoCollideWithMachine(Machine other)
        {

        }

        public void DoCollideWithWall(Wall other)
        {

        }

        public void DoCollideWithItem(Item other)
        {

        }

        public void DoCollideWithFloor(Floor other)
        {

        }

        public void DoItemCollided(Item other)
        {

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

            X += mySpeed.X * dt;
            Y += mySpeed.Y * dt;

            DoOnMoved(X - (mySpeed.X * dt), Y - (mySpeed.Y * dt), X, Y);
        }
        
    }
}
