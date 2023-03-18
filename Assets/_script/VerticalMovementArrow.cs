using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class VerticalMovementArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private GameController _gameController;
        private bool _isUpArrow;
        private int _pointerID;
        private bool _touched;

        private void Start()
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            _isUpArrow = CompareTag("su");
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (_touched)
            {
                return;
            }

            _touched = true;
            _pointerID = data.pointerId;
            if (_isUpArrow)
            {
                _gameController.verticalPlayerMovement = 1;
            }
            else
            {
                _gameController.verticalPlayerMovement = -1;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == _pointerID)
            {
                _gameController.verticalPlayerMovement = 0;
                _touched = false;
            }
        }
    }
}