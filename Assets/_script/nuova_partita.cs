using UnityEngine;

public class nuova_partita : MonoBehaviour
{
    public int NewGame = 0;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("NuovaPartita");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
