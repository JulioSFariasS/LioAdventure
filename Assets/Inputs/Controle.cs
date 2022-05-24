using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controle : MonoBehaviour
{
    public static bool upDown, upUp, up;
    public static bool downDown, downUp, down;
    public static bool leftDown, leftUp, left;
    public static bool rightDown, rightUp, right;

    public static bool jumpDown, jumpUp, jump;
    public static bool attackDown, attackUp, attack;

    static float low, high, time;
    static bool vibraStart;

    private void Update()
    {
        /* Any Key Down pro Gamepad
        if (Gamepad.current != null)
        {
            if (Gamepad.current.IsPressed())
            {

            }
        }
        */

        if (vibraStart)
        {
            Vibrar(low, high, time);
        }
    }

    private void LateUpdate()
    {
        upDown = false;
        upUp = false;
        downDown = false;
        downUp = false;
        leftDown = false;
        leftUp = false;
        rightDown = false;
        rightUp = false;
        jumpDown = false;
        jumpUp = false;
        attackDown = false;
        attackUp = false;
    }

    public static void SetVibra(float low, float high, float time)
    {
        Controle.low = low;
        Controle.high = high;
        Controle.time = time;
        vibraStart = true;
    }

    void Vibrar(float low, float high, float time)
    {
        vibraStart = false;
        if (Gamepad.current != null)
        {
            StopAllCoroutines();
            StartCoroutine(VibrarTime(low, high, time));
        }
    }

    IEnumerator VibrarTime(float low, float high, float time)
    {
        Gamepad.current.SetMotorSpeeds(low, high);
        yield return new WaitForSeconds(time);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    public void Up(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            upDown = true;
            up = true;
        }

        if (callback.canceled)
        {
            upUp = true;
            up = false;
        }
    }

    public void Down(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            downDown = true;
            down = true;
        }

        if (callback.canceled)
        {
            downUp = true;
            down = false;
        }
    }

    public void Left(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            leftDown = true;
            left = true;
        }

        if (callback.canceled)
        {
            leftUp = true;
            left = false;
        }
    }

    public void Right(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            rightDown = true;
            right = true;
        }

        if (callback.canceled)
        {
            rightUp = true;
            right = false;
        }
    }

    public void Jump(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            jumpDown = true;
            jump = true;
        }

        if (callback.canceled)
        {
            jumpUp = true;
            jump = false;
        }
    }

    public void Attack(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            attackDown = true;
            attack = true;
        }

        if (callback.canceled)
        {
            attackUp = true;
            attack = false;
        }
    }
}
