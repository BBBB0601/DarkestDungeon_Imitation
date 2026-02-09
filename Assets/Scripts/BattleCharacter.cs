using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [Header("Character Base")]
    public HeroData heroData;
    public MonsterData monsterData;
    public bool isHero;

    [Header("UI & Visual")]
    private CharacterUI _characterUI;

    // 현제 채력 추적
    private int _currHp;
    public int CurrHp => _currHp;

    void Start()
    {
        if (heroData != null)
            _currHp = heroData.MaxHp;
        else if(monsterData != null)
            _currHp = monsterData.MaxHp;

        Initialize();
    }

    private void Initialize()
    {
        // 위치 조정
        _characterUI = GetComponentInChildren<CharacterUI>();

        if(_characterUI == null)
        {
            Debug.LogError($"{gameObject.name} 하위에 CharacterUI가 없습니다.");
            return;
        }
        
        // 데이터 주입
        if(isHero && heroData != null)
        {
            _characterUI.SetHealthBar(_currHp, heroData.MaxHp);
        }
        else if(!isHero && monsterData != null)
        {
            _characterUI.SetHealthBar(_currHp, monsterData.MaxHp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
