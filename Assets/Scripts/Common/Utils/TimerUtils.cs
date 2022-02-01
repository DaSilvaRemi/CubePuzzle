using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUtils : MonoBehaviour
{
    [SerializeField] private float m_timer;

    public float TimeLeft { get; private set; }

    public float TimePassed { get => Timer - TimeLeft; }

    public float Timer { get => m_timer;  }

    public int FormatedTimerLeft { get => GetFormatedTime(TimeLeft); }

    public int FormatedTimePassed { get => GetFormatedTime(TimePassed); }

    public int FormatedTimer { get => GetFormatedTime(Timer); }

    public bool IsFinish { get => TimeLeft <= 0; }

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
        TimeLeft = 0;
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

    public static int GetFormatedTime(float time)
    {
        return Mathf.RoundToInt(time);
    }

    private IEnumerator Countdown()
    {
        while (TimeLeft > 0f)
        {
            TimeLeft -= Time.deltaTime;
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
