using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class GameWindow : GameInteractable
{
    public WindowData currentWindow {get; private set;}
    
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
        public WindowData(Data data, bool compress = true, bool responsive = false) { this.data = data; this.compress = compress; this.responsive = responsive; }
    }
    public GameWindow(World world, Transform transform, UnityEvent action) : base (world, transform, action)
    {
        this.image.raycastTarget = false;
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
    public void buildWindowData(WindowData wd)
    {
        
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