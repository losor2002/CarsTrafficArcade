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

            var lightComponent = GetComponent<Light>();
            lightComponent.intensity = intensity;
            lightComponent.shadowStrength = shadowStrength;
        }
    }
}