using UnityEngine;
using System.Collections;

public class BubbleMovement : MonoBehaviour
{
    [Header("Base Random Ranges")]
    [Tooltip("Minimum vertical speed")]
    public float minVerticalSpeed = 0.3f;

    [Tooltip("Maximum vertical speed")]
    public float maxVerticalSpeed = 0.7f;

    [Tooltip("Minimum horizontal sway amplitude")]
    public float minSwayAmplitude = 0.3f;

    [Tooltip("Maximum horizontal sway amplitude")]
    public float maxSwayAmplitude = 0.7f;

    [Tooltip("Minimum horizontal sway frequency")]
    public float minSwayFrequency = 0.5f;

    [Tooltip("Maximum horizontal sway frequency")]
    public float maxSwayFrequency = 1.5f;

    [Header("Speed Variation Over Time")]
    [Tooltip("Amplitude of the random speed variation (applied to both vertical and sway).")]
    public float speedVariationAmplitude = 0.1f;

    [Tooltip("Frequency of the random speed variation.")]
    public float speedVariationFrequency = 0.5f;

    public float modifiers = 10;

    // Chosen at runtime
    private float chosenVerticalSpeed;
    private float chosenSwayAmplitude;
    private float chosenSwayFrequency;
    private float swayPhase;               // Random offset to shift our sine wave

    private Vector3 initialPosition;
    private float elapsedTime = 0f;

    private void Start()
    {
        // Store initial position
        initialPosition = transform.position;

        // Randomly pick a vertical speed and horizontal sway parameters
        chosenVerticalSpeed = Random.Range(minVerticalSpeed, maxVerticalSpeed);
        chosenSwayAmplitude = Random.Range(minSwayAmplitude, maxSwayAmplitude);
        chosenSwayFrequency = Random.Range(minSwayFrequency, maxSwayFrequency);

        // Random starting phase to ensure the bubble doesn't start at the same sine position
        swayPhase = Random.Range(0f, 2f * Mathf.PI);

        // Start the floating coroutine
        StartCoroutine(FloatAndSway());
    }

    private IEnumerator FloatAndSway()
    {
        while (true)
        {
            elapsedTime += Time.deltaTime;

            // ---------------------------------------------------------
            // 1) Random Variation Over Time
            // ---------------------------------------------------------
            // Create small fluctuations in speed using a sine wave.
            // This variation is added on top of the chosen speed/amplitude.
            float verticalSpeedVariation = speedVariationAmplitude
                * Mathf.Sin(speedVariationFrequency * elapsedTime);

            float swayAmplitudeVariation = speedVariationAmplitude
                * Mathf.Sin(speedVariationFrequency * elapsedTime + Mathf.PI * 0.5f);
            // Phase shift for variety—feel free to remove or change.

            // ---------------------------------------------------------
            // 2) Calculate Actual Speeds with Variation
            // ---------------------------------------------------------
            float currentVerticalSpeed = chosenVerticalSpeed + verticalSpeedVariation;
            float currentSwayAmplitude = chosenSwayAmplitude + swayAmplitudeVariation;

            // ---------------------------------------------------------
            // 3) Update Position
            // ---------------------------------------------------------
            // Sway (x-direction) with random phase offset
            float swayOffset = Mathf.Sin((elapsedTime + swayPhase) * chosenSwayFrequency)
                               * currentSwayAmplitude * modifiers;

            // Float upward over time
            float upOffset = currentVerticalSpeed * elapsedTime* modifiers;

            // Set final position
            transform.position = new Vector3(
                initialPosition.x + swayOffset,
                initialPosition.y + upOffset,
                initialPosition.z
            );

            yield return null;
        }
    }
}
