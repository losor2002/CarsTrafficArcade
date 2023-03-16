using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class SimpleTouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool canFire;
        private int pointerID;

        private bool touched;

        private void Awake()
        {
            touched = false;
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (!touched)
            {
                touched = true;
                pointerID = data.pointerId;
                canFire = true;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == pointerID)
            {
                canFire = false;
                touched = false;
            }
        }

        public bool CanFire()
        {
            return canFire;
        }
    }
}