using UnityEngine;

namespace _script
{
    public class rotation : MonoBehaviour
    {

        private void Start()
        {
            Quaternion spawn = Quaternion.Euler(-90f, -180f, 0f);
            transform.rotation = spawn;
        }
    }
}
