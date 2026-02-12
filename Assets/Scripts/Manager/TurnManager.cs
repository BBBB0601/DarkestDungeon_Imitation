using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [Header("턴을 표시할 텍스트")]
    public TMP_Text turnNumber;

    // 현제 게임 상에 존재하는 캐릭터(엔티티)들의 리스트
    [SerializeField] private List<BattleCharacter> entityList = new List<BattleCharacter>();

    private int turn = 0;   // 턴 수를 기록

    /// <summary>
    /// 부를때마다 턴 수 증가, 턴을 나타내는 텍스트에 증가된 턴을 반영
    /// </summary>
    public void SetTurnText()
    {
        turnNumber.text = (++turn).ToString();
    }

    /// <summary>
    /// 처음 전투 시작 전 현제 아군/적의 리스트를 가져옴
    /// </summary>
    private void InitEntityList()
    {
        // 우선 리스트 초기화
        entityList.Clear();


    }
}
