using System;
using SoldByWizards.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoldByWizards.Maps
{
    public class MapDescriptor : MonoBehaviour
    {
        private static readonly int _randomHue = Shader.PropertyToID("_RandomHue");

        [field: SerializeField]
        public string Name { get; private set; }

        [SerializeField] private int _guaranteedSpawns = 3;
        [SerializeField] private ItemSO[] _additiveItemPool;

        private ItemSO[] _finalItemPool;
        private MapObjectSpawnPoint[] _objectSpawnPoints;

        private void Start()
        {
            var itemManager = FindObjectOfType<ItemsManager>();

            _finalItemPool = itemManager ? itemManager.GlobalObjectPool : _additiveItemPool;

            if (itemManager != null && _additiveItemPool?.Length > 0)
            {
                var globalPool = itemManager.GlobalObjectPool;

                _finalItemPool = new ItemSO[globalPool.Length + _additiveItemPool.Length];
                Array.Copy(globalPool, _finalItemPool, globalPool.Length);
                Array.Copy(_additiveItemPool, 0, _finalItemPool, globalPool.Length, _additiveItemPool.Length);
            }

            _objectSpawnPoints = GetComponentsInChildren<MapObjectSpawnPoint>();

            // Random hue for map shaders
            Shader.SetGlobalFloat(_randomHue, Random.Range(0, 360f));

            // Early return if we have no items to spawn
            if (_objectSpawnPoints.Length == 0) return;

            // Set object pools for every spawn point
            for (var i = 0; i < _objectSpawnPoints.Length; i++)
            {
                _objectSpawnPoints[i]
                    .SetSpawnPool(_objectSpawnPoints[i].UseGlobalObjectPool ? _finalItemPool : _additiveItemPool);
            }

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
