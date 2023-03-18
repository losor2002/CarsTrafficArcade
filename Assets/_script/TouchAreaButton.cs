using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class TouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private int _pointerCount;

        public void OnPointerDown(PointerEventData data)
        {
            _pointerCount++;
        }

        public void OnPointerUp(PointerEventData data)
        {
            _pointerCount--;
        }

        public bool IsPressed()
        {
            return _pointerCount > 0;
        }
    }
}