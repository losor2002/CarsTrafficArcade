using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class Touchpad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public float smoothing;
        public float horizontalDiscreteSensitivity;

        private Vector2 _direction;
        private int _horizontalDiscreteMovement;
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
                _horizontalDiscreteMovement = 0;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == _pointerID)
            {
                _direction = Vector2.zero;
                _touched = false;
                var directionRaw = data.position - _origin;
                var directionNormalized = directionRaw.normalized;
                if (directionNormalized.x > horizontalDiscreteSensitivity)
                {
                    _horizontalDiscreteMovement = 1;
                }
                else if (directionNormalized.x < -horizontalDiscreteSensitivity)
                {
                    _horizontalDiscreteMovement = -1;
                }
                else
                {
                    _horizontalDiscreteMovement = 0;
                }
            }
        }

        public Vector2 GetDirection()
        {
            _smoothDirection = Vector2.MoveTowards(_smoothDirection, _direction, smoothing);
            return _smoothDirection;
        }

        public int GetHorizontalDiscreteMovement()
        {
            var horizontalDiscreteMovement = _horizontalDiscreteMovement;
            _horizontalDiscreteMovement = 0;
            return horizontalDiscreteMovement;
        }
    }
}