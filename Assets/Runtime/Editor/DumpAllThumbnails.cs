#if UNITY_EDITOR
using SoldByWizards.Items;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace SoldByWizards.Editor
{
    public static class DumpAllThumbnails
    {
        [MenuItem("Sold By Wizards/Generate Thumbnails")]
        public static void DumpItemThumbnails()
        {
            DumpItemThumbnailsAsync().Forget();
        }

        private static async UniTask DumpItemThumbnailsAsync()
        {
            var items = Resources.FindObjectsOfTypeAll<ItemSO>();

            var processedItems = 0;
            var totalItems = items.Length;

            foreach (var item in items)
            {
                UpdateProgressBar((float)processedItems / totalItems);

                var preview = AssetPreview.GetAssetPreview(item.ItemPrefab.gameObject);

                await UniTask.Delay(1000, DelayType.Realtime);

                var bytes = DuplicateTexture(AssetPreview.GetAssetPreview(item.ItemPrefab.gameObject)).EncodeToPNG();
                Directory.CreateDirectory($"{Application.dataPath}/Textures/Items");
                var path = $"{Application.dataPath}/Textures/Items/{item.ItemName}.png";

                await File.WriteAllBytesAsync(path, bytes);


                processedItems++;
                UpdateProgressBar((float)processedItems / totalItems);
            }

            EditorUtility.ClearProgressBar();
        }

        private static void UpdateProgressBar(float v)
            => EditorUtility.DisplayProgressBar("Generating Thumbnails!", "Big fan of caeden's mom.", v);

        private static Texture2D DuplicateTexture(Texture2D source)
        {
            var renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);

            var previous = RenderTexture.active;

            RenderTexture.active = renderTex;

            var readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            return readableText;
        }
    }
}
#endif
