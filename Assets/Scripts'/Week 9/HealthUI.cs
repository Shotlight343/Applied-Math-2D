using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public RunnerPlayer player;
    public Slider slider;
    public TextMeshProUGUI label;

    private void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = player.maxHealth;
    }

    private void Update()
    {
        slider.value = player.health;
        if (label != null)
        {
            label.text = "HP " + Mathf.CeilToInt(player.health) + " / " + Mathf.RoundToInt(player.maxHealth);
        }
    }
}