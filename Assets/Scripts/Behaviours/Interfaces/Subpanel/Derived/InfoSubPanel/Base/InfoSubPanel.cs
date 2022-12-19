using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSubPanel : SubPanel
{
    // This is a sub-panel of the GamePanel class, the information panel on the left side
    [field: SerializeField] Transform listHolder;
    List<SubPanel> list = new List<SubPanel>();

    GameBar gameBar;
    SubPanel message;
    public override void initialize<T>(World world, T parentPanel)
    {
        base.initialize(world, parentPanel);
        gameBar = getInteractable<GameBar>(Panel.type.bar, "bar");
    }
    public void showProductInfo(EntityProductType productType)
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
        
        foreach(Belonging b in entity.cost) information += resourceNames[(int)b.resourceType] + " x" + b.amount + "\n";

        if (entity.required.Length > 0) information += "\n" + TextManager.bring(TextManager.Content.Required) + "\n\n";

        for(int i = 0; i < entity.required.Length; i++) { information += productNames[(int)entity.required[i]]; if (i < entity.required.Length - 1) information += ", "; }

        gameBar.setText(information);

        // generate button for crafting
        
        SubPanel produce = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListItem, this);

        addToList(produce);

        produce.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>("Craft"));

        GameButton produceButton = produce.getInteractable<GameButton>(type.button, "button");

        produceButton.setText(TextManager.bring(TextManager.Content.ProductProduce));

        produceButton.onClick(() => tryProduct(entity));
        
        // generate craftable unit information panels

        string[] unitNames = TextManager.bring(TextManager.Content.Units).Split('*');

        foreach(ProductEntity.Craftable craftable in entity.GetCraftables())
        {
            SubPanel item = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListInfo, this);

            addToList(item);

            item.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>(craftable.type.ToString()));
            item.getInteractable<GameButton>(type.button, "button").setText(unitNames[(int)craftable.type]);
        }
    }
    void tryProduct(ProductEntity entity)
    {
        // try generating the product
        // check if user has required resources

        getParentPanel<GamePanel>().clearHover();

        if (world.handle<UserHandler>().checkProductRequired(entity))
        {
            if(message != null && message.getPooledObject().isAwake()) message.getPooledObject().sendback();

            message = world.handle<InterfaceHandler>().bringPrompt
                (
                    TextManager.bring(TextManager.Content.ProductProducePrompt).Replace("%P", TextManager.bring(TextManager.Content.Products).Split('*')[(int)entity.productType]), 
                    () => hoverProduct(entity)
                );

            world.handle<AudioHandler>().playSoundActionB();
        }
        else errorInfo(TextManager.Content.NoResources);
    }
    private void hoverProduct(ProductEntity entity)
    {
        // if both user has required sources and accepted the prompt message, enable hover for the product

        GamePanel gp = getParentPanel<GamePanel>();

        gp.setHoverEntity(entity);
        gp.setSelectAction((Vector3Int position) => placeProduct(entity, position));
    }
    private void placeProduct(ProductEntity entity, Vector3Int position)
    {
        // try placing product
        // check if world map is eligible
        // if success, consume resources

        BoundsInt bounds = entity.bounds;
        bounds.position = position;

        if (world.handle<MapHandler>().canPlaceEntity(entity.ground, bounds))
        {
            world.handle<UserHandler>().productCraft(entity, bounds.position);

            getParentPanel<GamePanel>().updateStatusValues();
        }
        else 
        { 
            errorInfo(TextManager.Content.IneligiblePosition);

            message.getInteractable<GameButton>(type.button, "button").onClick
                (
                    () =>
                    {
                        showProductInfo(entity.productType);
                        hoverProduct(entity);
                        message.discard();
                    }
                );
        }
        
    }

    public void showCraftableInfo(ProductEntity entity)
    {
        // show info of user's selected product on the map
        clear();

        // set info panel's main icon to product image
        setMainImage(entity.productType.ToString());

        // set information text for the product
        string[] productNames = TextManager.bring(TextManager.Content.Products).Split('*');
        string[] resourceNames = TextManager.bring(TextManager.Content.Currency).Split('*');

        string information = 
            productNames[(int)entity.productType] + "\n" + 
            TextManager.bring(TextManager.Content.Durability) + entity.durability;

        gameBar.setText(information);
        
        // generate productable unit buttons

        string[] unitNames = TextManager.bring(TextManager.Content.Units).Split('*');

        foreach(ProductEntity.Craftable craftable in entity.GetCraftables())
        {
            SubPanel item = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListItem, this);

            addToList(item);

            item.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>(craftable.type.ToString()));

            item.getInteractable<GameButton>(type.button, "button").setText
                (
                    TextManager.bring(TextManager.Content.ProductCraft) + "\n" +
                    unitNames[(int)craftable.type]
                );

            item.getInteractable<GameButton>(type.button, "button").onClick(() => tryCraftable(entity, craftable));
        }
    }

    private void tryCraftable(ProductEntity entity, ProductEntity.Craftable craftable)
    {
        if (world.handle<UserHandler>().checkCraftableRequired(craftable))
        {
            if(message != null && message.getPooledObject().isAwake()) message.getPooledObject().sendback();

            string[] resourceNames = TextManager.bring(TextManager.Content.Currency).Split('*');

            string unitCost = "\n\n";
            foreach(Belonging b in craftable.required) unitCost += resourceNames[(int)b.resourceType] + " x" + b.amount + "\n";

            message = world.handle<InterfaceHandler>().bringPrompt
                (
                    TextManager.bring(TextManager.Content.ProductCraftPrompt).Replace("%P", TextManager.bring(TextManager.Content.Units).Split('*')[(int)craftable.type]) + unitCost, 
                    () => placeCraftable(entity, craftable)
                );

            world.handle<AudioHandler>().playSoundActionA();
        }
        else errorInfo(TextManager.Content.NoResources);
    }

    private void placeCraftable(ProductEntity entity, ProductEntity.Craftable craftable)
    {
        // try to generate selected unit from the built product
        if (entity != null && entity.durability > 0 && world.handle<UserHandler>().checkCraftableRequired(craftable))
        {
            UnitEntity unitEntity = EntityManager.GetUnit(craftable.type);

            Vector3Int[] positions = entity.GetSpawnPositions();
            
            bool spawned = false;

            foreach(Vector3Int p in positions)
            {
                Vector3Int position = p + entity.position;

                BoundsInt bounds = unitEntity.bounds;
                bounds.position = position;

                if (world.handle<MapHandler>().canPlaceEntity(unitEntity.ground, bounds))
                {
                    // position found, can spawn the unit
                    
                    world.handle<UserHandler>().unitCraft(craftable, unitEntity, position);

                    getParentPanel<GamePanel>().updateStatusValues();

                    getParentPanel<GamePanel>().setSelectedPosition(position);

                    world.handle<AudioHandler>().playSoundButtonA();

                    spawned = true;

                    break;
                }
            }

            if (!spawned) errorInfo(TextManager.Content.IneligiblePosition, true);
        }
        else errorInfo(TextManager.Content.NoResources);
    }

    public void showUnitInfo(UnitEntity entity)
    {
        // show info of user's selected unit on the map
        clear();

        // set info panel's main icon to unit image
        setMainImage(entity.unitType.ToString());

        // set information text for the unit
        string[] unitNames = TextManager.bring(TextManager.Content.Units).Split('*');
        string[] resourceNames = TextManager.bring(TextManager.Content.Currency).Split('*');
        string[] operationNames = TextManager.bring(TextManager.Content.Operations).Split('*');

        string information = 
            unitNames[(int)entity.unitType] + "\n" + 
            TextManager.bring(TextManager.Content.Durability) + entity.durability + "\n" +
            TextManager.bring(TextManager.Content.Power) + entity.cost[0].amount;

        gameBar.setText(information);
        
        // generate unit operation buttons

        foreach(UnitEntity.Operation operation in entity.operation)
        {
            SubPanel item = world.handle<InterfaceHandler>().bringSubPanel<SubPanel, InfoSubPanel>(SubPanelType.ListItem, this);

            addToList(item);

            item.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.loadFromCache<Sprite>(operation.type.ToString()));

            item.getInteractable<GameButton>(type.button, "button").setText
                (
                    operationNames[(int)operation.type]
                );

            item.getInteractable<GameButton>(type.button, "button").onClick(() => tryUnitOperation(entity, operation));
        }
    }
    void tryUnitOperation(UnitEntity entity, UnitEntity.Operation operation)
    {
        // prompt operation type and set its function
        if (entity != null && entity.durability > 0)
        {
            if(message != null && message.getPooledObject().isAwake()) message.getPooledObject().sendback();

            string operationName = TextManager.bring(TextManager.Content.Operations).Split('*')[(int)operation.type];
            string[] resourceNames = TextManager.bring(TextManager.Content.Currency).Split('*');

            string operationCost = operationName + ": \n\n";
            foreach(Belonging b in operation.required) operationCost += resourceNames[(int)b.resourceType] + " x" + b.amount + "\n";

            message = world.handle<InterfaceHandler>().bringPrompt
                (
                    operationCost, 
                    () => castOperation(entity, operation)
                );

            world.handle<AudioHandler>().playSoundActionA();
        }
        else errorInfo(TextManager.Content.NoResources);

    }
    void castOperation(UnitEntity entity, UnitEntity.Operation operation)
    {
        if (operation.type == EntityUnitOperationType.Move)
        {
            getParentPanel<GamePanel>().setHoverEntity(EntityManager.GetUnit(entity.unitType));
            getParentPanel<GamePanel>().setSelectAction
                (
                    (Vector3Int position) => 
                        {
                            // fill for movement command
                        }
                );
        }
    }
    private void setMainImage(string imageName)
    {
        // set info panel's main sprite
        gameBar.enable(true);
        gameBar.setSprite(ResourceManager.loadFromCache<Sprite>(imageName));
    }
    private void addToList(SubPanel subPanel)
    {
        // add a bar or a button to the info panel, reversed order

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
    public void clear()
    {
        // clear info panel buttons and the containing list
        gameBar.enable(false);
        foreach(SubPanel sp in list) sp.discard();
        list.Clear();
        getParentPanel<GamePanel>().clearHover();
    }
    private void errorInfo(TextManager.Content content, bool force = false)
    {
        if (message == null || !message.getPooledObject().isAwake() || force)
        {
            // failure message
            if (force && message != null) message.getPooledObject().sendback();

            message = world.handle<InterfaceHandler>().bringMessage(TextManager.bring(content));

            world.handle<AudioHandler>().playSoundButtonB();
        }
    }
}
