using Cysharp.Threading.Tasks;
using SoldByWizards.Glyphs;
using SoldByWizards.Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace SoldByWizards.Maps
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField] private GlyphAggregator _glyphAggregator;
        [SerializeField] private int[] _allMapSceneIdx;

        private bool _hasMap;
        private Scene _loadedMap;

        public async UniTask LoadMapFromGlyphs()
        {
            if (_hasMap) return;

            _hasMap = true;

            var glyphSeed = _glyphAggregator.GetAggregateHash();

            Random.InitState(glyphSeed);

            var sceneIdx = Random.Range(0, _allMapSceneIdx.Length);

            await SceneManager.LoadSceneAsync(_allMapSceneIdx[sceneIdx], LoadSceneMode.Additive);

            _loadedMap = SceneManager.GetSceneByBuildIndex(_allMapSceneIdx[sceneIdx]);
        }

        public async UniTask UnloadMap()
        {
            if (_loadedMap == default || !_loadedMap.isLoaded) return;

            // Progress stock market, reset penalties from selling items
            StockMarket.GameTime += 0.1f;
            StockMarket.ResetStockMarket();

            await SceneManager.UnloadSceneAsync(_loadedMap);

            _hasMap = false;
        }
    }
}
