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

        SubPanel craft = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListItem, this);

        craft.transform.SetParent(listHolder);
        craft.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        craft.GetComponent<RectTransform>().offsetMin = Vector2.zero;

        craft.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>("Craft"));

        GameButton craftButton = craft.getInteractable<GameButton>(type.button, "button");
        craftButton.setText("Craft!");
        craftButton.onClick(() => tryCraftable(productType));

        // productables
    }
    public void tryCraftable(EntityProductType productType)
    {
        Debug.Log(productType);
    }
    private void setMainImage(string imageName)
    {
        gameBar.setSprite(ResourceManager.loadFromCache<Sprite>(imageName));
        gameBar.setText(imageName);  // change later with textmanager.getname
    }
    private void clear()
    {
        foreach(SubPanel sp in list) sp.discard();
        list.Clear();
    }
}
