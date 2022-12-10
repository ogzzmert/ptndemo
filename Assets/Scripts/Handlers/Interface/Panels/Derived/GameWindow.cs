using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class GameWindow : GameInteractable
{
    public WindowData currentWindow {get; private set;}
    Dictionary<string, GameInteractable> dict;
    
    [Serializable]
    public class Data
    {
        public int frame;
        public string frames;
        public int width;
        public int height;
        public int size;
        public int opacity;
        public int font;
        public string text;
    }
    public class WindowData
    {
        public Data data;
        public Texture2D texture;
        public Color color;
        public bool compress;
        public bool responsive;
        public WindowData(Data data, bool compress = true, bool responsive = false) { this.data = data; this.compress = compress; this.responsive = responsive; buildWindowData(this); }
    }
    public GameWindow(World world, Transform transform) : base (world, transform)
    {
        this.image.raycastTarget = false;
        dict = new Dictionary<string, GameInteractable>();
    }
    public void initialize(WindowData windowData)
    {
        currentWindow = windowData;
        reload();
    }
    public void reload()
    {
        buildImage();
    }
    public void refresh()
    {
        currentWindow.texture.Apply();
    }
    public T addInteractable<T>(string key) where T : GameInteractable
    {
        T item = (T)Activator.CreateInstance(typeof(T), new object[] { this.world, new GameObject(key).AddComponent<RectTransform>() });
        item.setParent(this);

        if(!dict.ContainsKey(key))
        {
            dict.Add(key, item);
            return item;
        }
        else return null;
    }
    public T getInteractable<T>(string key) where T : GameInteractable
    {
        if (dict.ContainsKey(key))
        {
            return dict[key] as T;
        }
        else return null;
    }
    public void removeInteractable(string key)
    {
        if (dict.ContainsKey(key))
        {
            GameInteractable i = dict[key] as GameInteractable;
            dict.Remove(key);
            i.discard();
        }
    }
    public void Clear()
    {
        foreach(GameInteractable i in dict.Values) i.discard();
        dict.Clear();
    }
    public static void buildWindowData(WindowData wd)
    {
        /*
        TextureManager.FontTexture ft = TextureManager.font_textures[(TextureManager.font_type)wd.data.frame];

        int size_x = ft.size_x;
        int size_y = ft.size_y;

        List<string> content = Calculator.formatText(wd.data.text, wd.data.width);

        int width = wd.data.width;
        int height = wd.responsive ? content.Count + 2 : wd.data.height;

        wd.texture = new Texture2D(width * size_x, height * size_y, TextureFormat.RGBA32, false);
        string frames = wd.data.frames;
        if (frames.Length != 9) frames = GameWindow.frames[type.basic];

        wd.texture.filterMode = FilterMode.Point;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                char f = frames[8];
            
                if (i == 0 && j == height - 1) f = frames[0];
                else if (i == width - 1 && j == height - 1) f = frames[1];
                else if (i == 0 && j == 0) f= frames[2];
                else if (i == width - 1 && j == 0) f = frames[3];
                else if (i == 0 && j > 0 && j < height - 1) f = frames[4];
                else if (i == width - 1 && j > 0 && j < height - 1) f = frames[5];
                else if (j == height - 1 && i > 0 && i < width - 1) f = frames[6];
                else if (j == 0 && i > 0 && i < width - 1) f = frames[7];

                Texture2D _draw = ft.dict[f];
                for (int _x = 0; _x < size_x; _x++)
                {
                    for (int _y = 0; _y < size_y; _y++)
                    {
                        wd.texture.SetPixel(i * size_x + _x, j * size_y + _y, _draw.GetPixel(_x, _y));
                    }
                }
            }
        }
        
        wd.color = wd.color == null ? TextureManager.basicColor : wd.color;

        TextureManager.font_type font = (TextureManager.font_type)wd.data.font;

        int lineOffset = wd.compress ? TextureManager.font_textures[font].size_y / 3 : 0;
        
        for(int i = 0; i < content.Count; i++)
        {
            TextureManager.combineTex(
                wd.texture, 
                TextureManager.getTextureOfText(content[i], font, wd.color), 
                size_x, 
                (-i - 2) * size_y + (i * lineOffset), 
                false
            );
        }
        */
    }
    void buildImage()
    {
        refresh();
        this.image.sprite = TextureManager.spritify(currentWindow.texture);
        setSize(this.currentWindow.data.size);
        setOpacity(this.currentWindow.data.opacity);
    }
    public static GameWindow.Data cloneData(GameWindow.Data window)
    {
        return new Data()
            {
                frame = window.frame,
                frames = window.frames,
                width = window.width,
                height = window.height,
                size = window.size,
                opacity = window.opacity,
                font = window.font,
                text = window.text
            };
    }
}