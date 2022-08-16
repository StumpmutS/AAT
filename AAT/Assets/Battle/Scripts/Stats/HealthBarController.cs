using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Scripts;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image healthBarLazyFill;
    //[SerializeField] private TMP_Text healthBarPercentText;
    [SerializeField] private SpringController spring;
    [SerializeField] private float alphaResetTime;
    [SerializeField] private float lazySpeed;
    [SerializeField] private float healthScaleMultiplier;

    private Coroutine _coResetSpring;

    private void Start()
    {
        health.OnHealthPercentChanged += UpdateHealthBar;
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health.MaxHealth * healthScaleMultiplier);
    }

    private void UpdateHealthBar(float percent)
    {
        healthBarFill.fillAmount = percent;
        //healthBarPercentText.text = (percent * 100).ToString("F2") + "%";
        spring.SetTarget(1);
        if (_coResetSpring != null) StopCoroutine(_coResetSpring);
        StartCoroutine(CoResetSpring());
    }

    private IEnumerator CoResetSpring()
    {
        yield return new WaitForSeconds(alphaResetTime);
        
        spring.SetTarget(0);
    }

    private void OnDestroy()
    {
        health.OnHealthPercentChanged -= UpdateHealthBar;
    }

    private void LateUpdate()
    {
        healthBar.transform.LookAt(MainCameraRef.Cam.transform);
        healthBar.transform.Rotate(0, 180, 0);
        healthBarLazyFill.fillAmount = Mathf.Lerp(healthBarLazyFill.fillAmount, healthBarFill.fillAmount, Time.deltaTime * lazySpeed);
    }
}
