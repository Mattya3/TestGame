using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasAspectRatio : MonoBehaviour
{
    [SerializeField]
    private ScreenResolution _referenceResolution;

    [SerializeField]
    private List<AspectRatioFitter> _targetAspectRatioFitters;

    private void Awake()
    {
        _UpdateAspectRatioFitters();
    }

    private void _UpdateAspectRatioFitters()
    {
        float targetAspect = _referenceResolution.AspectRatio;
        foreach (var fitter in _targetAspectRatioFitters)
        {
            if (fitter != null)
                fitter.aspectRatio = targetAspect;
        }
    }
}
