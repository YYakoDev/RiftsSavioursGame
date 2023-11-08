using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurveTypes
{
    Linear,
    EaseInOut,
        
}
public static class TweenCurveLibrary
{

    static AnimationCurve _linearCurve;
    static AnimationCurve _easeInOutCurve;
    static bool _initialized = false;

    private static Dictionary<CurveTypes, AnimationCurve> _animationCurves = new();

    private static void SetCurves()
    {
        _linearCurve = AnimationCurve.Linear(0,0,1,1);
        _animationCurves.Add(CurveTypes.Linear, _linearCurve);
        _easeInOutCurve = AnimationCurve.EaseInOut(0,0,1,1);
        _animationCurves.Add(CurveTypes.EaseInOut, _easeInOutCurve);


        _initialized = true;
    }

    public static AnimationCurve GetCurve(CurveTypes _curveType)
    {
        if(!_initialized) SetCurves();
        return _animationCurves[_curveType];
    }
}
