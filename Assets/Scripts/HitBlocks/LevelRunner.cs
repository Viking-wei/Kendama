using System.Collections.Generic;
using UnityEngine;

namespace HitBlockLevels
{
    public class LevelRunner : MonoBehaviour
    {
        public int BPM = 100;
        public int LevelBeats = 100;
        private float _currentPlayBeats;
        public List<LevelBlockData> LevelBlocks;
    }
}