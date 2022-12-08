
using UnityEngine;
using UnityEngine.UI;
public class TextureManager
{
    private static Font font;
    
    private TextureManager() { }
    public static Sprite spritify(Texture2D tex, float offset=0.5f)
    {
        // generate sprite object from texture data, offset is set to center
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(offset, 0.5f));
    }
    public static Font getFont() { return font; }
}