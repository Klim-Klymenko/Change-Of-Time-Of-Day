using System;
using UnityEngine;
using GameEngine.DayFeature;

namespace Application.Installation
{
    [Serializable]
    internal struct LightningData
    {
        [field: SerializeField]
        internal Light Light { get; private set; }
        
        [field: SerializeField]
        internal DayTime DayTime { get; private set; }
    }
}