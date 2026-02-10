using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [Header("턴을 표시할 텍스트")]
    public TMP_Text turnNumber;

    private int turn;   // 턴 수를 기록

    // 부를때마다 턴 수 증가
    public void SetTurnText()
    {
        turnNumber.text = (++turn).ToString();
    }
}
