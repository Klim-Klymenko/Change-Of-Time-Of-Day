using UnityEngine;

namespace Application.Installation
{
    [System.Serializable]
    internal struct ColourData
    {
        [field: SerializeField]
        internal string Time { get; private set; }

        [field: SerializeField]
        internal Color Colour { get; private set; }
    }
}