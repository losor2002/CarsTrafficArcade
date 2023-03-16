using UnityEngine;

namespace _script
{
    public class LightProperties : MonoBehaviour
    {
        public float intensity;
        public float shadowStrength;
        public string key;

        private void Start()
        {
            if (PlayerPrefs.GetInt(key) != 1)
            {
                return;
            }

            var light = GetComponent<Light>();
            light.intensity = intensity;
            light.shadowStrength = shadowStrength;
        }
    }
}