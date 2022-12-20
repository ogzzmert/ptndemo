// Mert Oguz - 2022 demo project

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private Transform mainCamera;
    [field: SerializeField, Range(0.1f, 10f)] private float cameraSpeed;

    bool isAvailable = false;
    public void generate(WorldType worldType, int worldIndex)
    {
        isAvailable = true;
    }
    public void degenerate()
    {
        isAvailable = false;
        clear();
    }
    private void clear()
    {
        mainCamera.localPosition = Vector2.zero;
    }
    public void pushCamera(Vector3 direction)
    {
        mainCamera.Translate(0.01f * cameraSpeed * direction.x, 0.01f * cameraSpeed * direction.y, 0);
    }
    public void runMenuCamera() { StartCoroutine(iterateMenuCamera()); }
    IEnumerator iterateMenuCamera()
    {
        int counter = 0;
        int direction = 0;
        float step = 0.1f * cameraSpeed * 0.015f;

        while(isAvailable)
        {
            switch(direction)
            {
                case 0:
                    mainCamera.Translate(step, step, 0);
                    break;
                case 1:
                    mainCamera.Translate(-step, -step, 0);
                    break;
            }

            if (counter == 1000) direction = direction < 1 ? direction + 1 : 0;
            counter = counter < 1000 ? counter + 1 : 0;

            if (isAvailable) yield return new WaitForSeconds(0.015f);
            else break;
        }

        clear();
    }
}
