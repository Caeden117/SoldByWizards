using System;
using SoldByWizards.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoldByWizards.Maps
{
    public class MapObjectSpawnPoint : MonoBehaviour
    {
        [SerializeField] private float _spawnChance = 0.75f;
        [SerializeField] private ItemSO[] _itemPrefabs;

        private Item _item = null!;

        public void RandomSpawn()
        {
            if (Random.Range(0f, 1.0f) > _spawnChance) return;

            Spawn();
        }

        public void Spawn()
        {
            if (_item) return;

            var randomIdx = Random.Range(0, _itemPrefabs.Length);
            var t = transform;

            var itemSO = _itemPrefabs[randomIdx];

            if (itemSO.ItemPrefab  == null)
            {
                Debug.LogError("OOPS! SOMEONE FORGOT TO ASSIGN THE ITEM PREFAB!!");
                return;
            }

            _item = Instantiate(itemSO.ItemPrefab, t.position, t.rotation);

            _item.transform.localEulerAngles = _item.transform.localEulerAngles.WithY(Random.Range(0f, 360f));

            _item.ItemSO = itemSO;
            _item.SellPrice = StockMarket.CalculatePriceFor(itemSO);
            _item.SpawnPoint = this;
        }

        public void ClearSpawn() => _item = null;

        private void OnDestroy()
        {
            if (!_item) return;

            DestroyImmediate(_item.gameObject);
        }
    }
}
