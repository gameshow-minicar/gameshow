using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float launchTime = -1f;
    public Transform player;
    public StageGenerator stage;
    public CarMove car;

    public TMP_Text aimtext;
    public TMP_Text nowtext;
    public TMP_Text counttext;
    public TMP_Text chargetext;
    public TMP_Text hinttext;
    public TMP_Text ntext;
    public TMP_Text resultscoretext;

    public TMP_Text hyoukatext;

    public GameObject startPanel;

    public GameObject gamePanel;
    public GameObject resultPanel;

    public float aimDist;
    public float nowDist;

    public int scoredist;

    private bool initialized = false;
    private bool resultShown = false;

    public enum GameState //状態
{
    Start,
    Charge,
    Wait,
    Ready,
    Launch,
    Result
}

public AudioSource bgm;
public AudioSource se;

public AudioClip gameBGM;
public AudioClip goodBGM;
public AudioClip greatBGM;
public AudioClip perfectBGM;
public AudioClip badBGM;


public AudioClip startSE;
public AudioClip goSE;
public AudioClip launchSE;

public AudioClip c1SE;
public AudioClip c2SE;
public GameState state;

    void Start()
    {
        resultPanel.SetActive(false);
        gamePanel.SetActive(false);

        StartCoroutine(GameFlow());
    }

    void Update()
    {
        if (state == GameState.Result &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("titlescene");
        }
        if (state == GameState.Ready &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            state = GameState.Launch;
            hinttext.text = "spaceキーでブレーキ";
            se.PlayOneShot(launchSE);
            counttext.text = "";
            launchTime = Time.time;
        }
        // ゴール距離は一度だけ取得
        if (!initialized)
        {
            aimDist = stage.lastRoadPos.x - player.position.x;
            aimtext.text = "目標: " + (aimDist / 10f).ToString("0") + " m";
            initialized = true;
        }

        // 現在距離
        nowDist = stage.lastRoadPos.x - player.position.x;
        nowtext.text = "残り: " + (nowDist / 10f).ToString("0") + " m";

        // リザルト表示
        if (!resultShown &&
            car.BrakeMode &&
            Time.time - launchTime > 1f &&
            car.GetComponent<Rigidbody>().linearVelocity.magnitude < 0.01f)
        {
            resultShown = true;
            scoredist = (int)(nowDist / 10f);
            if(scoredist < 0)
            {
                scoredist = 0;
            }
            if(car.killed == false){
                resultscoretext.text = (scoredist).ToString("0") + " m";
                if(scoredist == 0)
                {
                    bgm.clip = perfectBGM;
                    hyoukatext.text = "ピッタリ!!!!!!!";
                }
                else if(scoredist > 0 && scoredist <= 25)
                {
                    bgm.clip = greatBGM;
                    hyoukatext.text = "ギリギリ!"; 
                }
                else if(scoredist > 25 && scoredist <= 100)
                {
                    bgm.clip = goodBGM;
                    hyoukatext.text = "いい感じだね"; 
                }
                else
                {
                    bgm.clip = badBGM;
                    hyoukatext.text = "遠いね"; 
                }
            }
            else if(car.killed == true)
            {
                ntext.text = "";
                bgm.clip = badBGM;
                resultscoretext.text = "激突";
                hyoukatext.text = "あちゃー"; 
            }
            
            StartCoroutine(ShowResult());
        }
    }

    IEnumerator GameFlow()
    {
        // START
        state = GameState.Start;
        se.PlayOneShot(startSE);
        startPanel.SetActive(true);
        yield return new WaitForSeconds(2f);

        // CHARGE
        bgm.clip = gameBGM;
        bgm.loop = true;
        bgm.Play();
        state = GameState.Charge;

        startPanel.SetActive(false);
        gamePanel.SetActive(true);

        chargetext.text = "パワーを貯めろ！";
        hinttext.text = "Pキー連打でチャージ";
        for (int i = 10; i > 0; i--)
        {
            counttext.text = i.ToString();
            if(i <= 5)
            {
                se.PlayOneShot(c2SE);
            }
            else
            {
                se.PlayOneShot(c1SE);
            }
            yield return new WaitForSeconds(1f);
        }

        chargetext.text = "";
        counttext.text = "";
        state = GameState.Wait;
        hinttext.text = "";
        yield return new WaitForSeconds(2f);
    // READY
        state = GameState.Ready;
        hinttext.text = "spaceキーで発車";
        se.PlayOneShot(goSE);
        counttext.text = "GO!";
    }

    IEnumerator ShowResult()
    {
        gamePanel.SetActive(false);
        bgm.Stop();
        yield return new WaitForSeconds(3f);
        bgm.loop = true;
        bgm.Play();
        resultPanel.SetActive(true);
        state = GameState.Result;
    }
}