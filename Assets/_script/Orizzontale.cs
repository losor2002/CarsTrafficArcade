using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class Orizzontale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool a;
        private GameController gamecontroller;
        private int pointerID;
        private bool touched;

        private void Start()
        {
            var gc = GameObject.FindGameObjectWithTag("GameController");
            gamecontroller = gc.GetComponent<GameController>();
            touched = false;
            if (CompareTag("destra"))
            {
                a = true;
            }
            else
            {
                a = false;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (!touched)
            {
                touched = true;
                pointerID = data.pointerId;
                if (a)
                {
                    gamecontroller.horizontalPlayerMovement = 1;
                }
                else
                {
                    gamecontroller.horizontalPlayerMovement = -1;
                }
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == pointerID)
            {
                gamecontroller.horizontalPlayerMovement = 0;
                touched = false;
            }
        }
    }
}