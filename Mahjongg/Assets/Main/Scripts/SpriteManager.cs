using Sirenix.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public List<(Sprite, Sprite)> LoadImages()
    {
        string[] files = Directory.GetFiles($"{Application.streamingAssetsPath}/Images");

        string[] filesNoMeta = files.Where(e => !e.EndsWith(".meta")).ToArray();

        Texture2D[] textures = new Texture2D[filesNoMeta.Length];

        for (int i = 0; i < filesNoMeta.Length; i++)
        {
            textures[i] = new Texture2D(1, 1);
            byte[] bytes = File.ReadAllBytes(filesNoMeta[i]);
            ImageConversion.LoadImage(textures[i], bytes);
        }

        Sprite[] sprites = new Sprite[textures.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            Rect rec = new Rect(0, 0, textures[i].width, textures[i].height);
            sprites[i] = Sprite.Create(textures[i], rec, new Vector2(0.5f, 0.5f), 1);
        }

        List<(Sprite, Sprite)> spriteList = new();

        for (int i = 0; i < sprites.Length; i+=2)
        {
            spriteList.Add((sprites[i], sprites[i + 1]));
        }

        return spriteList;
    }
}
