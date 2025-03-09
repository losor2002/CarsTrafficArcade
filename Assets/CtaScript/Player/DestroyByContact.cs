using CtaScript.GameController;
using UnityEngine;
using UnityEngine.Serialization;

namespace CtaScript.Player
{
    public class DestroyByContact : MonoBehaviour
    {
        public GameObject explosion;
        public GameObject fallingZombie;

        private GameControllerPlayScene _gameControllerPlayScene;
        private int _zombie;

        private void Start()
        {
            _gameControllerPlayScene = FindAnyObjectByType<GameControllerPlayScene>();
            _zombie = PlayerPrefs.GetInt("zombie");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                var otherTransform = other.transform;
                if (_zombie == 0)
                {
                    var otherPosition = otherTransform.position + new Vector3(0f, 0.1f, 0f);
                    Instantiate(explosion, otherPosition, otherTransform.rotation);
                    Destroy(other.gameObject);
                }
                else
                {
                    Instantiate(fallingZombie, otherTransform.position, otherTransform.rotation);
                    Destroy(other.gameObject);
                }

                var position = transform.position + new Vector3(0f, 0.1f, 0f);
                Instantiate(explosion, position, transform.rotation);
                _gameControllerPlayScene.GameOver();
                Destroy(gameObject);
            }
        }
    }
}