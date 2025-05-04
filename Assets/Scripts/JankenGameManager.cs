using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class JankenGameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject againPanel;

    [Header("UI Texts")]
    public TextMeshProUGUI readyText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI resultText;

    [Header("Images")]
    public Image playerHandImage;
    public Image cpuHandImage;
    public Sprite[] handSprites;
    public Sprite timeoutSprite;

    [Header("Buttons")]
    public GameObject handButtons;

    [Header("Timeout UI")]
    public GameObject timeoutDisplay;

    [Header("Record")]
    public RecordManager recordManager;  // 追加：戦績記録用

    private bool hasSelected = false;
    private int playerChoice = -1;

    public void OnClickStart()
    {
        titlePanel.SetActive(false);
        againPanel.SetActive(false);
        timeoutDisplay.SetActive(false);
        resultText.text = "";
        playerHandImage.sprite = null;
        playerHandImage.gameObject.SetActive(false);
        cpuHandImage.sprite = null;
        cpuHandImage.gameObject.SetActive(false);
        handButtons.SetActive(true);
        StartCoroutine(GameSequence());
        resultText.gameObject.SetActive(false);
        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
    }

    IEnumerator GameSequence()
    {
        handButtons.SetActive(false);

        readyText.gameObject.SetActive(true);
        readyText.text = "Ready...";
        yield return new WaitForSeconds(0.3f);
        readyText.gameObject.SetActive(false);

        handButtons.SetActive(true);

        hasSelected = false;
        playerChoice = -1;

        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = $"残り時間：{i}";
            yield return new WaitForSeconds(1f);

            if (hasSelected)
                break;
        }

        countdownText.text = "";
        countdownText.gameObject.SetActive(false);

        if (!hasSelected)
        {
            playerHandImage.gameObject.SetActive(true);
            playerHandImage.sprite = timeoutSprite;
            timeoutDisplay.SetActive(true);
            handButtons.SetActive(false);
            playerChoice = -1;
            Judge(playerChoice);
        }
        else
        {
            timeoutDisplay.SetActive(false);
        }
    }

    public void OnClickHand(int choice)
    {
        if (hasSelected) return;

        hasSelected = true;
        handButtons.SetActive(false);
        playerChoice = choice;

        // 🔸 プレイヤーの手を記録
        recordManager.AddHand(choice);

        StartCoroutine(DelayedShowHandAndJudge());
    }

    IEnumerator DelayedShowHandAndJudge()
    {
        yield return new WaitForSeconds(0.7f);
        playerHandImage.gameObject.SetActive(true);
        playerHandImage.sprite = handSprites[playerChoice];
        Judge(playerChoice);
    }

    void Judge(int playerHand)
    {
        int cpuHand = Random.Range(0, 3);
        cpuHandImage.gameObject.SetActive(true);
        cpuHandImage.sprite = handSprites[cpuHand];

        string[] hands = { "グー", "チョキ", "パー" };
        string result;

        if (playerHand == -1)
        {
            result = "時間切れ！\nあなたの負け！";

            // 🔸 負けを記録
            recordManager.AddLose();

            ShowAgainPanel();
        }
        else if (playerHand == cpuHand)
        {
            result = $"あいこ！\n（あなた：{hands[playerHand]}、CPU：{hands[cpuHand]}）";
            resultText.gameObject.SetActive(true);
            resultText.text = result;
            StartCoroutine(WaitAndRestart());
            return;
        }
        else if ((playerHand == 0 && cpuHand == 1) || (playerHand == 1 && cpuHand == 2) || (playerHand == 2 && cpuHand == 0))
        {
            result = $"あなたの勝ち！\n（あなた：{hands[playerHand]}、CPU：{hands[cpuHand]}）";

            // 🔸 勝ちを記録
            recordManager.AddWin();

            ShowAgainPanel();
        }
        else
        {
            result = $"コンピューターの勝ち！\n（あなた：{hands[playerHand]}、CPU：{hands[cpuHand]}）";

            // 🔸 負けを記録
            recordManager.AddLose();

            ShowAgainPanel();
        }

        resultText.gameObject.SetActive(true);
        resultText.text = result;
        handButtons.SetActive(false);
    }

    void ShowAgainPanel()
    {
        againPanel.SetActive(true);
        resultText.gameObject.SetActive(true);
    }

    public void OnClickAgain()
    {
        againPanel.SetActive(false);
        timeoutDisplay.SetActive(false);
        resultText.text = "";
        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
        playerHandImage.gameObject.SetActive(false);
        playerHandImage.sprite = null;
        cpuHandImage.gameObject.SetActive(false);
        cpuHandImage.sprite = null;
        StartCoroutine(GameSequence());
    }

    public void OnClickQuit()
    {
        againPanel.SetActive(false);
        timeoutDisplay.SetActive(false);
        resultText.text = "";
        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
        playerHandImage.gameObject.SetActive(false);
        playerHandImage.sprite = null;
        cpuHandImage.gameObject.SetActive(false);
        cpuHandImage.sprite = null;
        titlePanel.SetActive(true);
    }

    IEnumerator WaitAndRestart()
    {
        yield return new WaitForSeconds(2.0f);
        playerHandImage.gameObject.SetActive(false);
        cpuHandImage.gameObject.SetActive(false);
        playerHandImage.sprite = null;
        cpuHandImage.sprite = null;
        resultText.text = "";
        resultText.gameObject.SetActive(false);
        StartCoroutine(GameSequence());
    }
}
