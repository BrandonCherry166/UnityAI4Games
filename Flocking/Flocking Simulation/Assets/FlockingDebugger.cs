using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class FlockingDebugger : MonoBehaviour
{
    public FlockingManager fManager;

    [Header("Sliders")]
    public Slider radiusSlider;
    public Slider kCohesionSlider;
    public Slider kAlignmentSlider;
    public Slider kSeparationSlider;
    public Slider maxForceSlider;
    public Slider maxSpeedSlider;

    [Header ("Boid Count")]
    public TMP_InputField boidCountInput;
    // Start is called before the first frame update
    void Start()
    {
        radiusSlider.value = fManager.radius;
        kCohesionSlider.value = fManager.kCohesion;
        kAlignmentSlider.value = fManager.kAlignment;
        kSeparationSlider.value = fManager.kSeparation;
        maxForceSlider.value = fManager.maxForce;
        maxSpeedSlider.value = fManager.maxSpeed;

        boidCountInput.text = fManager.boids.Count.ToString();

        //Listeners
        radiusSlider.onValueChanged.AddListener(val => fManager.radius = val);
        kCohesionSlider.onValueChanged.AddListener(val => fManager.kCohesion = val);
        kAlignmentSlider.onValueChanged.AddListener(val => fManager.kAlignment = val);
        kSeparationSlider.onValueChanged.AddListener(val => fManager.kSeparation = val);
        maxForceSlider.onValueChanged.AddListener(val => fManager.maxForce = val);
        maxSpeedSlider.onValueChanged.AddListener(val => fManager.maxSpeed = val);

        boidCountInput.onEndEdit.AddListener(ChangeBoidCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeBoidCount(string newCount)
    {
        if (int.TryParse(newCount, out int count))
        {
            int current = fManager.boids.Count;
            if (count > current)
            {
                for (int i = 0; i < count - current; i++)
                    fManager.SpawnBoid();
            }
            else if (count < current)
            {
                for (int i = 0; i < current - count; i++)
                    fManager.RemoveBoid();
            }
        }
        else
        {
            Debug.LogWarning("Invalid number input for boid count");
        }
    }
}
