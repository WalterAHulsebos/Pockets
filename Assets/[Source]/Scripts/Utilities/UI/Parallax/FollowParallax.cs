using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This parallax is suitable for working with moving objects.
/// Inertia depends on the movement of the target.
/// </summary>
public class FollowParallax : UIParallax
{
    // The object behind the movements of which will move parallax
    public GameObject target;

    // Old position target object
    private Vector3 _targetOldPosition;

    private void Start()
    {
        if (target == null)
            Debug.LogWarning("Warning. Parallax will be static. Target object not found. Please make sure component UIParallax is configured correctly.");

        if (parallaxLayers.Length > 0) return;
        Debug.LogWarning(" The problem of UI Parallax initialization. Parallax layers are not found. Please make sure component UIParallax is configured correctly.");
        initialized = false;

    }

	private void Update ()
    {
        if (target.transform.position == _targetOldPosition) return;

        //Vector3 parallaxDirection = target.transform.position - _targetOldPosition;
        Parallaxing(target.transform.position);

        //_targetOldPosition = target.transform.position;	
	}
}
