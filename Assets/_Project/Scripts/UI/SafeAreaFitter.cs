using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        private void Awake()
        {
            var rectTransform = (RectTransform)transform;
            Rect safeArea = Screen.safeArea;
            var screenSize = new Vector2(Screen.width, Screen.height);

            rectTransform.anchorMin = safeArea.min / screenSize;
            rectTransform.anchorMax = safeArea.max / screenSize;
        }
    }
}
