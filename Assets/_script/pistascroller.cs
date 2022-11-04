using System.Collections;
using UnityEngine;

namespace _script
{
    public class pistascroller : MonoBehaviour

    {
        public Vector3 spawn;
        public GameObject pista;
        public float spawntime;
        public float spawntime1;
        public float spawntime2;

        private GameController gamecontroller;

        void Start ()
        {
            gamecontroller = GetComponent<GameController>();
            StartCoroutine(frequenza());
        }

        private void Update()
        {
            if (gamecontroller.score < 50 && gamecontroller.score >= 30)
            {
                spawntime = spawntime1;
            }
            if (gamecontroller.score >= 50)
            {
                spawntime = spawntime2;
            }
        }

        IEnumerator frequenza()
        {
            while (!gamecontroller.gameOver)
            {
                Quaternion rotation = Quaternion.Euler(1.825f, 0.0f, 2.157f);
                Instantiate(pista, spawn, rotation);
                yield return new WaitForSeconds(spawntime);
            }
        }
    }
}
