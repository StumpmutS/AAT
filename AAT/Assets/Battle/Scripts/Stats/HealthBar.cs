using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Scripts;

[RequireComponent(typeof(IHealth))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthBarPercentText;

    private IHealth healthScript;

    private void Start()
    {
        healthScript = GetComponent<IHealth>();
        healthScript.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(float percent)
    {
        healthBar.fillAmount = percent;
        healthBarPercentText.text = percent * 100 + "%";
    }

    private void OnDestroy()
    {
        healthScript.OnHealthChanged -= UpdateHealthBar;
    }

    private void LateUpdate()
    {
        healthBar.transform.LookAt(MainCameraRef.Cam.transform);
        healthBar.transform.Rotate(0, 180, 0);
    }
}
