using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUtils : MonoBehaviour
{
    [SerializeField] public float Timer { get; private set; }

    public bool IsFinish { get => Timer <= 0; }

    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

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
