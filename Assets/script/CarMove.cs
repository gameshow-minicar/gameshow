using UnityEngine;
using UnityEngine.InputSystem;
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
    //Image型の変数_imageを宣言しておく
    public Image bar;

    public TMPro.TMP_Text powertext;

    public bool killed = false;

    public AudioSource se;

    public AudioClip chargeSE;

    public AudioClip brakeSE;
    public AudioClip crashSE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();      
        if (rb == null)
        {
            Debug.Log("Rigidbodyが見つかりません");
        }  
        rb.linearDamping = deceleration;   // 数値を大きくすると減速が強くなる
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame && game.state == GameManager.GameState.Charge)
        {
            if(rb != null && BrakeMode == false)
            {
                se.PlayOneShot(chargeSE);
                power += 1000;
            }
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame  && game.state == GameManager.GameState.Launch)
        {
            if(rb != null && BrakeMode == false)
            {
                rb.AddForce(transform.forward * power);
                BrakeMode = true;
            }
            else if(rb != null && BrakeMode == true && braked == false)
            {
                se.PlayOneShot(brakeSE);
                braked = true;
                rb.linearDamping = deceleration * 50;
            }
        }
        bar.fillAmount = power / Maxbar;
        powertext.text = (power / 1000f).ToString("0") + "%";
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("kill"))
        {
            se.PlayOneShot(crashSE);
            killed = true;
        }
    }
}
