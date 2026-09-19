using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
            if (Keyboard.current != null &&
                Keyboard.current.enterKey.wasPressedThisFrame &&
                BrakeMode == false)
            {
                if (rb != null)
                {
                    rb.AddForce(transform.forward * power);

                    BrakeMode = true;
                    braked = false;

                    launchTimer = 0f;
                    canBrake = false;

                    game.launchTime = Time.time;

                    // GameManagerをLaunch状態にする
                    game.state = GameManager.GameState.Launch;

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
                game.hinttext.text = "ボタンを押してブレーキ!";
            }


            // --------------------------------------
            // Spaceキーでブレーキ
            // --------------------------------------

            if (canBrake &&
                braked == false &&
                Keyboard.current != null &&
                Keyboard.current.enterKey.wasPressedThisFrame)
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