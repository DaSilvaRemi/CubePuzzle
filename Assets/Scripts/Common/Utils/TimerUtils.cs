using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *<summary>Util Timer manage countdown timer</summary> 
 */
public class TimerUtils : MonoBehaviour
{
    [Header("Timer")]
    [Tooltip("float")]
    [SerializeField] private float m_timer;

    /**
     * <summary>The time left to the counter</summary> 
     */
    public float TimeLeft { get; private set; }

    /**
     * <summary>The time passed</summary> 
     */
    public float TimePassed { get => Timer - TimeLeft; }

    /**
     * <summary>The timer definied in the component</summary>
     */
    public float Timer { get => m_timer; }

    /**
     * <summary>A int format for the time left</summary>
     */
    public int FormatedTimerLeft { get => Tools.FormatFloatToInt(TimeLeft); }

    /**
     * <summary>A int format for the time passed</summary>
     */
    public int FormatedTimePassed { get => Tools.FormatFloatToInt(TimePassed); }

    /**
     * <summary>A int format for the timer</summary>
     */
    public int FormatedTimer { get => Tools.FormatFloatToInt(Timer); }

    /**
     * <summary>If the timer is finish</summary>
     * <remarks>The timer is finish only if the TimeLeft <= 0 </remarks>
     */
    public bool IsFinish { get => TimeLeft <= 0; }

    /**
     * <summary>Start the timer</summary>
     */
    public void StartTimer()
    {
        TimeLeft = Timer;
        StartCoroutine(Countdown());
    }

    /**
     * <summary>Reset the timer</summary> 
     */
    public void ResetTimer()
    {
        TimeLeft = 0;
    }

    /**
     * <summary>Stop the timer</summary> 
     */
    public void StopTimer()
    {
        StopCoroutine(Countdown());
    }

    /**
     * <summary>Wait a time</summary> 
     * <param name="time">The time</param>
     * <returns>The IEnumerator</returns>
     */
    public static IEnumerator Wait(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
        }
    }

    /**
     * <summary>Countdown the timer</summary>
     * <returns>The IEnumerator</returns>
     */
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
