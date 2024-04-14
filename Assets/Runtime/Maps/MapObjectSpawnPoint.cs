using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.Maps
{
    public class MapObjectSpawnPoint : MonoBehaviour
    {
        [SerializeField] private float _spawnChance = 0.75f;
        [SerializeField] private ItemSO[] _itemPrefabs;

        public bool HasItem { get; private set; }

        public void RandomSpawn()
        {
            if (Random.Range(0f, 1.0f) > _spawnChance) return;

            Spawn();
        }

        public void Spawn()
        {
            if (HasItem) return;

            HasItem = true;

            var randomIdx = Random.Range(0, _itemPrefabs.Length);
            var t = transform;

            var itemSO = _itemPrefabs[randomIdx];

            if (itemSO.ItemPrefab  == null)
            {
                Debug.LogError("OOPS! SOMEONE FORGOT TO ASSIGN THE ITEM PREFAB!!");
                return;
            }

            var item = Instantiate(itemSO.ItemPrefab, t.position, t.rotation);

            item.transform.localEulerAngles = item.transform.localEulerAngles.WithY(Random.Range(0f, 360f));

            item.ItemSO = itemSO;
        }
    }
}
