//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Time is organized into separate time groups!

namespace SpaceQuestionmark.Systems
{
    public static class Time
    {
        public enum TimeGroup
        {
            DEFAULT,
            UITHINK,
            UIRENDER,
            WORLDTHINK,
            WORLDRENDER
        }

        private static float deltaTimeDefaultScale = 1.0f;
        private static float deltaTimeUIThinkScale = 1.0f;
        private static float deltaTimeUIRenderScale = 1.0f;
        private static float deltaTimeWorldThinkScale = 1.0f;
        private static float deltaTimeWorldRenderScale = 1.0f;

        public static float GetDeltaTime(TimeGroup timeGroup = TimeGroup.DEFAULT)
        {
            float deltaTime = Otter.Game.Instance.RealDeltaTime;

            switch (timeGroup)
            {
                case TimeGroup.DEFAULT:
                    deltaTime *= deltaTimeDefaultScale;
                    break;
                case TimeGroup.UITHINK:
                    deltaTime *= deltaTimeUIThinkScale;
                    break;
                case TimeGroup.UIRENDER:
                    deltaTime *= deltaTimeUIRenderScale;
                    break;
                case TimeGroup.WORLDTHINK:
                    deltaTime *= deltaTimeWorldThinkScale;
                    break;
                case TimeGroup.WORLDRENDER:
                    deltaTime *= deltaTimeWorldRenderScale;
                    break;
            }

            return deltaTime;
        }

        public static float GetCumulativeDeltaTime(List<TimeGroup> timeGroups)
        {
            if(timeGroups.Count < 1)
            {
                return 1.0f;
            }

            float deltaCumulative = 1.0f;
            foreach(TimeGroup tg in timeGroups)
            {
                deltaCumulative *= GetDeltaTime(tg);
            }

            return deltaCumulative;
        }

        public static float GetAverageDeltaTime(List<TimeGroup> timeGroups)
        {
            if(timeGroups.Count < 1)
            {
                return 1.0f;
            }

            float deltaAverage = 1.0f;

            foreach (TimeGroup tg in timeGroups)
            {
                deltaAverage += GetDeltaTime(tg);
            }

            return deltaAverage / timeGroups.Count;
        }

    }

}
