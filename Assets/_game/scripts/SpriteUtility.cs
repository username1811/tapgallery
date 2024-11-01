using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUtility
{
    public static Sprite GetSpriteFromSolidColor(Color color, Texture2D originalTexture2D)
    {
        // Tạo một Texture2D mới với kích thước mong muốn
        Texture2D texture2D = new Texture2D(originalTexture2D.width, originalTexture2D.height);
        Color[] originalPixels = originalTexture2D.GetPixels();


        // Gán màu cho tất cả các pixel của Texture2D mới
        Color[] pixels = new Color[originalTexture2D.width * originalTexture2D.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = originalPixels[i];
            if (pixels[i].a < 0.01f) continue;
            pixels[i] = color;
        }
        texture2D.SetPixels(pixels);
        texture2D.filterMode = FilterMode.Point;
        texture2D.Apply(); // Cần apply để xác nhận màu đã đặt

        // Chuyển Texture2D mới thành Sprite
        return Sprite.Create(texture2D, new Rect(0, 0, originalTexture2D.width, originalTexture2D.height), new Vector2(0.5f, 0.5f));
    }


    public static Sprite GetSpriteFromTexture2D(Texture2D texture2D)
    {
        int width = texture2D.width;
        int height = texture2D.height;
        // Chuyển Texture2D thành Sprite
        return Sprite.Create(texture2D, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }
}
