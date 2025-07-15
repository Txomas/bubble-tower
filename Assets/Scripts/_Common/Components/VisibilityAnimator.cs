using System.Collections;
using UnityEngine;

namespace Common.Components
{
    public class VisibilityAnimator : MonoBehaviour
    {
        public void ShowInstantly()
        {
            gameObject.SetActive(true);
        }
        
        public void HideInstantly()
        {
            gameObject.SetActive(false);
        }
        
        public IEnumerator ShowCoroutine()
        {
            ShowInstantly();
            yield break;
        }
        
        public IEnumerator HideCoroutine()
        {
            HideInstantly();
            yield break;
        }
    }
}