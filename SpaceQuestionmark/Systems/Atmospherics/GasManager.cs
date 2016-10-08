//
// @Author: J Brown (@DrMelon)
// 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace SpaceQuestionmark.Systems.Atmospherics
{
    public class GasManager
    {
        // Processes all the gases for the tiles!
        public Dictionary<Vector2, GasMixture> allGasMixtures = new Dictionary<Vector2, GasMixture>();
        public bool NeedToRecalculateNeighbours = true;

        public void SetGasMixture(int x, int y, GasMixture gm)
        {
            Vector2 vec = new Vector2(x, y);

            allGasMixtures.AddOrUpdate(vec, gm);
        }

        public void RemoveGasMixture(int x, int y)
        {
            Vector2 vec = new Vector2(x, y);

            allGasMixtures.AddOrUpdate(vec, null);
        }

        public GasMixture GetMixtureAt(int x, int y)
        {
            if(x < 0 || y < 0 || x > 255 || y > 255)
            {
                return null;
            }
            Vector2 vec = new Vector2(x, y);

            GasMixture outMix = null;
            if (allGasMixtures.TryGetValue(vec, out outMix))
            {
                return outMix;
            }

            return null;
        }

        public void CalculateNeighbours(int x, int y)
        {
            CalculateNeighbours(GetMixtureAt(x, y));
        }

        public Vector2 GetLocationForMixture(GasMixture gm)
        {
            Vector2 outvec = new Vector2(-1, -1);

            foreach(var kvp in allGasMixtures)
            {
                if(kvp.Value == gm)
                {
                    return kvp.Key;
                }
            }

            return outvec;
        }

        public void CalculateNeighbours(GasMixture gm, int in_x = -1, int in_y = 1)
        {
            int x = -1;
            int y = -1;
            if (in_x >= 0)
            {
                x = in_x;
                if (in_y >= 0)
                {
                    y = in_y;
                }
            }
            else
            {
                Vector2 vec = GetLocationForMixture(gm);
                x = (int)vec.X;
                y = (int)vec.Y;
            }






            if(gm != null)
            {
                GasMixture north = GetMixtureAt(x, y - 1);
                if(north != null)
                {
                    north.SouthNeighbour = gm;
                    gm.NorthNeighbour = north;

                    gm.RecountNeighbours = true;
                    north.RecountNeighbours = true;
                }

                GasMixture south = GetMixtureAt(x, y + 1);
                if (south != null)
                {
                    south.NorthNeighbour = gm;
                    gm.SouthNeighbour = south;

                    gm.RecountNeighbours = true;
                    south.RecountNeighbours = true;
                }

                GasMixture east = GetMixtureAt(x + 1, y);
                if (east != null)
                {
                    east.WestNeighbour = gm;
                    gm.EastNeighbour = east;

                    gm.RecountNeighbours = true;
                    east.RecountNeighbours = true;
                }

                GasMixture west = GetMixtureAt(x - 1, y);
                if (west != null)
                {
                    west.EastNeighbour = gm;
                    gm.WestNeighbour = west;

                    gm.RecountNeighbours = true;
                    west.RecountNeighbours = true;
                }
            }



        }

        public void Update(float dt)
        {
            if (NeedToRecalculateNeighbours)
            {
                foreach (var kvp in allGasMixtures)
                {
                    CalculateNeighbours((int)kvp.Key.X, (int)kvp.Key.Y);
                    kvp.Value.RecountNeighbours = true;
                }
                NeedToRecalculateNeighbours = false;
            }

            // update all gasmixtures
            foreach (var kvp in allGasMixtures)
            {
                kvp.Value.Update(dt);
            }


        }

        public void DebugDraw()
        {
            // draw all the gas vectors!
            foreach(var kvp in allGasMixtures)
            {
                Vector2 getPos = kvp.Key;
                GasMixture gm = kvp.Value;
                
                if (gm.GetPressure() < 0.1f)
                {
                    Draw.Circle((getPos.X * 64) + 32, (getPos.Y * 64) + 32, 4, Color.Red);
                }
                else
                {
                    //Draw.RoundedLine((getPos.X * 64) + 32, (getPos.Y * 64) + 32, ((getPos.X * 64) + 32) + gm.GasMotion.X * 1000, ((getPos.Y * 64) + 32) + gm.GasMotion.Y * 1000, Color.Red, 3);
                    Draw.Circle((getPos.X * 64) + 32, (getPos.Y * 64) + 32, 4, new Color(1.0f - gm.GetPressure(), gm.GetPressure(), 0.0f, 1.0f));
                }
            }
        }
    }
}
