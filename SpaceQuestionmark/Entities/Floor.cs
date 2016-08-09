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
    public class Floor : Entity
    {
        public String myName = "Perfectly Generic Floor";
        public String myDescription = "This is a Perfectly Generic Floor, and it's Perfectly Boring! Wow!";
        public const int FLOOR_TILE_SIZE = 64;
        public Managers.FloorMaster myFloorMaster; // all hail the mighty FLOORMASTER, that manages all the floors.
        public int myTileX, myTileY;

        public Floor(int tileX, int tileY, Managers.FloorMaster floormaster)
        {
            // Register us in the Book of Floors, it handles rendering and stuff for us, but maintains a ref to us so that we can
            // do special logic. 
            myTileX = tileX;
            myTileY = tileY;

            myFloorMaster = floormaster;
        }

        public void AddSelfToFloorMaster()
        {
            if(myFloorMaster.GetTile(myTileX, myTileY) == null)
            {
                myFloorMaster.AddFloorTile(this, myTileX, myTileY);
            }
            else
            {
                Util.Log("Can't add a tile that already exists, bruh");
            }
            
        }

        public void DoItemCollided(Item other)
        {

        }

    }
}
