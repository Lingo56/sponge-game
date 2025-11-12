using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FramerateCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float updateInterval = 0.5f; // How often to update the text
    [SerializeField] private int decimals = 1; // How many decimal places

    private float accumulatedTime = 0f;
    private int frames = 0;
    private float timeLeft;

    private void Start()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        // Count frames
        timeLeft -= Time.deltaTime;
        accumulatedTime += Time.unscaledDeltaTime;
        frames++;

        // Update text when interval has passed
        if (timeLeft <= 0f)
        {
            float fps = frames / accumulatedTime;
            fpsText.text = $"FPS: {fps.ToString($"F{decimals}")}";
            
            // Reset counters
            frames = 0;
            accumulatedTime = 0f;
            timeLeft = updateInterval;
        }
    }
}
