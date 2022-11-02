using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class Orizzontale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool touched;
        private int pointerID;
        private bool a;
        private GameController gamecontroller;

        void Start()
        {
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            gamecontroller = gc.GetComponent<GameController>();
            touched = false;
            if (this.CompareTag("destra"))
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
                    gamecontroller.hor = 1;
                }
                else
                {
                    gamecontroller.hor = -1;
                }
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == pointerID)
            {
                gamecontroller.hor = 0;
                touched = false;
            }
        }
    }
}
