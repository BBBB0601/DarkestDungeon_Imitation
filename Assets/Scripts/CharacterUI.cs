using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;

    // 체력바 설정
    public void SetHealthBar(int currHp, int maxHp)
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = currHp;
    }

    public void UpdateHealth(int currHp)
    {
        hpSlider.value = currHp;
    }
}
