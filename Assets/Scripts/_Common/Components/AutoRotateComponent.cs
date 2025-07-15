using UnityEngine;

namespace Common.Components
{
    public class AutoRotateComponent : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = -200f;
        
        private void Update()
        {
            transform.Rotate(0f, 0f, _rotationSpeed * Time.unscaledDeltaTime);
        }
    }
}