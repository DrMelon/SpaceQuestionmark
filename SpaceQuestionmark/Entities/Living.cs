//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// LIVING THIIIIINGS!
// Living things have a body, which they update to make sure they aten't dead yet.

namespace SpaceQuestionmark.Entities
{
    public class Living : EntityEx
    {
        public string myName = "Living Creature";
        public string myDescription = "It's alive!";

        public Systems.Body.Body myBody = new Systems.Body.Body();

        public Living()
        {
            myBody.Owner = this;
        }

        public override void Update()
        {
            base.Update();

            if(myBody.dead)
            {
                Die();
            }
        }

        public void Die()
        {
            //erk
            myName = "Dead Creature";
            myDescription = "It's dead!";
        }
    }
}
