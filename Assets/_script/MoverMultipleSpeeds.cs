﻿using UnityEngine;

namespace _script
{
    public class MoverMultipleSpeeds : MonoBehaviour
    {
        public float speed;
        public float speed1;
        public float speed2;

        private GameController _gameController;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        private void Update()
        {
            _rigidbody.velocity = _gameController.score switch
            {
                >= 30 and < 50 => transform.forward * speed1,
                >= 50 => transform.forward * speed2,
                _ => transform.forward * speed
            };

            if (_gameController.gameOver)
            {
                if (PlayerPrefs.GetInt("zombie") == 1)
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