using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraFitter : MonoBehaviour
    {
        [SerializeField] private float _referenceAspect = 9f / 16f;

        private void Awake()
        {
            var camera = GetComponent<Camera>();

            if (camera.aspect >= _referenceAspect)
                return;

            float halfHeight = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float fittedHalfHeight = halfHeight * _referenceAspect / camera.aspect;

            camera.fieldOfView = Mathf.Atan(fittedHalfHeight) * 2f * Mathf.Rad2Deg;
        }
    }
}
