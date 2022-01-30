using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUtils : MonoBehaviour
{
    [SerializeField] float m_timer;


    public float Timer { 
        get
        {
            return m_timer;
        }
        private set
        {
            m_timer = value;
        }
    }

    public bool IsFinish { get => Timer <= 0; }

    /**
     * <summary>Start the timer</summary>
     */
    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    /**
     * <summary>Reset the timer</summary> 
     */
    public void ResetTimer()
    {
        Timer = 0;
    }

    public void StopTimer()
    {
        StopCoroutine(Countdown());
    }

    public static IEnumerator Wait(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Countdown()
    {
        while (Timer > 0f)
        {
            Timer -= Time.deltaTime;
            yield return null;
        }
    }

    private void Start()
    {
        StartTimer();
    }

    private void OnDestroy()
    {
        StopTimer();
    }
}
