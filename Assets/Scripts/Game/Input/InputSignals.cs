using UnityEngine;

namespace Game.Input
{
    public struct PointerTappedSignal
    {
        public Vector2 ScreenPosition { get; }
        
        public PointerTappedSignal(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }
    
    public struct PointerMovedSignal
    {
        public Vector2 ScreenPosition { get; }
        public Vector2 Delta { get; }
        
        public PointerMovedSignal(Vector2 screenPosition, Vector2 delta)
        {
            ScreenPosition = screenPosition;
            Delta = delta;
        }
    }
    
    public struct MoveSignal
    {
        public Vector2 Direction { get; }
        
        public MoveSignal(Vector2 direction)
        {
            Direction = direction;
        }
    }
    
    public struct ScrollSignal
    {
        public float Delta { get; }
        
        public ScrollSignal(float delta)
        {
            Delta = delta;
        }
    }
    
    public struct NextClickedSignal
    {
    }
    
    public struct PreviousClickedSignal
    {
    }
    
    public struct SecondaryClickedSignal
    {
        public Vector2 ScreenPosition { get; }
        
        public SecondaryClickedSignal(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }
}