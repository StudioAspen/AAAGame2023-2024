using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {
    [SerializeField] Slider healthSlider;
    [SerializeField] Killable killable;


    private void Start() {
        killable.OnHealthChange.AddListener(UpdateBar);
    }
    public void UpdateBar() {
        healthSlider.value = killable.currentHP / killable.maxHP;
    }
}
