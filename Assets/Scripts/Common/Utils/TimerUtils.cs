using System;
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

    private float m_timeLeft;
    private IEnumerator m_MyCountdownCoroutineRef;

    #region TimerUtils properties
    /**
     * <summary>The time left to the counter</summary> 
     */
    public float TimeLeft { get => Tools.GetRoundedFloat(this.TimeLeft, 2); private set { if (value >= 0) this.m_timeLeft = value; } }

    /**
     * <summary>The time passed</summary> 
     */
    public float TimePassed { get => this.Timer - this.TimeLeft; }

    /**
     * <summary>The timer definied in the component</summary>
     */
    public float Timer { get => this.m_timer; }

    /**
     * <summary>A int format for the time left</summary>
     */
    public string FormatedTimerLeft { get => Tools.FormatFloatNumberToString(this.TimeLeft); }

    /**
     * <summary>A int format for the time passed</summary>
     */
    public string FormatedTimePassed { get => Tools.FormatFloatNumberToString(this.TimePassed); }

    /**
     * <summary>A int format for the timer</summary>
     */
    public string FormatedTimer { get => Tools.FormatFloatNumberToString(Timer); }

    /**
     * <summary>If the timer is finish</summary>
     * <remarks>The timer is finish only if the TimeLeft <= 0 </remarks>
     */
    public bool IsFinish { get => Tools.GetRoundedFloat(this.TimeLeft, 1) <= 0.0f; }
    #endregion

    #region TimerUtils methods
    /**
     * <summary>Start the timer</summary>
     */
    public void StartTimer()
    {
        this.ResetTimer();
        this.ContinueTimer();
    }


    /// <summary>
    /// Continue a stopped timer <see cref="StopTimer"/> and <see cref="MyCountdownCoroutine"/>
    /// </summary>
    public void ContinueTimer()
    {
        this.StopTimer();
        this.m_MyCountdownCoroutineRef = this.MyCountdownCoroutine();
        this.StartCoroutine(this.m_MyCountdownCoroutineRef);
    }

    /**
     * <summary>Reset the timer</summary> 
     */
    public void ResetTimer()
    {
        this.TimeLeft = this.Timer;
    }

    /**
     * <summary>Stop the timer</summary> 
     */
    public void StopTimer()
    {
        if (this.m_MyCountdownCoroutineRef != null)
        {
            this.StopCoroutine(this.m_MyCountdownCoroutineRef);
            this.m_MyCountdownCoroutineRef = null;
        }
    }

    /// <summary>
    /// Late the current timer
    /// </summary>
    /// <param name="addTime">Add time to current timer</param>
    public void LateTime(float addTime)
    {
        this.TimeLeft += addTime;
    }

    /**
     * <summary>Countdown the timer</summary>
     * <returns>The IEnumerator</returns>
     */
    private IEnumerator MyCountdownCoroutine()
    {
        while (this.TimeLeft > 0f)
        {
            this.TimeLeft -= Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region MonoBehaviour methods
    private void Start()
    {
        this.StartTimer();
    }

    private void OnDestroy()
    {
        this.StopTimer();
    }
    #endregion
}
