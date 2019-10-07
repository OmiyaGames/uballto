using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class Crate : ISoundMaker
{
    [Header("Sound")]
    [SerializeField]
    protected SoundEffect defaultBoundsSound;
    [SerializeField]
    protected SoundEffect waterSplashSound;

    Rigidbody2D body = null;

    public Rigidbody2D Body
    {
        get => OmiyaGames.Utility.GetComponentCached(this, ref body);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        defaultBoundsSound.Play();
    }

    public override void OnEnterWater(WaterTrigger source)
    {
        base.OnEnterWater(source);
        waterSplashSound.Play();
    }
}
