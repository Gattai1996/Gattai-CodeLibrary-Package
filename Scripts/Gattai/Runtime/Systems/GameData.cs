using System;
using System.Collections.Generic;

namespace Gattai.Runtime.Systems
{
    [Serializable]
    public class GameData
    {
        public int unlockedLevels = 1;
        public int currentLevelIndex;
        public int coins;
        public int gems;
    }
}