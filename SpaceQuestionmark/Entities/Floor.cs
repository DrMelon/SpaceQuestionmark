//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// A floor is an entity. No way that's gonna lag like heck, rite?
// I'll do some optimisation for rendering & doing collisions, but we gotta
// set them up as entities I'm pretty sure.

namespace SpaceQuestionmark.Entities
{
    public class Floor : EntityEx
    {
        public String myName = "Perfectly Generic Floor";
        public String myDescription = "This is a Perfectly Generic Floor, and it's Perfectly Boring! Wow!";
        public const int FLOOR_TILE_SIZE = 64;
        public int myTileX, myTileY;
        public bool markRemove = false;

        public Floor()
        {

        }

    }
}
