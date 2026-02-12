using UnityEngine;

[CreateAssetMenu(fileName = "NewHero", menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    // 이름
    [SerializeField] private string _heroName;
    public string HeroName => _heroName;

    [Header("기본 능력치")]

    // 최대 체력
    [SerializeField] private int _maxHp;
    public int MaxHp => _maxHp;

    // 속도
    [SerializeField] private int _speed;
    public int Speed => _speed;

    // 레벨
    [SerializeField] private int _level;
    public int Level => _level;

    // 회피
    [SerializeField] private float _dodge;
    public float Dodge => _dodge;

    // 방어력
    [SerializeField] private float _protection;
    public float Protection => _protection;

    // 명중률 보정
    [SerializeField] private float _correction;
    public float Correction => _correction;

    // 치명타 확률
    [SerializeField] private float _critical;
    public float Critical => _critical;

    // 최소 데미지
    [SerializeField] private int _minDamage;
    public int MinDamage => _minDamage;

    // 최대 데미지
    [SerializeField] private int _maxDamage;
    public int MaxDamage => _maxDamage;

    [Header("저항력")]

    [SerializeField] private float _stunResist;
    public float StunResist => _stunResist;

    [SerializeField] private float _blightResist;
    public float BlightResist => _blightResist;

    [SerializeField] private float _diseaseResist;
    public float DiseaseResist => _diseaseResist;

    [SerializeField] private float _deathblowResist;
    public float DeathblowResist => _deathblowResist;

    [SerializeField] private float _moveResist;
    public float MoveResist => _moveResist;

    [SerializeField] private float _bleedResist;
    public float BleedResist => _bleedResist;

    [SerializeField] private float _debuffResist;
    public float DebuffResist => _debuffResist;

    [SerializeField] private float _trapDisable;       
    public float TrapDisable => _trapDisable;

    [Header("사용 기술")]
    public HeroSkillData[] skills;

    public void Initialize(string name, int hp, float dodge, float prot, int speed, float correct,
                           float crit, int minAtk, int maxAtk, float[] resists, HeroSkillData[] skillAssets)
    {
        _heroName = name;
        _maxHp = hp;
        _dodge = dodge;
        _protection = prot;
        _speed = speed;
        _correction = correct;
        _critical = crit;
        _minDamage = minAtk;
        _maxDamage = maxAtk;

        // 저항력 순서: 기절, 중독, 질병, 죽음의일격, 이동, 출혈, 약화, 함정
        _stunResist = resists[0];
        _blightResist = resists[1];
        _diseaseResist = resists[2];
        _deathblowResist = resists[3];
        _moveResist = resists[4];
        _bleedResist = resists[5];
        _debuffResist = resists[6];
        _trapDisable = resists[7];

        skills = skillAssets;
    }
}
