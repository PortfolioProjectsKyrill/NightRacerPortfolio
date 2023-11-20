using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.HighDefinition;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Camera mainCamera;
    private HDAdditionalCameraData HDCData;

    private void Start()
    {
        HDCData = mainCamera.GetComponent<HDAdditionalCameraData>();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);
    }

    public void SetQual(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetFieldOfView(float Fov)
    {
        mainCamera.fieldOfView = Fov;
    }

    public void SetAntiAliasing(int antiAliasingIndex)
    {
        switch (antiAliasingIndex)
        {
            case 0:
                HDCData.antialiasing = HDAdditionalCameraData.AntialiasingMode.None;
                break;
            case 1:
                HDCData.antialiasing = HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;
                break;
            case 2:
                HDCData.antialiasing = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing;
                break;
            case 3:
                HDCData.antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                break;
        }
    }
}
