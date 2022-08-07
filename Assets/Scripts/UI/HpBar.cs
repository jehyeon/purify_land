using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private Transform _target;
    private Vector3 _offset;

    private void Update()
    {
        if (_target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        this.transform.position = Camera.main.WorldToScreenPoint(_target.position) + _offset;
    }

    public void SetTarget(Transform target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
    }

    public void UpdateHpBar(float percent)
    {
        slider.value = percent;

        if (slider.value == 0 || slider.value == 1)
        {
            Destroy(this.gameObject);
        }
    }
}
