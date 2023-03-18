using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class Touchpad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public float smoothing;

        private Vector2 _direction;
        private Vector2 _origin;
        private int _pointerID;
        private Vector2 _smoothDirection;
        private bool _touched;

        public void OnDrag(PointerEventData data)
        {
            if (data.pointerId == _pointerID)
            {
                var directionRaw = data.position - _origin;
                _direction = directionRaw.normalized;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (!_touched)
            {
                _touched = true;
                _pointerID = data.pointerId;
                _origin = data.position;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == _pointerID)
            {
                _direction = Vector2.zero;
                _touched = false;
            }
        }

        public Vector2 GetDirection()
        {
            _smoothDirection = Vector2.MoveTowards(_smoothDirection, _direction, smoothing);
            return _smoothDirection;
        }
    }
}