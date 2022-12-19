using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TextManager
{
    public static Language currentLanguage { get; private set; } = Language.ENG;
    static Dictionary<Content, TextContent> texts = new Dictionary<Content, TextContent>();

    protected class TextData
    {
        public string[] items;
    }

    protected class TextContent
    {
        Dictionary<Language, string> items = new Dictionary<Language, string>();
        public TextContent(TextData td) 
        {
            for(int i = 0; i < td.items.Length; i++) items.Add((Language)i, td.items[i].Replace('-', '\n'));
        }
        public string get(Language language) { return items[language]; }
    }
    private TextManager() { }
    public static void load()
    {

        foreach(Content content in Enum.GetValues(typeof(Content)))
        {
            string txtname = content.ToString();
            TextContent txtc = new TextContent(JsonUtility.FromJson<TextData>(ResourceManager.load<TextAsset>("Prefab/Text/" + txtname).text));
            if (txtc != null)
            {
                texts.Add(content, txtc);
            }
        }
    }
    public static void changeLanguage(Language language) { currentLanguage = language; }
    public static string bring(Content content) 
    { 
        if (texts.ContainsKey(content))
        {
            return texts[content].get(currentLanguage);
        }
        else return "?? ERROR ??";
    }
    public enum Content
    {
        Credits = 10,
        MenuCredits = 11,
        MenuStart = 12,
        ProductProduce = 13,
        ProductProducePrompt = 14,
        Products = 15,
        Units = 16,
        Currency = 17,
        Required = 18,
        NoResources = 19,
        IneligiblePosition = 20,
        Durability = 21,
        ProductCraft = 22,
        ProductCraftPrompt = 23,
        Operations = 24,
        Power = 25
    }
    public enum Language
    {
        TR = 0,
        ENG = 1
    }
}