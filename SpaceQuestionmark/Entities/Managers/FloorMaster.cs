//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// The FLOORMASTER holds a dictionary of all floor pieces in a map. Whoa!
// ain't that neat??

// it also does all the rendering and collisions but nbd i guess lmao



namespace SpaceQuestionmark.Entities.Managers
{
    public class FloorMaster : Entity
    {
        Dictionary<int, Floor> floorMap; // the floormap!! wueuw
        Image stitchedImage;
        GridCollider collisionMap;

        public int GetHashForCoordinate(int x, int y)
        {
            Vector2 hash = new Vector2(x, y);
            return hash.GetHashCode();
        }

        public void AddFloorTile(Floor tile, int tileX, int tileY)
        {
            floorMap.AddOrUpdate(GetHashForCoordinate(tileX, tileY), tile);
        }

        public void RemoveFloorTile(int tileX, int tileY)
        {
            floorMap.Remove(GetHashForCoordinate(tileX, tileY));
        }

        public void RemoveFloorTile(Floor tile)
        {
            floorMap.Remove(GetHashForCoordinate(tile.myTileX, tile.myTileY));
        }

        public Floor GetTile(int tileX, int tileY)
        {
            Floor outValue = null;

            floorMap.TryGetValue(GetHashForCoordinate(tileX, tileY), out outValue);

            return outValue;
        }
        
    }
}
