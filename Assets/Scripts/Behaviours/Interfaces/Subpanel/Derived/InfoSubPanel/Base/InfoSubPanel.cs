using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSubPanel : SubPanel
{
    [field: SerializeField] Transform listHolder;
    List<SubPanel> list = new List<SubPanel>();

    GameBar gameBar;
    public override void initialize<T>(World world, T parentPanel)
    {
        base.initialize(world, parentPanel);
        gameBar = getInteractable<GameBar>(Panel.type.bar, "bar");
    }
    public void showCraftableInfo(EntityProductType productType)
    {
        clear();

        setMainImage(productType.ToString());

        gameBar.setText(TextManager.bring(TextManager.Content.Products).Split('*')[(int)productType]);

        SubPanel craft = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListItem, this);
        addToList(craft);

        craft.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>("Craft"));

        GameButton craftButton = craft.getInteractable<GameButton>(type.button, "button");

        craftButton.setText(TextManager.bring(TextManager.Content.ProductCraft));
        craftButton.onClick(() => tryCraftable(productType));
        
        // productables
    }
    public void tryCraftable(EntityProductType productType)
    {
        world.handle<AudioHandler>().playSoundActionB();
    }
    private void setMainImage(string imageName)
    {
        gameBar.setSprite(ResourceManager.loadFromCache<Sprite>(imageName));
    }
    private void addToList(SubPanel subPanel)
    {
        subPanel.transform.SetParent(listHolder);
        subPanel.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        subPanel.GetComponent<RectTransform>().offsetMin = Vector2.zero;

        subPanel.transform.Translate
        (
            0, 
            subPanel.transform.GetChild(0).GetComponent<RectTransform>().rect.height * list.Count,
            0
         );
         
        list.Add(subPanel);
    }
    private void clear()
    {
        foreach(SubPanel sp in list) sp.discard();
        list.Clear();
    }
}
