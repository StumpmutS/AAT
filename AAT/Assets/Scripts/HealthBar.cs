using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        healthBarPercentText.text = percent + "%";
    }

    private void OnDestroy()
    {
        healthScript.OnHealthChanged -= UpdateHealthBar;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
