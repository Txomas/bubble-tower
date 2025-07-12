using System;
using UnityEngine;

namespace Game.Core
{
    [Serializable]
    public class PlayerProgressModel
    {
        [field: SerializeField] public int Level { get; private set; }
    }
}