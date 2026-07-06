using UnityEngine;
using UnityEngine.InputSystem;

public class CarMove : MonoBehaviour
{
    public float power = 100000f;
    public float deceleration = 1f;

    public bool BrakeMode = false;
    public bool braked = false;
    private Rigidbody rb;
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
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if(rb != null && BrakeMode == false)
            {
                rb.AddForce(transform.forward * power);
                BrakeMode = true;
            }
            else if(rb != null && BrakeMode == true && braked == false)
            {
                braked = true;
                rb.linearDamping = deceleration * 50;
            }
        }
    }
}
