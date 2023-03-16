using UnityEngine;

namespace _script
{
    public class TouchedBonus : MonoBehaviour
    {
        private GameController _gameController;
        private bool _touched;

        private void Start()
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !_touched)
            {
                _gameController.Touched();
                _touched = true;
            }
        }
    }
}