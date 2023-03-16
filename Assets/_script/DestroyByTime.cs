using UnityEngine;

namespace _script
{
    public class DestroyByTime : MonoBehaviour
    {
        public float lifetime;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}