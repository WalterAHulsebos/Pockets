using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class implements the abstraction of the parallax system.
/// </summary>
public abstract class UIParallax : MonoBehaviour
{
    
    [System.Serializable]
    public struct ParallaxElement
    {
        // Image source for the parallax layer
        public RawImage image;

        // You can select the required range
        [Range(0, 100)]
        public float intensity;

    }
    
    // Layers of parallax.They can be configured from the inspector
    public ParallaxElement[] ParallaxLayers;

    protected bool _isInitialize = true;

    /// <summary>
    /// The main method that implemented parallax.
    /// </summary>
    /// <param name="direction"> Parallax movement direction </param>
    protected void Parallaxing(Vector3 direction) {

        if (!_isInitialize)
            return;

        if (ParallaxLayers.Length <= 0) {
            Debug.LogError("UIParallax! Parallax layers are not found");
            return;
        }            

        for (int i = 0; i < ParallaxLayers.Length; i++) {

            ParallaxElement parallaxLayer = ParallaxLayers[i];

            Vector2 offset = direction * Time.deltaTime * parallaxLayer.intensity;

            if(ParallaxLayers[i].image == null) {
                Debug.LogError("ParallaxElement with id : " + i + " error! Not found Image!");
                continue;
            }

            float x = ParallaxLayers[i].image.uvRect.x + offset.x;
            float y = ParallaxLayers[i].image.uvRect.y + offset.y;

            if (x > 1 || x < -1)
                x = 0;

            if (y > 1 || y < -1)
                y = 0;

            ParallaxLayers[i].image.uvRect = new Rect(x, y, ParallaxLayers[i].image.uvRect.width, ParallaxLayers[i].image.uvRect.height);

        }

    }



}
