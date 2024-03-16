using UnityEngine;

namespace GetMikyled.LevelEditor
{
    ///-//////////////////////////////////////////////////////////
    ///
    public static class ExtensionMethods
    {
        ///-//////////////////////////////////////////////////////////
        ///
        public static float Round(this float f)
        {
            return Mathf.Round(f);
        }

        ///-//////////////////////////////////////////////////////////
        ///
        /// Rounds to snap to varying grid sizes
        ///
        public static float Round(this float f, float size)
        {
            return (f / size).Round() * size;
        }
    }

}