using System.Collections.Generic;
using UnityEngine;

public enum MonsterType 
{
    Human,      // 인간
    Beast,      // 짐승(야수)
    Eldrich,    // 이물
    Unholy,     // 불경
    Stone,      // 석재
    Woodwork    // 목공품
}

[CreateAssetMenu(fileName = "NewMonster", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private string _monsterName;   // 몬스터 이름
    public string MonsterName => _monsterName;

    [SerializeField] private int _maxHp;            // 최대 체력
    public int MaxHp => _maxHp;

    [SerializeField] private float _dodge;          // 회피
    public float Dodge => _dodge;

    [SerializeField] private float _protection;     // 방어력
    public float Protection => _protection;

    [SerializeField] private int _speed;            // 속도
    public float Speed => _speed;

    [SerializeField] private List<MonsterType> _monsterTypes;  // 몬스터 유형
    public List<MonsterType> MonsterTypes => _monsterTypes;


    // __여기 밑에서 부터 저항력__

    [SerializeField] private float _stunResist;         // 기절 저항력
    public float StunResist => _stunResist;

    [SerializeField] private float _poisonResist;       // 중독 저항력
    public float PoisonResist => _poisonResist;

    [SerializeField] private float _bleedResist;        // 출혈 저항력
    public float BleedResist => _bleedResist;

    [SerializeField] private float _debuffResist;       // 약화 저항력
    public float DebuffResist => _debuffResist;

    [SerializeField] private float _moveResist;         // 이동 저항력
    public float MoveResist => _moveResist;

    [Header("사용 기술")]
    public MonsterSkillData[] skills;

    public void Initialize(string name, int hp, float dodge, float prot, int speed,
                           List<MonsterType> types, float[] resists, MonsterSkillData[] skillAssets)
    {
        // 몬스터 기본 능력치
        _monsterName = name;
        _maxHp = hp;
        _dodge = dodge;
        _protection = prot;
        _speed = speed;
        _monsterTypes = types;

        // __여기 밑으로 저항력__

        _stunResist = resists[0];
        _poisonResist = resists[1];
        _bleedResist = resists[2];
        _debuffResist = resists[3];
        _moveResist = resists[4];

        // 사용 스킬
        skills = skillAssets;
    }
}
