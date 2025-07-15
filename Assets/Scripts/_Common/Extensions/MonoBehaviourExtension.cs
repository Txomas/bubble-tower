using UnityEngine;

namespace Common.Extensions
{
    public static class MonoBehaviourExtension
    {
        public static void SafeDestroy(this MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour != null)
            {
                Object.Destroy(monoBehaviour.gameObject);
            }
        }
        
        public static void SetActive(this MonoBehaviour monoBehaviour, bool value)
        {
            monoBehaviour.gameObject.SetActive(value);
        }
        
        public static void Show(this MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(true);
        }
        
        public static void Hide(this MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(false);
        }
        
        public static bool IsActiveSelf(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.gameObject.activeSelf;
        }
        
        public static bool IsActiveInHierarchy(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.gameObject.activeInHierarchy;
        }
    }
}