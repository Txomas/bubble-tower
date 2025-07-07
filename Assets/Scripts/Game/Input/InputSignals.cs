using UnityEngine;

namespace Game.Input
{
    public struct PointerMovedSignal
    {
        public Vector2 ScreenPosition { get; }
        
        public PointerMovedSignal(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }
}