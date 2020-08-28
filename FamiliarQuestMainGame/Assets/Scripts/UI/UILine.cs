using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class UILine : MonoBehaviour {
    RawImage image;
    Texture2D texture;
    public int x1;
    public int y1;
    public int x2;
    public int y2;

    public void Initialize(int width, int height, int x1, int y1, int x2, int y2) {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        image = GetComponent<RawImage>();
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        image.texture = texture;
        Color[] colors = new Color[width * height];
        texture.SetPixels(0, 0, width, height, colors);
        for (int xMod = -5; xMod < 5; xMod++) {
            for (int yMod = -5; yMod < 5; yMod++) {
                var dx = x2 - x1;
                var dy = y2 - y1;
                if (dx != 0) {
                    for (int x = x1; x != x2; x += (int)Mathf.Sign(x2 - x1)) {
                        int y = y1 + dy * (x - x1) / dx;
                        if (x + xMod >= 0 && x + xMod < width && y + yMod >= 0 && y + yMod < height) texture.SetPixel(x + xMod, y + yMod, Color.white);
                    }
                }
                else {
                    for (int y = y1; y != y2; y += (int)Mathf.Sign(y2 - y1)) {
                        if (x1 + xMod >= 0 && x1 + xMod < width && y + yMod >= 0 && y + yMod < height) texture.SetPixel(x1 + xMod, y + yMod, Color.white);
                    }
                }
            }
        }
        texture.Apply();
    }
}