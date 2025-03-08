using UnityEngine;

namespace CtaScript.SceneManagement
{
    public class DestroyByBoundary : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}