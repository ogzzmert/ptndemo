using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private Canvas canvas;
    [field: SerializeField] private GameInterface[] panelList;

    [Serializable]
    private class GameInterface
    {
        public WorldType type;
        public GameObject panel;
    } 

    Dictionary<WorldType, GameObject> panels = new Dictionary<WorldType, GameObject>();
    Panel panel;

    protected override void initialize()
    {
        foreach(GameInterface gi in panelList) panels.Add(gi.type, gi.panel);
    }
    public void generate(WorldType worldType, int worldIndex)
    {
        if (panel != null)
        {
            panel.discard();

            world.destroy(panel.gameObject);

            panel = null;

        }

        if (panels.ContainsKey(worldType))
        {

            panel = world.spawn(panels[worldType]).GetComponent<Panel>();
            
            panel.gameObject.transform.SetParent(this.canvas.gameObject.transform);
            panel.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            panel.gameObject.transform.localScale = new Vector3(1, 1, 1);

            panel.initialize(world, worldIndex);

        }
    }

}
