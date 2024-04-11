using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFramerate : MonoBehaviour
{
    [SerializeField] private int maxFramerate = 60;
    private void Awake()
    {
        Application.targetFrameRate = maxFramerate;
    }
}
