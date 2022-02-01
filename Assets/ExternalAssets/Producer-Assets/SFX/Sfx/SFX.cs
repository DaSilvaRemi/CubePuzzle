using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource Grass;
    public AudioSource Wind;

    public void PlayGrass()
    {
        Grass.Play();
    }

    public void PlayWind()
    {
        Wind.Play();
    }

}
