using UnityEngine;

namespace CtaScript.Movement
{
    public class Mover : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
    }
}