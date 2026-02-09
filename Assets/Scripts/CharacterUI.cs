using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider stressSlider;

    // 체력바 설정
    public void SetHealthBar(int currHp, int maxHp)
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = currHp;
    }

    public void SetStressBar(int currStress)
    {
        stressSlider.maxValue = 100;
        stressSlider.value = currStress;
    }

    public void UpdateHealth(int currHp)
    {
        hpSlider.value = currHp;
    }
}
