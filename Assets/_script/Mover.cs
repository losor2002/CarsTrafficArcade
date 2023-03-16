using UnityEngine;

namespace _script
{
    public class Mover : MonoBehaviour
    {
        public float speed;

        private void Start()
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }
}