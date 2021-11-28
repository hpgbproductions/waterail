using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodedRailScript : MonoBehaviour
{
    [SerializeField] private Transform RailParentTransform;
    [SerializeField] private Transform RailTransform;
    [SerializeField] private Transform CenterColliderTransform;
    [SerializeField] private Transform AircraftTrackerTransform;

    [SerializeField] private Transform PlatformTransform;

    [SerializeField] private Transform BrightTransform;
    [SerializeField] private Renderer BrightRenderer;
    [SerializeField] private Renderer BrightRendererSurrounding;

    [SerializeField] private RailCubeMaker RailCubeMaker;

    bool WaterActive = true;

    private void Awake()
    {
        ServiceProvider.Instance.DevConsole.RegisterCommand<float>("SetRailGauge", SetRailScale);
        ServiceProvider.Instance.DevConsole.RegisterCommand<float>("SetRailHeading", SetRailHeading);
        ServiceProvider.Instance.DevConsole.RegisterCommand<float>("SetBrightPlaneDepth", SetBrightPlaneDepth);
        ServiceProvider.Instance.DevConsole.RegisterCommand<float, float, float>("SetBrightPlaneColor", SetBrightPlaneColor);
        ServiceProvider.Instance.DevConsole.RegisterCommand<float, float, float>("TranslatePlatform", TranslatePlatform);
        ServiceProvider.Instance.DevConsole.RegisterCommand("ToggleWater", ToggleWater);
        ServiceProvider.Instance.DevConsole.RegisterCommand<float, float>("GenerateRailCubes", GenerateRailCubes);
    }

    private void SetRailScale(float scale)
    {
        RailTransform.localScale = new Vector3(scale, 1f, scale);
    }

    private void SetRailHeading(float heading)
    {
        AircraftTrackerTransform.position = ServiceProvider.Instance.PlayerAircraft.MainCockpitPosition;
        AircraftTrackerTransform.eulerAngles = ServiceProvider.Instance.PlayerAircraft.MainCockpitRotation;
        RailParentTransform.localEulerAngles = new Vector3(0f, heading, 0f);
        CenterColliderTransform.localEulerAngles = new Vector3(0f, heading, 0f);
        ServiceProvider.Instance.PlayerAircraft.MainCockpitRotation = AircraftTrackerTransform.eulerAngles;
        ServiceProvider.Instance.PlayerAircraft.MainCockpitPosition = AircraftTrackerTransform.position;
    }

    private void SetBrightPlaneDepth(float depth)
    {
        BrightTransform.localPosition = new Vector3(0f, -depth, 0f);
    }

    private void SetBrightPlaneColor(float r, float g, float b)
    {
        Color c = new Color(r, g, b, 1f);
        BrightRenderer.material.color = c;
        BrightRendererSurrounding.material.color = c;
    }

    private void ToggleWater()
    {
        if (WaterActive)
            ServiceProvider.Instance.GameWorld.SeaLevel = null;
        else
            ServiceProvider.Instance.GameWorld.SeaLevel = 0f;

        WaterActive = !WaterActive;
    }

    private void TranslatePlatform(float x, float y, float z)
    {
        PlatformTransform.Translate(new Vector3(x, y, z), Space.Self);
    }

    private void GenerateRailCubes(float origin, float range)
    {
        Vector3 rcmPos = RailCubeMaker.transform.localPosition;
        RailCubeMaker.transform.localPosition = new Vector3(rcmPos.x, rcmPos.y, origin);
        RailCubeMaker.RegenerateCubes(range);
    }

    private void OnDestroy()
    {
        ServiceProvider.Instance.DevConsole.UnregisterCommand("SetRailGauge");
        ServiceProvider.Instance.DevConsole.UnregisterCommand("SetRailHeading");
        ServiceProvider.Instance.DevConsole.UnregisterCommand("SetBrightPlaneDepth");
        ServiceProvider.Instance.DevConsole.UnregisterCommand("SetBrightPlaneColor");
        ServiceProvider.Instance.DevConsole.UnregisterCommand("TranslatePlatform");
        ServiceProvider.Instance.DevConsole.UnregisterCommand("ToggleWater");
        ServiceProvider.Instance.DevConsole.UnregisterCommand("GenerateRailCubes");
    }
}
