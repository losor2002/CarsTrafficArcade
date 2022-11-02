using System.Collections;
using UnityEngine;

namespace _script
{
    public class moverbg : MonoBehaviour
    {
        public float speed;

        private GameController gc;

        private void Start()
        {
            GameObject GC = GameObject.FindGameObjectWithTag("GameController");
            gc = GC.GetComponent<GameController>();
        }

        private void Update()
        {
            if (gc.score >= 50)
            {
                StartCoroutine(Destroy());
            }
        }

        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(1.35f);
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
            yield return new WaitForSeconds(2.25f);
            Destroy(gameObject);
        }
    }
}
