using UnityEngine;

namespace _script
{
    public class TouchedBonus : MonoBehaviour
    {
        private GameControllerPlayScene _gameControllerPlayScene;
        private bool _touched;

        private void Start()
        {
            _gameControllerPlayScene = GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<GameControllerPlayScene>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !_touched)
            {
                _gameControllerPlayScene.Touched();
                _touched = true;
            }
        }
    }
}