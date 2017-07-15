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
    public enum Zone
    {
        SPACE,
        ACCESS,
        CREW,
        MEDICAL,
        SCIENCE,
        ENGINEERING,
        ATMOSPHERICS,
        STORAGE,
        LOGISTICS,
        COMMS,
        SECURITY,
        SHUTTLEPORT,
        BOTANY,

        COUNT
    }

    public struct Room
    {
        public int x, y, w, h;
        public Zone zone;
    }

    public class BSPLeaf
    {
        public const int MinLeafSize = 14;
        public int x, y, w, h;
        public BSPLeaf lChild = null;
        public BSPLeaf rChild = null;

        public bool Split()
        {
            if(lChild != null || rChild != null)
            {
                return false;
            }

            // Initial split will be random
            bool splitH = Otter.Rand.Bool;

            // Split based on roomsize
            if(w > h && (float)w / (float)h >= 1.25)
            {
                splitH = false;
            }
            else if(h > w && (float)h / (float)w >= 1.25)
            {
                splitH = true;
            }

            // check min size
            int maxSize = (splitH ? h : w) - MinLeafSize;

            if(maxSize <= MinLeafSize)
            {
                return false;
            }

            // find point along axis to split
            int splitPoint = Otter.Rand.Int(MinLeafSize, maxSize);

            // do split
            if(splitH)
            {
                lChild = new BSPLeaf() { x=x, y=y, w=w, h=splitPoint };
                rChild = new BSPLeaf() { x = x, y = y+splitPoint, w = w, h = h-splitPoint };
            }
            else
            {
                lChild = new BSPLeaf() { x = x, y = y, w = splitPoint, h = h };
                rChild = new BSPLeaf() { x = x + splitPoint, y = y, w = w - splitPoint, h = h };
            }

            return true;

        }


    }





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
        public List<Room> roomsMade = new List<Room>();

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
            myGasManager.relatedFloor = this;
            GenerateMap();

            AddGraphic(floorMap);
            AddCollider(floorCol);
            AddGraphic(wallMap);
            AddCollider(wallCol);

            
        }

        public void GenerateMap()
        {

            // New, BSP-style zone stuff!

            // first things first, let's organize the outer zones.
            

            // the whole map size is 256*256, so let's border 64 tiles off each size as space.
            roomsMade.Add(new Room() { x = 0, y = 0, w = 256, h = 64, zone = Zone.SPACE } );
            roomsMade.Add(new Room() { x = 256 - 64, y = 0, w = 64, h = 256, zone = Zone.SPACE });
            roomsMade.Add(new Room() { x = 0, y = 256 - 64, w = 256, h = 64, zone = Zone.SPACE });
            roomsMade.Add(new Room() { x = 0, y = 0, w = 64, h = 256, zone = Zone.SPACE });

            // let's make a list of necessary rooms for a station to work
            List<Zone> requiredZones = new List<Entities.Zone>(){ Zone.ENGINEERING, Zone.SHUTTLEPORT, Zone.ATMOSPHERICS };


            // now let's recursively split the remaining space up
            BSPLeaf startLeaf = new BSPLeaf() { x = 65, y = 65, w = 256-65, h = 265-65 };

            List<BSPLeaf> leaves = new List<BSPLeaf>();
            leaves.Add(startLeaf);

            bool splitCorrectly = true;

            while(splitCorrectly)
            {
                splitCorrectly = false;
                List<BSPLeaf> newleaves = new List<BSPLeaf>();
                foreach(BSPLeaf leaf in leaves)
                {
                    if(leaf.Split())
                    {
                        newleaves.Add(leaf.lChild);
                        newleaves.Add(leaf.rChild);
                        splitCorrectly = true;
                    }
                }
                leaves.Add(newleaves.ToArray());
            }

            // now only get the lowest-layer of leaves
            leaves.RemoveAll(leaf => (leaf.lChild != null || leaf.rChild != null));

            // trim out based on radial selection?
            leaves.RemoveAll(leaf => (Math.Abs((128 - leaf.x)) * Math.Abs((128 - leaf.x)) + Math.Abs((128 - leaf.y)) * Math.Abs((128 - leaf.y))) > 100*100);

            // now we got leaves!! assign them zones, making sure the required zones get in.
            if (leaves.Count < requiredZones.Count)
            {
                Util.Log("UH OH!!!!");
            }

            

            foreach(Zone reqZone in requiredZones)
            {
                // pick a random leaf from the leaves and pop it
                BSPLeaf leafPick = leaves.ElementAt(Otter.Rand.Int(0, leaves.Count));

                leaves.Remove(leafPick);

                roomsMade.Add(new Entities.Room() { x = leafPick.x, y = leafPick.y, w = leafPick.w, h = leafPick.h, zone = reqZone });
            }

            // Now do the same for remaining leaves but with random zones
            foreach(BSPLeaf leaf in leaves)
            {
                roomsMade.Add(new Entities.Room() { x = leaf.x, y = leaf.y, w = leaf.w, h = leaf.h, zone = (Zone)Otter.Rand.Int(0, (int)Zone.COUNT) });
            }


            // Whew!! now we have rooms made from zones.

            // Next step is to physically fill out each room with sub-rooms based on the zone type.
            foreach(Room room in roomsMade)
            {
                MakePhysicalRoom(room);
            }

            // Then we do adjacency, airlocks, doors.

            // Then we do major devices for the sub-rooms.

            // Then we do wiring, lights, atmospherics, logistics, comms connections.

            // Then we place items in each sub-room based on the zone/sub-room .

            // Then we place mobs!

            // Then we place the player (todo: safely)
            while(GetZoneAtPixCoords(playerStartX*64, playerStartY*64) == Zone.SPACE || !IsFloorAt(playerStartX,playerStartY))
            {
                playerStartX = Rand.Int(64, 256-64);
                playerStartY = Rand.Int(64, 256-64);
            }
            
           

            // And finally, we kickstart atmos
            FindSpaceTiles();

            myGasManager.NeedToRecalculateNeighbours = true;

            // OLD



            // Map
            // wave-function gen
            //Generate.DoTheDance();

            /*

            // A WHAM BAM BOODLY TIME TO MAKE A SPACE SHIBOODLY
            List<Room> roomsMade = new List<Room>();

            // Pick a player spawn point, and gen from there.
            playerStartX = Rand.Int(10, 245);
            playerStartY = Rand.Int(10, 245);

            // Make a lil starting pod room around the player here.
            MakeRoom(playerStartX, playerStartY, 6, 6, 2);
            roomsMade.Add(new Room() { x=playerStartX, y=playerStartY, w=6, h=6 });

            // Now make an asston of rooms
            int maxRooms = 50;

            for(int i = 0; i < 50; i++)
            {
                int roomMaxWidth = 20;
                int roomMinWidth = 2;
                int roomWidth = Rand.Int(roomMinWidth, roomMaxWidth);

                int roomMaxHeight = 20;
                int roomMinHeight = 2;
                int roomHeight = Rand.Int(roomMinHeight, roomMaxHeight);

                int xpos = Rand.Int(1, 254);
                int ypos = Rand.Int(1, 254);

                if(CheckSpaceForRoom(xpos, ypos, roomWidth, roomHeight))
                {
                    MakeRoom(xpos, ypos, roomWidth, roomHeight, 2);
                    roomsMade.Add(new Room() { x = xpos, y = ypos, w = roomWidth, h = roomHeight });
                }
            }

            // Now that rooms are made, tunnels gotta connect em.
            for(int i = 0; i < roomsMade.Count-1; i+=2)
            {
                Room roomA = roomsMade[i];
                Room roomB = roomsMade[i+1];

                // make a tunnel from room A to room B.
                int xWidth = roomB.x - roomA.x;
                int xDir = Math.Sign(xWidth);
                xWidth = Math.Abs(xWidth);

                int yWidth = roomB.y - roomA.y;
                int yDir = Math.Sign(yWidth);
                yWidth = Math.Abs(yWidth);
                

                if(xDir < 0)
                {
                    FloorFillRect(roomB.x, roomA.y, xWidth, 1, 1, true, true);
                    WallFillRect(roomB.x - 1, roomA.y + 1, xWidth + 2, 1, 1, true);
                    WallFillRect(roomB.x - 1, roomA.y - 1, xWidth + 2, 1, 1, true);
                }
                else
                {
                    FloorFillRect(roomA.x, roomA.y, xWidth+1, 1, 1, true, true);
                    WallFillRect(roomA.x - 1, roomA.y + 1, xWidth + 2, 1, 1, true);
                    WallFillRect(roomA.x - 1, roomA.y - 1, xWidth + 2, 1, 1, true);
                }

                if(yDir < 0)
                {
                    FloorFillRect(roomB.x, roomB.y, 1, yWidth, 1, true, true);
                    WallFillRect(roomB.x + 1, roomB.y - 1, 1, yWidth + 2, 1, true);
                    WallFillRect(roomB.x - 1, roomB.y - 1, 1, yWidth + 2, 1, true);
                }
                else
                {
                    FloorFillRect(roomB.x, roomA.y, 1, yWidth+1, 1, true, true);
                    WallFillRect(roomB.x + 1, roomA.y - 1, 1, yWidth + 2, 1, true);
                    WallFillRect(roomB.x - 1, roomA.y - 1, 1, yWidth + 2, 1, true);
                }



            }

            // Find borders to space and create negative pressure nodes
            FindSpaceTiles();

            myGasManager.NeedToRecalculateNeighbours = true;

        */
        }


        public void MakePhysicalRoom(int cx, int cy, int w, int h, int floortile)
        {
            FloorFillRect(cx - w / 2, cy - h / 2, w, h, floortile, true);
            FloorLineRect(cx - w / 2, cy - h / 2, w, h, 1, true);
            WallLineRect(cx - w / 2, cy - h / 2, w, h, 1);
        }

        public void MakePhysicalRoom(Room room)
        {
            switch (room.zone)
            {
                case Zone.SPACE:
                    break;

                default:
                    FloorFillRect(room.x+2, room.y+2, room.w-4, room.h-4, 2, true);
                    FloorLineRect(room.x+2, room.y+2, room.w-4, room.h-4, 1, true);
                    WallLineRect(room.x+2, room.y+2, room.w-4, room.h-4, 1);
                    break;
            }

            
        }

        public bool CheckSpaceForRoom(int cx, int cy, int w, int h)
        {
            bool spaceAvailable = true;

            for(int i = cx - w/2; i < cx + w/2; i++)
            {
                for(int j = cy - h/2; j < cy + w/2; j++)
                {
                    if(IsFloorAt(i, j) || IsWallAt(i, j))
                    {
                        spaceAvailable = false;
                    }
                    if(j < 0 || j > 255 || i < 0 || i > 255)
                    {
                        spaceAvailable = false;
                    }
                }
            }

            return spaceAvailable;
        }

        public bool IsFloorAt(int x, int y)
        {
            return floorMap.GetTile(x, y) != null && floorMap.GetTileIndex(floorMap.GetTile(x, y)) != 0;
        }

        public bool IsWallAt(int x, int y)
        {
            return wallMap.GetTile(x, y) != null && wallMap.GetTileIndex(wallMap.GetTile(x, y)) != 0;
        }

        public void FloorFillTile(int x, int y, int tileID, bool air = false, bool zapwalls = false)
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

            if(zapwalls)
            {
                WallClearTile(x, y);
            }
            
        }

        public void FloorClearTile(int x, int y)
        {
            floorMap.SetTile(x, y, 0);
            floorCol.SetTile(x, y, false);
            myGasManager.SetGasMixture(x, y, null);
        }

        public void FloorFillRect(int x, int y, int w, int h, int tileID, bool air = false, bool zapwalls = false)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    FloorFillTile(i, j, tileID, air, zapwalls);
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

        public void WallFillTile(int x, int y, int tileID, bool spaceOnly = false)
        {
            if (!spaceOnly)
            {
                wallMap.SetTile(x, y, tileID);
                wallCol.SetTile(x, y, true);
            }
            else
            {
                if (!IsFloorAt(x, y) && !IsWallAt(x, y))
                {
                    wallMap.SetTile(x, y, tileID);
                    wallCol.SetTile(x, y, true);
                }
            }
        }

        public void WallClearTile(int x, int y)
        {
            wallMap.SetTile(x, y, 0);
            wallCol.SetTile(x, y, false);
        }

        public void WallFillRect(int x, int y, int w, int h, int tileID, bool spaceOnly = false)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    WallFillTile(i, j, tileID, spaceOnly);
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

        public Zone GetZoneAtPixCoords(int x, int y)
        {
            int tileX = x / 64;
            int tileY = y / 64;

            Room checkRoom = roomsMade[0];
            int roomsMadeIdx = 0;

            while((tileX < checkRoom.x || tileY < checkRoom.y) || (tileX > checkRoom.x + checkRoom.w || tileY > checkRoom.y + checkRoom.h))
            {
                roomsMadeIdx++;

                if(roomsMadeIdx == roomsMade.Count)
                {
                    return Zone.SPACE;
                }

                checkRoom = roomsMade[roomsMadeIdx];
            }

            return checkRoom.zone;
        }

        public override void Render()
        {
            base.Render();

            if(Global.debugMode)
            {
                myGasManager.DebugDraw();

                foreach(Room room in roomsMade)
                {
                    Color roomCol = Color.White;

                    switch (room.zone)
                    {
                        case Zone.ACCESS:
                            roomCol.R = 0.5f;
                            roomCol.G = 0.5f;
                            roomCol.B = 0.5f;
                            break;

                        case Zone.ATMOSPHERICS:
                            roomCol.R = 0.6f;
                            roomCol.G = 0.95f;
                            roomCol.B = 1.0f;
                            break;

                        case Zone.MEDICAL:
                            roomCol.R = 1.0f;
                            roomCol.G = 0.96f;
                            roomCol.B = 1.0f;
                            break;

                        case Zone.SHUTTLEPORT:
                            roomCol.R = 0.8f;
                            roomCol.G = 0.5f;
                            roomCol.B = 0.8f;
                            break;

                        case Zone.ENGINEERING:
                            roomCol.R = 1.0f;
                            roomCol.G = 0.85f;
                            roomCol.B = 0.0f;
                            break;

                        case Zone.STORAGE:
                            roomCol.R = 0.5f;
                            roomCol.G = 0.8f;
                            roomCol.B = 0.5f;
                            break;

                        case Zone.LOGISTICS:
                            roomCol.R = 0.8f;
                            roomCol.G = 0.5f;
                            roomCol.B = 0.5f;
                            break;

                        case Zone.BOTANY:
                            roomCol.R = 0.65f;
                            roomCol.G = 0.8f;
                            roomCol.B = 0.5f;
                            break;

                        case Zone.COMMS:
                            roomCol.R = 0.5f;
                            roomCol.G = 0.5f;
                            roomCol.B = 0.5f;
                            break;

                        default:
                            roomCol.R = 0.5f;
                            roomCol.G = 0.5f;
                            roomCol.B = 0.5f;
                            break;
                    }



                    roomCol.A = 0.5f;

                    if (room.zone != Zone.SPACE)
                    {
                        Draw.Rectangle(room.x * FLOOR_TILE_SIZE, room.y * FLOOR_TILE_SIZE, room.w * FLOOR_TILE_SIZE, room.h * FLOOR_TILE_SIZE, roomCol, roomCol * Color.Grey, 2);
                    }
                }

                Draw.Circle(128 * 64, 128 * 64, 100 * 64, Color.None ,Color.Cyan, 16);
            }
        }

        public void FindSpaceTiles()
        {
            // find borders to space and make negative pressure nodes there
            myGasManager.RemoveAllVoids();

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
