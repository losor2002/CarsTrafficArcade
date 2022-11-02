using UnityEngine;

namespace _script
{
    public class destroybyboundary : MonoBehaviour
    {
        void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}
