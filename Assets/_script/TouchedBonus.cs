using UnityEngine;

namespace _script
{
    public class TouchedBonus : MonoBehaviour
    {
        private GameControllerPlayScene _gameControllerPlayScene;

        private void Start()
        {
            _gameControllerPlayScene = FindAnyObjectByType<GameControllerPlayScene>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _gameControllerPlayScene.Touched();
            }
        }
    }
}