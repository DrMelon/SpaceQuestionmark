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

        public Systems.Atmospherics.GasManager myGasManager = new Systems.Atmospherics.GasManager();

        public int playerStartX;
        public int playerStartY;

        public bool NeedUpdateSpace = false;

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

            // Find borders to space and create negative pressure nodes
            FindSpaceTiles();

            myGasManager.NeedToRecalculateNeighbours = true;
        }


        public void MakeRoom(int cx, int cy, int w, int h, int floortile)
        {
            FloorFillRect(cx - w / 2, cy - h / 2, w, h, floortile, true);
            FloorLineRect(cx - w / 2, cy - h / 2, w, h, 1, true);
            WallLineRect(cx - w / 2, cy - h / 2, w, h, 1);
        }

        public bool IsFloorAt(int x, int y)
        {
            return floorMap.GetTile(x, y) != null && floorMap.GetTileIndex(floorMap.GetTile(x, y)) != 0;
        }

        public bool IsWallAt(int x, int y)
        {
            return wallMap.GetTile(x, y) != null && wallMap.GetTileIndex(wallMap.GetTile(x, y)) != 0;
        }

        public void FloorFillTile(int x, int y, int tileID, bool air = false)
        {
            floorMap.SetTile(x, y, tileID);
            floorCol.SetTile(x, y, true);
            if(air)
            {
                Systems.Atmospherics.GasMixture gm = new Systems.Atmospherics.GasMixture();
                gm.AddGasChange("Oxygen", 0.2095f);
                gm.AddGasChange("Nitrogen", 0.7808f);
                gm.AddGasChange("CO2", 0.0040f);
                
                myGasManager.SetGasMixture(x, y, gm);
            }
            else
            {
                myGasManager.SetGasMixture(x, y, new Systems.Atmospherics.GasMixture());
            }
            
        }

        public void FloorClearTile(int x, int y)
        {
            floorMap.SetTile(x, y, 0);
            floorCol.SetTile(x, y, false);
            myGasManager.SetGasMixture(x, y, null);
        }

        public void FloorFillRect(int x, int y, int w, int h, int tileID, bool air = false)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    FloorFillTile(i, j, tileID, air);
                }
            }
        }

        public void FloorLineRect(int x, int y, int w, int h, int tileID, bool air = false)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    if (i == x || i == x + w - 1 || j == y || j == y + h - 1)
                    {
                        FloorFillTile(i, j, tileID, air);
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

        public override void Update()
        {
            base.Update();

            if(NeedUpdateSpace)
            {
                FindSpaceTiles();
            }

            myGasManager.Update(Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.WORLDTHINK));
        }

        public override void Render()
        {
            base.Render();

            if(Global.debugMode)
            {
                myGasManager.DebugDraw();
            }
        }

        public void FindSpaceTiles()
        {
            // find borders to space and make negative pressure nodes there
            
            // a border to space is when a tile is touching a non-tile and doesn't have a wall on it
            for(int x = 0; x < 256; x++)
            {
                for(int y = 0; y < 256; y++)
                {
                    // not a floor
                    if(!IsFloorAt(x, y))
                    {
                        continue;
                    }

                    // check neighbours
                    if(y - 1 < 0 || x - 1 < 0 || x + 1 > 255 || y + 1 > 255)
                    {
                        continue;
                    }

                    if(IsWallAt(x, y))
                    {
                        continue;
                    }

                    // north
                    if(!IsFloorAt(x, y - 1) && !IsWallAt(x, y - 1))
                    {
                        Systems.Atmospherics.GasMixture newGas = new Systems.Atmospherics.GasMixture();
                        newGas.Void = true;
                        newGas.AddGasChange("Oxygen", -1);
                        myGasManager.SetGasMixture(x, y - 1, newGas);
                        myGasManager.NeedToRecalculateNeighbours = true;
                    }
                    //south
                    if (!IsFloorAt(x, y + 1) && !IsWallAt(x, y + 1))
                    {
                        Systems.Atmospherics.GasMixture newGas = new Systems.Atmospherics.GasMixture();
                        newGas.Void = true;
                        newGas.AddGasChange("Oxygen", -1);
                        myGasManager.SetGasMixture(x, y + 1, newGas);
                        myGasManager.NeedToRecalculateNeighbours = true;
                    }
                    //west
                    if (!IsFloorAt(x - 1, y) && !IsWallAt(x - 1, y))
                    {
                        Systems.Atmospherics.GasMixture newGas = new Systems.Atmospherics.GasMixture();
                        newGas.Void = true;
                        newGas.AddGasChange("Oxygen", -1);
                        myGasManager.SetGasMixture(x - 1, y, newGas);
                        myGasManager.NeedToRecalculateNeighbours = true;
                    }
                    //east
                    if (!IsFloorAt(x + 1, y) && !IsWallAt(x + 1, y))
                    {
                        Systems.Atmospherics.GasMixture newGas = new Systems.Atmospherics.GasMixture();
                        newGas.Void = true;
                        newGas.AddGasChange("Oxygen", -1);
                        myGasManager.SetGasMixture(x + 1, y, newGas);
                        myGasManager.NeedToRecalculateNeighbours = true;
                    }
                }
            }

            NeedUpdateSpace = false;
        }

    }
}
