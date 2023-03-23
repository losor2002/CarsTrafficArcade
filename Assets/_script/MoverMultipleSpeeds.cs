using UnityEngine;

namespace _script
{
    public class MoverMultipleSpeeds : MonoBehaviour
    {
        public float speed;
        public float speed1;
        public float speed2;

        private GameControllerPlayScene _gameControllerPlayScene;
        private Rigidbody _rigidbody;
        private bool _zombieMode;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _gameControllerPlayScene = GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<GameControllerPlayScene>();
            _zombieMode = PlayerPrefs.GetInt("zombie") != 0;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _gameControllerPlayScene.score switch
            {
                >= 30 and < 50 => transform.forward * speed1,
                >= 50 => transform.forward * speed2,
                _ => transform.forward * speed
            };

            if (_gameControllerPlayScene.gameOver)
            {
                if (_zombieMode)
                {
                    _rigidbody.velocity = CompareTag("pista") || CompareTag("zombieCade")
                        ? Vector3.zero
                        : new Vector3(0f, 0f, -1f);
                }
                else
                {
                    _rigidbody.velocity = CompareTag("pista") ? Vector3.zero : new Vector3(0f, 0f, 10f);
                }
            }
        }
    }
}