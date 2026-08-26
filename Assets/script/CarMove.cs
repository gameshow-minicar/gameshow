using UnityEngine;
using UnityEngine.UI;

public class CarMove : MonoBehaviour
{
    public float power = 100000f;
    public float deceleration = 1f;

    public bool BrakeMode = false;
    public bool braked = false;

    public GameManager game;

    private Rigidbody rb;

    private float Maxbar = 100000.0f;

    public Image bar;
    public TMPro.TMP_Text powertext;

    public bool killed = false;

    public AudioSource se;

    public AudioClip chargeSE;
    public AudioClip brakeSE;
    public AudioClip crashSE;

    // changeの発動条件
    private const double changeThreshold = 5.0;

    // 発射後の経過時間
    private float launchTimer = 0f;

    // ブレーキ可能か
    private bool canBrake = false;

    // 発射後、一度changeが5未満になったか
    private bool changeReset = false;

    // SEの間隔
    private float seTimer = 0f;

    private const float seDelay = 0.5f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("Rigidbodyが見つかりません");
            return;
        }

        rb.linearDamping = deceleration;
    }


    void Update()
    {
        // ==========================================
        // SEタイマー
        // ==========================================

        if (seTimer > 0)
        {
            seTimer -= Time.deltaTime;
        }


        // ==========================================
        // CHARGE
        // ==========================================

        if (game.state == GameManager.GameState.Charge)
        {
            if (ChangeManager.change >= changeThreshold)
            {
                if (rb != null && BrakeMode == false)
                {
                    power += (float)ChangeManager.change * 8.5f;

                    PlaySE(chargeSE);
                }
            }
        }


        // ==========================================
        // READY
        // change >= 5 で発射
        // ==========================================

        if (game.state == GameManager.GameState.Ready)
        {
            if (BrakeMode == false &&
                ChangeManager.change >= changeThreshold)
            {
                if (rb != null)
                {
                    rb.AddForce(transform.forward * power);

                    BrakeMode = true;
                    braked = false;

                    launchTimer = 0f;
                    canBrake = false;
                    changeReset = false;

                    game.launchTime = Time.time;

                    // GameManagerをLaunch状態にする
                    game.state = GameManager.GameState.Launch;

                    // 発射時刻を記録
                    // GameManagerのリザルト判定にも必要
                    // GameManager側のlaunchTimeがprivateなので、
                    // ここでは別途処理する必要があります。

                    PlaySE(game.launchSE);

                    Debug.Log("発射！");
                }
            }
        }


        // ==========================================
        // LAUNCH
        // ==========================================

        if (game.state == GameManager.GameState.Launch)
        {
            // 発射後の時間
            game.hinttext.text = " ";
            game.counttext.text = " ";
            launchTimer += Time.deltaTime;


            // --------------------------------------
            // 発射から1秒経過
            // --------------------------------------

            if (launchTimer >= 1.0f)
            {
                canBrake = true;
                game.hinttext.text = "ハンドルを後ろに倒してブレーキ";
            }


            // --------------------------------------
            // changeが5未満になった
            // --------------------------------------

            if (canBrake &&
                ChangeManager.change < changeThreshold)
            {
                changeReset = true;
            }


            // --------------------------------------
            // 再びchangeが5以上になった
            // → ブレーキ
            // --------------------------------------

            if (canBrake &&
                changeReset &&
                braked == false &&
                ChangeManager.change >= changeThreshold)
            {
                if (rb != null)
                {
                    rb.linearDamping = deceleration * 50f;

                    braked = true;

                    PlaySE(brakeSE);

                    Debug.Log("ブレーキ！");
                }
            }
        }


        // ==========================================
        // UI
        // ==========================================

        bar.fillAmount = power / Maxbar;

        powertext.text =
            (power / 1000f).ToString("0") + "%";
    }


    // ==========================================
    // SE
    // ==========================================

    void PlaySE(AudioClip clip)
    {
        if (clip == null || se == null)
        {
            return;
        }

        if (seTimer > 0)
        {
            return;
        }

        se.PlayOneShot(clip);

        seTimer = seDelay;
    }


    // ==========================================
    // 衝突
    // ==========================================

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("kill"))
        {
            se.PlayOneShot(crashSE);

            killed = true;
        }
    }
}