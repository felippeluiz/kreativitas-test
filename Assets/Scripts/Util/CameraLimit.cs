using UnityEngine;

namespace Util
{
    public class CameraLimit : MonoBehaviour
    {
        public static float GetCameraXLimit(bool isLeft)
        {
            var camera = Camera.main;
            if (camera == null) return 0f;

            float halfWidth = camera.orthographicSize * camera.aspect;
            float xPos = isLeft ? -halfWidth : halfWidth;
            
            return camera.transform.position.x + xPos;
        }
    }
}
