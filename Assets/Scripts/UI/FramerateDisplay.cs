using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FramerateDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    private void Update()
    {
        if (displayText.text != null)
        displayText.text = "FPS:" + (int)(1f / Time.deltaTime);
    }
}