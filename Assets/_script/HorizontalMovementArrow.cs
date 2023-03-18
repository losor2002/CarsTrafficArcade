using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class HorizontalMovementArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private GameController _gameController;
        private bool _isRightArrow;
        private int _pointerID;
        private bool _touched;

        private void Start()
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            _isRightArrow = CompareTag("destra");
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (_touched)
            {
                return;
            }

            _touched = true;
            _pointerID = data.pointerId;
            if (_isRightArrow)
            {
                _gameController.horizontalPlayerMovement = 1;
            }
            else
            {
                _gameController.horizontalPlayerMovement = -1;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == _pointerID)
            {
                _gameController.horizontalPlayerMovement = 0;
                _touched = false;
            }
        }
    }
}