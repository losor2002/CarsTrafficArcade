using UnityEngine;

public class light : MonoBehaviour
{
    public float intensity;
    public float shadowStrenght;

    private void Start()
    {
        int z = PlayerPrefs.GetInt("zombie");
        if (z == 1)
        {
            GetComponent<Light>().intensity = intensity;
            GetComponent<Light>().shadowStrength = shadowStrenght;
        }
    }
}
