using UnityEngine;

namespace Lab.ObjectSpawning
{
    public class Spawner : MonoBehaviour
    {
        public GameObject spawnPoint;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < 30; i++)
            {
                SpawnTree();
            }
        }

        private void SpawnTree()
        {
            var pos = Random.insideUnitSphere * 50;

            pos.y = 0;

            var tree = Instantiate(
                Resources.Load("BroadLeafTree") as GameObject,
                pos + spawnPoint.transform.position,
                Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}