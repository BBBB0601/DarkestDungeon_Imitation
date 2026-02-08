using UnityEngine;

public enum Type    // 몬스터 타입
{
    Hero,     // 플레이어가 조종할 영웅
    Unholy,     // 불경
    Human,      // 인간
    Eldritch,   // 이물
    Beast,      // 짐승
    Stone       // 석재
}

[CreateAssetMenu(fileName = "NewEntity", menuName = "Data/Entity")]
public class EntityData : ScriptableObject
{
    // 적 유형 또는 아군 지정
    [SerializeField] private Type _type;
    public Type EntityType => _type;

    // 이름
    [SerializeField] private string _entityName;
    public string EntityName => _entityName;

    [Header("기본 능력치")]

    // 최대 체력
    [SerializeField] private int _maxHp;
    public int MaxHp { get => _maxHp; private set => _maxHp = value; }

    // 현제 체력
    [SerializeField] private int _currHp;
    public int CurrentHp { get => _currHp; private set => _currHp = value; }

    // 속도
    [SerializeField] private int _speed;
    public int Speed => _speed;

    // 레벨
    [SerializeField] private int _level;
    public int Level => _level;

    // 회피
    [SerializeField] private int _dodge;
    public int Dodge => _dodge;

    // 방어력
    [SerializeField] private float _protection;
    public float Protection => _protection;

    // 명중률 보정
    [SerializeField] private int _correction;
    public int Correction => _correction;

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

    [SerializeField] private float _poisonResist;
    public float PoisonResist => _poisonResist;

    [SerializeField] private float _diseaseResist;
    public float DiseaseResist => _diseaseResist;

    [SerializeField] private float _deathblowResist;         // 몬스터 유형일 경우 0, 사용하지 않음
    public float DeathblowResist => _deathblowResist;

    [SerializeField] private float _moveResist;
    public float MoveResist => _moveResist;

    [SerializeField] private float _bleedResist;
    public float BleedResist => _bleedResist;

    [SerializeField] private float _debuffResist;
    public float DebuffResist => _debuffResist;

    [SerializeField] private float _trapDisable;       // 몬스터 유형일 경우 0, 사용하지 않음
    public float TrapDisable => _trapDisable;

    [Header("사용 기술")]
    public SkillData[] skills;

    // 오버라이드 | 영웅일 경우
    public void Initialize(string name, int hp, int dodge, float prot, int speed, int correct,
                           float crit, int minAtk, int maxAtk, float[] resists, SkillData[] skillAssets)
    {
        _entityName = name;
        _type = Type.Hero;
        _maxHp = hp;    _currHp = hp;
        _dodge = dodge;
        _protection = prot;
        _speed = speed;
        _correction = correct;
        _critical = crit;
        _minDamage = minAtk;
        _maxDamage = maxAtk;

        // 저항력 순서: 기절, 중독, 질병, 죽음의일격, 이동, 출혈, 약화, 함정
        _stunResist = resists[0];
        _poisonResist = resists[1];
        _diseaseResist = resists[2];
        _deathblowResist = resists[3];
        _moveResist = resists[4];
        _bleedResist = resists[5];
        _debuffResist = resists[6];
        _trapDisable = resists[7];

        skills = skillAssets;
    }

    // 오버라이드 | 몬스터일 경우
    public void Initialize(string name, Type type, int hp, int dodge, float prot, int speed, int correct,
                           float crit, int minAtk, int maxAtk, float[] resists, SkillData[] skillAssets)
    {
        _entityName = name;
        _type = type;
        _maxHp = hp; _currHp = hp;
        _dodge = dodge;
        _protection = prot;
        _speed = speed;
        _correction = correct;
        _critical = crit;
        _minDamage = minAtk;
        _maxDamage = maxAtk;

        // 저항력 순서: 기절, 중독, 질병, 죽음의일격, 이동, 출혈, 약화, 함정
        _stunResist = resists[0];
        _poisonResist = resists[1];
        _diseaseResist = resists[2];
        _deathblowResist = resists[3];
        _moveResist = resists[4];
        _bleedResist = resists[5];
        _debuffResist = resists[6];
        _trapDisable = resists[7];

        skills = skillAssets;
    }
}