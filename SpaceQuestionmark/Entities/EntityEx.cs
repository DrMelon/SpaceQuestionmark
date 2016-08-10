//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// Extended entity class!

namespace SpaceQuestionmark.Entities
{
    public class EntityEx : Entity
    {
        public string myName = "Entity";
        public string myDescription = "Amazing.";


        // YOOO MY EXTENDED METHODS SONNN - DrMelon 2016
        public bool IsCollideWith<T>(float dx, float dy, int colliderTag) where T : EntityEx
        {
            bool didCollide = false;
            didCollide = Overlap(X + dx, Y + dy, colliderTag);
            didCollide = (Overlapped != this);

            if (didCollide)
            {
                DoCollideWith<T>((T)Overlapped);
                ((T)Overlapped).DoCollidedBy(this);
            }

            return didCollide;
        }

        public void DoCollideWith<T>(T other)
        {

        }

        public void DoCollidedBy<T>(T other)
        {

        }

        public void UsedBy<T>(T other)
        {

        }
    }
}
