using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _script
{
    public class TrackSpawn : MonoBehaviour
    {
        [FormerlySerializedAs("pista")] public GameObject track;
        [FormerlySerializedAs("spawn")] public Vector3 spawnPosition;
        public Quaternion spawnRotation;
        [FormerlySerializedAs("spawntime")] public float spawnInterval;
        [FormerlySerializedAs("spawntime1")] public float spawnInterval1;
        [FormerlySerializedAs("spawntime2")] public float spawnInterval2;

        private GameController _gameController;

        private void Start()
        {
            _gameController = GetComponent<GameController>();
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (!_gameController.gameOver)
            {
                Instantiate(track, spawnPosition, spawnRotation);

                spawnInterval = _gameController.score switch
                {
                    >= 30 and < 50 => spawnInterval1,
                    >= 50 => spawnInterval2,
                    _ => spawnInterval
                };
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}