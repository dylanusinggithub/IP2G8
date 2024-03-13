using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPulse : MonoBehaviour
{
    [Header("Variables")]
    public float minIntensity = 1f;  
    public float maxIntensity = 2f; 
    public float pulseSpeed = 1f;

    private Light2D light2D;
    private float baseIntensity;

    void Start()
    {
        //Get the Light
        light2D = GetComponent<Light2D>();

        //Store the intensity
        baseIntensity = light2D.intensity;
    }

    void Update()
    {
        //Calculate intensity
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulseSpeed, 1f));

        //Apply the intensity
        light2D.intensity = baseIntensity * intensity;
    }
}
