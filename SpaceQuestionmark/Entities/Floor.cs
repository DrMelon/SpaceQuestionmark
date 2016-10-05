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

        Tilemap floorMap;
        GridCollider floorCol;
        Tilemap wallMap;
        GridCollider wallCol;

        public int playerStartX;
        public int playerStartY;

        public Floor()
        {
            floorMap = new Tilemap(Assets.GFX_FLOORTILES, 256 * 64, 64);
            floorCol = new GridCollider(256 * 64, 256 * 64, 64, 64, (int)Global.ColliderTags.FLOOR);
            wallMap = new Tilemap(Assets.GFX_WALLTILES, 256 * 64, 64);
            wallCol = new GridCollider(256 * 64, 256 * 64, 64, 64, (int)Global.ColliderTags.WALL);

            GenerateMap();

            AddGraphic(floorMap);
            AddCollider(floorCol);
            AddGraphic(wallMap);
            AddCollider(wallCol);
        }

        public void GenerateMap()
        {
            // Map

            // A WHAM BAM BOODLY TIME TO MAKE A SPACE SHIBOODLY

            // Pick a player spawn point, and gen from there.
            playerStartX = Rand.Int(10, 245);
            playerStartY = Rand.Int(10, 245);

            // Make a lil starting pod room around the player here.
            MakeRoom(playerStartX, playerStartY, 6, 6, 2);

        }

        public void MakeRoom(int cx, int cy, int w, int h, int floortile)
        {
            FloorFillRect(cx - w / 2, cy - h / 2, w, h, floortile);
            FloorLineRect(cx - w / 2, cy - h / 2, w, h, 1);
            WallLineRect(cx - w / 2, cy - h / 2, w, h, 1);
        }


        public void FloorFillTile(int x, int y, int tileID)
        {
            floorMap.SetTile(x, y, tileID);
            floorCol.SetTile(x, y, true);
        }

        public void FloorClearTile(int x, int y)
        {
            floorMap.SetTile(x, y, 0);
            floorCol.SetTile(x, y, false);
        }

        public void FloorFillRect(int x, int y, int w, int h, int tileID)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    FloorFillTile(i, j, tileID);
                }
            }
        }

        public void FloorLineRect(int x, int y, int w, int h, int tileID)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    if (i == x || i == x + w - 1 || j == y || j == y + h - 1)
                    {
                        FloorFillTile(i, j, tileID);
                    }
                }
            }
        }

        public void FloorClearRect(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    FloorClearTile(i, j);
                }
            }
        }

        public void WallFillTile(int x, int y, int tileID)
        {
            wallMap.SetTile(x, y, tileID);
            wallCol.SetTile(x, y, true);
        }

        public void WallClearTile(int x, int y)
        {
            wallMap.SetTile(x, y, 0);
            wallCol.SetTile(x, y, false);
        }

        public void WallFillRect(int x, int y, int w, int h, int tileID)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    WallFillTile(i, j, tileID);
                }
            }
        }

        public void WallLineRect(int x, int y, int w, int h, int tileID)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    if (i == x || i == x + w - 1 || j == y || j == y + h - 1)
                    {
                        WallFillTile(i, j, tileID);
                    }
                }
            }
        }

        public void WallClearRect(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    WallClearTile(i, j);
                }
            }
        }

    }
}
