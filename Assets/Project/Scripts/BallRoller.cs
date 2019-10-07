using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class BallRoller : Crate
{
    [Header("Music")]
    [SerializeField]
    AmbientMusic music;
    [SerializeField]
    Vector2 pitchRange = new Vector2(0.6f, 2f);
    [SerializeField]
    float velocityToPitchMultiplier = 0.25f;
    [SerializeField]
    float offsetPitch = 0.1f;

    [Header("Movement")]
    [SerializeField]
    float stillTorque = 10f;
    [SerializeField]
    Vector2 stillForce;

    private void Start()
    {
        music.Audio.time = Random.Range(0f, music.Audio.clip.length);
    }

    void FixedUpdate()
    {
        if(Body.bodyType == RigidbodyType2D.Dynamic)
        {
            Body.AddTorque(stillTorque * Time.deltaTime, ForceMode2D.Force);
            Body.AddForce(stillForce * Time.deltaTime, ForceMode2D.Force);

            // Change the music pitch based on velocity
            if(music.Audio.isPlaying == false)
            {
                music.Play();
            }
            float pitch = Body.velocity.magnitude * velocityToPitchMultiplier;
            pitch += offsetPitch;
            music.Audio.pitch = Mathf.Clamp(pitch, pitchRange.x, pitchRange.y);
        }
        else if (music.Audio.isPlaying)
        {
            music.Pause();
        }
    }
}
