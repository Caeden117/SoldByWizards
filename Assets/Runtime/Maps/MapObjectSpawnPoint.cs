using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.Maps
{
    public class MapObjectSpawnPoint : MonoBehaviour
    {
        [SerializeField] private float _spawnChance = 0.75f;
        [SerializeField] private Item[] _itemPrefabs;

        public bool HasItem { get; private set; }

        public void RandomSpawn()
        {
            if (Random.Range(0f, _spawnChance) > _spawnChance) return;

            Spawn();
        }

        public void Spawn()
        {
            if (HasItem) return;

            HasItem = true;

            var randomIdx = Random.Range(0, _itemPrefabs.Length);
            var t = transform;

            Instantiate(_itemPrefabs[randomIdx], t.position, t.rotation);
        }
    }
}
