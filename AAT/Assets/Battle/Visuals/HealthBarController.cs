using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Scripts;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Selectable selectable;
    [SerializeField] private GameObject iAttackSystem;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image healthBarLazyFill;
    //[SerializeField] private TMP_Text healthBarPercentText;
    [SerializeField] private SpringController spring;
    [SerializeField] private float alphaResetTime;
    [SerializeField] private float lazySpeed;
    [SerializeField] private float healthScaleMultiplier;
    [SerializeField] private float healthScaleMin = 300;
    [SerializeField] private float healthScaleMax = 700;

    private IAttackSystem _attackSystem;
    private Coroutine _coResetSpring;

    private void Start()
    {
        health.OnHealthPercentChanged += UpdateHealthBar;
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(health.MaxHealth * healthScaleMultiplier, healthScaleMin, healthScaleMax));
        _attackSystem = iAttackSystem.GetComponent<IAttackSystem>();
        /*_attackSystem.OnAttack += SetSpring; todo
        _attackSystem.OnCrit += SetSpring;*/
    }

    private void UpdateHealthBar(float percent)
    {
        healthBarFill.fillAmount = percent;
        SetSpring();
        //healthBarPercentText.text = (percent * 100).ToString("F2") + "%";
        if (_coResetSpring != null) StopCoroutine(_coResetSpring);
        _coResetSpring = StartCoroutine(CoResetSpring());
    }
    
    private void SetSpring(GameObject _ = default, float u = default) => spring.SetTarget(1);

    private IEnumerator CoResetSpring()
    {
        yield return new WaitForSeconds(alphaResetTime);
        
        spring.SetTarget(0);
    }

    private void OnDestroy()
    {
        if (health != null) health.OnHealthPercentChanged -= UpdateHealthBar;
        /*if (_attackSystem != null) _attackSystem.OnAttack -= SetSpring;
        if (_attackSystem != null) _attackSystem.OnCrit -= SetSpring;*/
    }

    private void LateUpdate()
    {
        healthBar.transform.LookAt(MainCameraRef.Cam.transform);
        healthBar.transform.Rotate(0, 180, 0);
        healthBarLazyFill.fillAmount = Mathf.Lerp(healthBarLazyFill.fillAmount, healthBarFill.fillAmount, Time.deltaTime * lazySpeed);
    }
}
