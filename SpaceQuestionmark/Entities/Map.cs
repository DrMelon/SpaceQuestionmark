//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Mippidy map

namespace SpaceQuestionmark.Entities
{
    class Map : EntityEx
    {
        public Dictionary<Otter.Vector2, Floor> worldMap = new Dictionary<Otter.Vector2, Floor>();
        public List<Floor> currentDrawables = new List<Floor>();
        public int curRenderX = 12220;
        public int curRenderY = 12220;

        public Map(int w, int h)
        {
            for(int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Entities.Floor newFloor = CreateFloorTile(i, j);
                    worldMap.Add(new Otter.Vector2(i, j), newFloor); // do worldgen here
                }
            }
        }

        public override void Added()
        {
            base.Added();

         
        }

        public void CycleRenderables(int x, int y)
        {
            if(x == curRenderX && y == curRenderY)
            {
                return;
            }
            curRenderX = x;
            curRenderY = y;

            foreach(Entities.Floor floor in currentDrawables)
            {
                floor.markRemove = true;
            }

            currentDrawables.Clear();

            for(int i = x - 32; i < x + 32; i++)
            {
                for (int j = y - 32; j < y + 32; j++)
                {
                    if(i < 0 || j < 0)
                    {
                        continue;
                    }

                    Floor floor;
                    worldMap.TryGetValue(new Otter.Vector2(i, j), out floor);
                    if(floor != null)
                    {
                        currentDrawables.Add(floor);
                        floor.markRemove = false;
                        Scene.Add(floor);
                    }
                    else
                    {
                        Otter.Util.Log("No tile at " + i.ToString() + ", " + j.ToString());
                    }
                    
                }
            }



        }

        public void RemoveMarked()
        {
            foreach (var kvp in worldMap)
            {
                if (kvp.Value.IsInScene && kvp.Value.markRemove)
                {
                    Scene.Remove(kvp.Value);
                }
            }
        }

        public Floor CreateFloorTile(int x, int y)
        {
            Entities.Floor newFloor = new Floor(x, y);
            newFloor.X = x * 64;
            newFloor.Y = y * 64;
            newFloor.AddGraphic(new Otter.Image(Assets.GFX_TILE));
            newFloor.Graphic.CenterOrigin();
            newFloor.AddCollider(new Otter.BoxCollider(64, 64, (int)Global.ColliderTags.FLOOR));
            newFloor.Collider.CenterOrigin();

            return newFloor;
        }
    }
}
