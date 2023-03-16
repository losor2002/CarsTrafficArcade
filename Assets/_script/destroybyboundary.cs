using UnityEngine;

namespace _script
{
    public class destroybyboundary : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}