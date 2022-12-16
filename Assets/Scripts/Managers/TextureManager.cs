
using System;
using UnityEngine;
using UnityEngine.UI;
public class TextureManager
{
    private static Font font;
    
    private TextureManager() { }

    public static void load()
    {
        foreach(EntityProductType ept in Enum.GetValues(typeof(EntityProductType)))
        {
            ResourceManager.save<Sprite>("UI/Entities/Product/", ept.ToString());
        }
        ResourceManager.save<Sprite>("UI/Entities/Product/", "Craft");
    }
    public static Sprite spritify(Texture2D tex, float offset=0.5f)
    {
        // generate sprite object from texture data, offset is set to center
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(offset, 0.5f));
    }
    public static Font getFont() { return font; }
}