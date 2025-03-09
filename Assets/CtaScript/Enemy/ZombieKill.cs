using CtaScript.GameController;
using UnityEngine;
using UnityEngine.Serialization;

namespace CtaScript.Enemy
{
    public class ZombieKill : MonoBehaviour
    {
        public GameObject fallingZombie;

        private bool _hit;
        private GameControllerPlayScene _gameControllerPlayScene;

        private void Start()
        {
            _gameControllerPlayScene = FindAnyObjectByType<GameControllerPlayScene>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("proiettile"))
            {
                if (_hit)
                {
                    Instantiate(fallingZombie, transform.position, transform.rotation);
                    _gameControllerPlayScene.ZombieKill();
                    Destroy(gameObject);
                }
                else
                {
                    _hit = true;
                }

                Destroy(other.gameObject);
            }
        }
    }
}