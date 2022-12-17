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
        // Clear previous buttons on info screen
        clear();

        // Get related entity data from the manager
        ProductEntity entity = EntityManager.GetProduct(productType);

        // set info panel's main icon to product image
        setMainImage(productType.ToString());

        // set information text for the product
        string[] productNames = TextManager.bring(TextManager.Content.Products).Split('*');
        string[] resourceNames = TextManager.bring(TextManager.Content.Currency).Split('*');

        string information = 
            productNames[(int)productType] + "\n" + 
            "[ " + entity.bounds.size.x + " x " + entity.bounds.size.y + " ]\n\n";
        
        for(int i = 0; i < entity.cost.Length; i++) information += resourceNames[(int)entity.cost[i].resourceType] + " x" + entity.cost[i].amount + "\n";

        if (entity.required.Length > 0) information += "\n" + TextManager.bring(TextManager.Content.Required) + "\n\n";

        for(int i = 0; i < entity.required.Length; i++) { information += productNames[(int)entity.required[i]]; if (i < entity.required.Length - 1) information += ", "; }

        gameBar.setText(information);

        // generate button for crafting
        
        SubPanel craft = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListItem, this);

        addToList(craft);

        craft.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>("Craft"));

        GameButton craftButton = craft.getInteractable<GameButton>(type.button, "button");

        craftButton.setText(TextManager.bring(TextManager.Content.ProductCraft));

        craftButton.onClick(() => tryCraftable(entity));
        
        // generate productable unit information panels

        string[] unitNames = TextManager.bring(TextManager.Content.Units).Split('*');

        foreach(ProductEntity.Craftable craftable in entity.GetCraftables())
        {
            SubPanel item = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListInfo, this);

            addToList(item);

            item.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>(craftable.type.ToString()));
            item.getInteractable<GameButton>(type.button, "button").setText(unitNames[(int)craftable.type]);
        }
    }
    void tryCraftable(ProductEntity entity)
    {
        BoundsInt bounds = entity.bounds;
        bounds.position = getParentPanel<GamePanel>().getSelectedPosition();

        world.handle<MapHandler>().setTiles(MapHandler.Layer.Settlement, bounds, entity.tiles);

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

        subPanel.transform.localPosition = new Vector3
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
