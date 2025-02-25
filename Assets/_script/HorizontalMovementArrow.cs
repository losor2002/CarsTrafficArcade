using UnityEngine;
using UnityEngine.EventSystems;

namespace _script
{
    public class HorizontalMovementArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private GameControllerPlayScene _gameControllerPlayScene;
        private bool _isRightArrow;
        private int _pointerID;
        private bool _touched;

        private void Start()
        {
            _gameControllerPlayScene = FindAnyObjectByType<GameControllerPlayScene>();
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
                _gameControllerPlayScene.horizontalPlayerMovement = 1;
            }
            else
            {
                _gameControllerPlayScene.horizontalPlayerMovement = -1;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerId == _pointerID)
            {
                _gameControllerPlayScene.horizontalPlayerMovement = 0;
                _touched = false;
            }
        }
    }
}