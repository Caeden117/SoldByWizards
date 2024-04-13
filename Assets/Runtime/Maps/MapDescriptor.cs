using UnityEngine;
using Random = UnityEngine.Random;

namespace SoldByWizards.Maps
{
    public class MapDescriptor : MonoBehaviour
    {
        private static readonly int _randomHue = Shader.PropertyToID("_RandomHue");

        [field: SerializeField]
        public string Name { get; private set; }

        [SerializeField] private MapObjectSpawnPoint[] _objectSpawnPoints;
        [SerializeField] private int _guaranteedSpawns = 3;

        private void Start()
        {
            // Random hue for map shaders
            Shader.SetGlobalFloat(_randomHue, Random.Range(0, 360f));

            // Early return if we have no items to spawn
            if (_objectSpawnPoints.Length == 0) return;

            // Ensure X amount of items always spawn
            for (var i = 0; i < _guaranteedSpawns; i++)
            {
                var randomIdx = Random.Range(0, _objectSpawnPoints.Length);
                _objectSpawnPoints[randomIdx].Spawn();
            }

            // Any other items will use the random chance spawn system
            for (var i = 0; i < _objectSpawnPoints.Length; i++)
            {
                _objectSpawnPoints[i].RandomSpawn();
            }
        }
    }
}
