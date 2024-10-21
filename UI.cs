using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private ChessBoard board;
    [SerializeField] private GameObject objectMate1;
    [SerializeField] private GameObject objectMate2;
    [SerializeField] private GameObject checkmateScreen;

    [Header("White Info")]
    [SerializeField] private GameObject whiteNameG;
    [SerializeField] private GameObject whiteTurnG;
    [SerializeField] private GameObject whiteScoreG;

    [Header("Black Info")]
    [SerializeField] private GameObject blackNameG;
    [SerializeField] private GameObject blackTurnG;
    [SerializeField] private GameObject blackScoreG;

    // Variables
    private TMP_Text whiteName;
    private TMP_Text whiteTurn;
    private TMP_Text whiteScore;
    private TMP_Text blackName;
    private TMP_Text blackTurn;
    private TMP_Text blackScore;
    private TMP_Text mate1;
    private TMP_Text mate2;

    private int res;
    private int newRes;

    // Setup
    private void Awake()
    {
        res = 0;
        newRes = 0;

        whiteName = whiteNameG.GetComponent<TMP_Text>();
        whiteTurn = whiteTurnG.GetComponent<TMP_Text>();
        whiteScore = whiteScoreG.GetComponent<TMP_Text>();
        blackName = blackNameG.GetComponent<TMP_Text>();
        blackTurn = blackTurnG.GetComponent<TMP_Text>();
        blackScore = blackScoreG.GetComponent<TMP_Text>();

        mate1 = objectMate1.GetComponent<TMP_Text>();
        mate2 = objectMate2.GetComponent<TMP_Text>();


        string pref1 = PlayerPrefs.GetString("WhiteName");
        string pref2 = PlayerPrefs.GetString("BlackName");

        if ( pref1 != null)
        {
            whiteName.text = pref1;
            whiteTurn.text = whiteName.text;
        }
        else
        {
            whiteName.text = "";
            whiteTurn.text = whiteName.text;
        }
        
        if(pref2 != null)
        {
            blackName.text = pref2;
            blackTurn.text = blackName.text;
        }
        else
        {
            blackName.text = "";
            blackTurn.text = blackName.text;
        }
    }

    private void Update()
    {
        int[] value = board.calculateValue();
        newRes = value[0] - value[1];

        if (newRes != res)
        {
            res = newRes;
            updateScore(res);
        }
    }

    // Useful functions
    private void updateScore(int res)
    {
        if(res < 0)
        {
            whiteScore.text = "";
            blackScore.text = "+ " + (res * -1).ToString();
        }

        else if (res > 0)
        {
            blackScore.text = "";
            whiteScore.text = "+ " + res.ToString();
        }

        else if (res == 0)
        {
            whiteScore.text = "";
            blackScore.text = "";
        }
    }

    public void switchTurns()
    {
        if(whiteNameG.activeSelf)
        {
            whiteNameG.SetActive(false);
            blackTurnG.SetActive(false);

            blackNameG.SetActive(true);
            whiteTurnG.SetActive(true);
        }
        else
        {
            whiteNameG.SetActive(true);
            blackTurnG.SetActive(true);

            blackNameG.SetActive(false);
            whiteTurnG.SetActive(false);
        }
    }

    public void checkMate(int team)
    {
        if(team == 0)
        {
            mate1.text = "Checkmate by " + whiteName.text;
            mate2.text = "Victory for white";
        }
        else
        {
            mate1.text = "Checkmate by " + blackName.text;
            mate2.text = "Victory for black";
        }

        checkmateScreen.SetActive(true);
    }
}
