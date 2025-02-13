using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Util
{
    /// <summary>
    /// Detects and processes drag events in the world space using the main camera.
    /// Invokes Unity events for drag start, drag update, and drag end.
    /// </summary>
    public class WorldDragDetector : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;

        [SerializeField] private UnityEvent<Vector3> _onDragStart;
        [SerializeField] private UnityEvent<Vector3> _onDragUpdate;
        [SerializeField] private UnityEvent<Vector3> _onDragEnd;

        private bool _dragging = false;

        /// <summary>
        /// Checks for input and updates drag events accordingly.
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _onDragStart.Invoke(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
                _dragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _onDragEnd.Invoke(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
                _dragging = false;
                return;
            }

            if (!_dragging) return;
            
            _onDragUpdate.Invoke(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}