using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class touchpad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public float smoothing;
        private Vector2 direction;

        private Vector2 origin;
        private int pointerID;
        private Vector2 smoothdirection;
        private bool touched;

        private void Awake()
        {
            direction = Vector2.zero;
            touched = false;
        }

        public void OnDrag(PointerEventData data)
        {
            if (data.pointerId == pointerID)
            {
                var currentpos = data.position;
                var directionRaw = currentpos - origin;
                direction = directionRaw.normalized;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (!touched)
            {
                touched = true;
                pointerID = data.pointerId;
                origin = data.position;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == pointerID)
            {
                direction = Vector2.zero;
                touched = false;
            }
        }

        public Vector2 GetDirection()
        {
            smoothdirection = Vector2.MoveTowards(smoothdirection, direction, smoothing);
            return smoothdirection;
        }
    }
}