using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxhealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void HealthAmount(int health)
    {
        slider.value = health;
    }
}
