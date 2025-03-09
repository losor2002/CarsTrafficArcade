using UnityEngine;

namespace CtaScript.SceneManagement
{
    public class TrackSpawn : MonoBehaviour
    {
        public Vector3 spawnPosition;
        public Quaternion spawnRotation;
        public float zSpawnThreshold;
        
        private bool _spawned;

        private void Update()
        {
            if (!_spawned && transform.position.z <= zSpawnThreshold)
            {
                GameObject newTrack = Instantiate(gameObject, spawnPosition, spawnRotation);
                newTrack.name = gameObject.name;
                _spawned = true;
            }
        }
    }
}