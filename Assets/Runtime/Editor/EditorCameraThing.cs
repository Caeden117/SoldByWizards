#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SoldByWizards.Editor
{
    public class EditorCameraThing : MonoBehaviour
    {
        public List<ItemCameraData> ItemCameraDatas = new();
        public Camera Camera = null!;
        public int PreviewIndex;

        public void PreviewCamera()
        {
            var item = ItemCameraDatas[PreviewIndex];
            this.transform.position = (transform.forward * (-item.Length)) + new Vector3(0, item.Height, 0);
            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, item.CameraRotation);
            foreach (var itemData in ItemCameraDatas)
            {
                itemData.Item.gameObject.SetActive(itemData == item);
            }
        }

        public void DoCamera()
        {
            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < ItemCameraDatas.Count; i++)
            {
                var item = ItemCameraDatas[i];
                this.transform.position = (transform.forward * (-item.Length)) + new Vector3(0, item.Height, 0);
                this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, item.CameraRotation);
                foreach (var itemData in ItemCameraDatas)
                {
                    itemData.Item.gameObject.SetActive(itemData == item);
                }
                var path = Path.Join(Application.dataPath, $"Textures/ItemIcons/{i}.png");
                CaptureTransparentScreenshot(Camera, 512, 512, path);

            }
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }

    public static void CaptureTransparentScreenshot(Camera cam, int width, int height, string screengrabfile_path) {
        // This is slower, but seems more reliable.
        var bak_cam_targetTexture = cam.targetTexture;
        var bak_cam_clearFlags = cam.clearFlags;
        var bak_RenderTexture_active = RenderTexture.active;

        var tex_white = new Texture2D(width, height, TextureFormat.ARGB32, false);
        var tex_black = new Texture2D(width, height, TextureFormat.ARGB32, false);
        var tex_transparent = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Must use 24-bit depth buffer to be able to fill background.
        var render_texture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
        var grab_area = new Rect(0, 0, width, height);

        RenderTexture.active = render_texture;
        cam.targetTexture = render_texture;
        cam.clearFlags = CameraClearFlags.SolidColor;

        cam.backgroundColor = Color.black;
        cam.Render();
        tex_black.ReadPixels(grab_area, 0, 0);
        tex_black.Apply();

        cam.backgroundColor = Color.white;
        cam.Render();
        tex_white.ReadPixels(grab_area, 0, 0);
        tex_white.Apply();

        // Create Alpha from the difference between black and white camera renders
        for (int y = 0; y < tex_transparent.height; ++y) {
            for (int x = 0; x < tex_transparent.width; ++x) {
                float alpha = tex_white.GetPixel(x, y).r - tex_black.GetPixel(x, y).r;
                alpha = 1.0f - alpha;
                Color color;
                if (alpha == 0) {
                    color = Color.clear;
                }
                else {
                    color = tex_black.GetPixel(x, y) / alpha;
                }
                color.a = alpha;
                tex_transparent.SetPixel(x, y, color);
            }
        }

        // Encode the resulting output texture to a byte array then write to the file
        byte[] pngShot = ImageConversion.EncodeToPNG(tex_transparent);
        File.WriteAllBytes(screengrabfile_path, pngShot);

        cam.clearFlags = bak_cam_clearFlags;
        cam.targetTexture = bak_cam_targetTexture;
        RenderTexture.active = bak_RenderTexture_active;
        RenderTexture.ReleaseTemporary(render_texture);

        Texture2D.DestroyImmediate(tex_black);
        Texture2D.DestroyImmediate(tex_white);
        Texture2D.DestroyImmediate(tex_transparent);
    }

    }
}

#endif
