using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class TextureUtil {
  public static Image CreateImage(Transform parent, Texture2D tex, float scale, string name) {
    var go = new GameObject(name);
    var image = go.AddComponent<Image>();

    image.transform.SetParent(parent, false);
    image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1);
    image.rectTransform.sizeDelta = new Vector2(tex.width * scale, tex.height * scale);
    image.color = new Color(1, 1, 1, 0.6f);
    return image;
  }

  public static Texture2D CreateResultTexture(int width, int height, List<Vector2Int> result) {
    var pixels = new Color[width * height];

    for (int i = 0; i < width * height; ++i) {
      pixels[i] = new Color(1, 1, 1, 0);
    }

    for (int i = 0; i < result.Count; ++i) {
      float f = (float)i / result.Count;
      var p = result[i];
      var (x, y) = (p.x, p.y);
      var color = new Color(f, 0, 1 - f, 0.6f);
      SetPixel(x, y, width, height, color, pixels);
    }

    var tex = new Texture2D(width, height);
    tex.SetPixels(pixels);
    tex.filterMode = FilterMode.Point;
    tex.Apply();
    return tex;
  }

  public static Texture2D CreateTextureWithTable(int width, int height, int[] table) {
    var pixels = new Color[table.Length];

    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        var srcIndex = y * width + x;
        var color = Cost.CostToColor(table[srcIndex]);
        SetPixel(x, y, width, height, color, pixels);
      }
    }

    var tex = new Texture2D(width, height);
    tex.SetPixels(pixels);
    tex.filterMode = FilterMode.Point;
    tex.Apply();
    return tex;
  }

  private static void SetPixel(int x, int y, int width,int height, Color color, Color[] pixels) {
    var dstIndex = (height - y - 1) * width + x;
    pixels[dstIndex] = color;
  }
}
