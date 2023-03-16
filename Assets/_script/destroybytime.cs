using UnityEngine;

namespace _script
{
    public class destroybytime : MonoBehaviour
    {
        public float lifetime;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}