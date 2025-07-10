using UnityEngine;

namespace Game.Core.Bubbles
{
    public class BubbleView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        
        public Vector2Int GridIndex { get; set; }
        public Color Color
        {
            get => _renderer.material.color;
            set => _renderer.material.color = value;
        }
    }
}