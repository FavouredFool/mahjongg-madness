using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public List<(Sprite, Sprite)> LoadImages()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images");
        List<(Sprite, Sprite)> spriteList = new();

        for (int i = 0; i < sprites.Length; i+=2)
        {
            spriteList.Add((sprites[i], sprites[i + 1]));
        }

        return spriteList;
    }
}
