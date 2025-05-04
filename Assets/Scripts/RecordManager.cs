using UnityEngine;
using TMPro;

public class RecordManager : MonoBehaviour
{
    public GameObject recordPanel;             // 戦績パネル
    public TextMeshProUGUI recordText;         // 表示用テキスト

    private int winCount = 0;
    private int loseCount = 0;
    private int guuCount = 0;
    private int chokiCount = 0;
    private int paaCount = 0;

    // 戦績画面を開く
    public void OnClickShowRecord()
    {
        recordPanel.SetActive(true);
        recordText.gameObject.SetActive(true);
        UpdateRecordText();
    }

    // 閉じる
    public void OnClickCloseRecord()
    {
        recordPanel.SetActive(false);
        recordText.gameObject.SetActive(false);
    }

    // 勝ちを記録
    public void AddWin()
    {
        winCount++;
    }

    // 負けを記録
    public void AddLose()
    {
        loseCount++;
    }

    // 手を記録（0:グー、1:チョキ、2:パー）
    public void AddHand(int hand)
    {
        switch (hand)
        {
            case 0: guuCount++; break;
            case 1: chokiCount++; break;
            case 2: paaCount++; break;
        }
    }

    // 戦績のテキストを更新
    void UpdateRecordText()
    {
        int totalGames = winCount + loseCount;
        float winRate = totalGames > 0 ? (winCount * 100f / totalGames) : 0f;

        int totalHands = guuCount + chokiCount + paaCount;
        float guuRate = totalHands > 0 ? (guuCount * 100f / totalHands) : 0f;
        float chokiRate = totalHands > 0 ? (chokiCount * 100f / totalHands) : 0f;
        float paaRate = totalHands > 0 ? (paaCount * 100f / totalHands) : 0f;

        recordText.text =
            $"【戦績】\n" +
            $"勝ち数: {winCount}\n" +
            $"負け数: {loseCount}\n" +
            $"勝率: {winRate:F1}%\n" +
            $"グー: {guuCount}回（{guuRate:F1}%）\n" +
            $"チョキ: {chokiCount}回（{chokiRate:F1}%）\n" +
            $"パー: {paaCount}回（{paaRate:F1}%）";
    }
}
