using UnityEngine;

public enum Type    // 몬스터 타입
{
    Player,     // 플레이어가 조종할 영웅
    Unholy,     // 불경
    Human,      // 인간
    Eldritch,   // 이물
    Beast,      // 짐승
    Stone       // 석재
}

[CreateAssetMenu(fileName = "NewEntity", menuName = "Data/Entity")]
public class EntityData : ScriptableObject
{
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
    public int Dodge { get => _dodge; private set => _dodge = value; }

    // 방어력
    [SerializeField] private float _protection;
    public float Protection { get => _protection; private set => _protection = value; }

    // 명중률 보정
    [SerializeField] private int _correction;
    public int Correction { get => _correction; private set => _correction = value; }

    // 치명타 확률
    [SerializeField] private float _critical;
    public float Critical { get => _critical; private set => _critical = value; }

    // 최소 데미지
    [SerializeField] private int _minDamage;
    public int MinDamage { get => _minDamage; private set => _minDamage = value; }

    // 최대 데미지
    [SerializeField] private int _maxDamage;
    public int MaxDamage { get => _maxDamage; private set => _maxDamage = value; }

    [Header("저항력")]
    public float stunResist;
    public float poisonResist;
    public float diseaseResist;
    public float deathblowResist;         // 몬스터 유형일 경우 0, 사용하지 않음
    public float moveResist;
    public float bleedResist;
    public float debuffResist;
    public float trapDisable;       // 몬스터 유형일 경우 0, 사용하지 않음

    [Header("사용 기술")]
    public SkillData[] skills;
}