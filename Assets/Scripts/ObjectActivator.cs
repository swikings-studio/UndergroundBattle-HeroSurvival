using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    private GameObject currentActivatedObject;

    private void OnEnable()
    {
        bool isActivateObject = System.Convert.ToBoolean(Random.Range(0, 2));
        if (currentActivatedObject == null)
        {
            if (isActivateObject)
            {
                currentActivatedObject = gameObjects[Random.Range(0, gameObjects.Length)];
                currentActivatedObject.SetActive(true);
            }
        }
        else
        {
            if (isActivateObject)
            {
                currentActivatedObject.SetActive(false);
                currentActivatedObject = gameObjects[Random.Range(0, gameObjects.Length)];
                currentActivatedObject.SetActive(true);
            }
            else
            {
                currentActivatedObject.SetActive(false);
                currentActivatedObject = null;
            }
        }
    }
}