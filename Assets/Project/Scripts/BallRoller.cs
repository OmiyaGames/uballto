using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class BallRoller : MonoBehaviour
{
    [SerializeField]
    float stillTorque = 10f;
    [SerializeField]
    Vector2 stillForce;

    [Header("Music")]
    [SerializeField]
    AmbientMusic music;
    [SerializeField]
    Vector2 pitchRange = new Vector2(0.6f, 2f);
    [SerializeField]
    float velocityToPitchMultiplier = 0.25f;
    [SerializeField]
    float offsetPitch = 0.1f;

    [Header("Sound")]
    [SerializeField]
    SoundEffect defaultBoundsSound;

    Rigidbody2D body = null;

    public Rigidbody2D Body
    {
        get => OmiyaGames.Utility.GetComponentCached(this, ref body);
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        defaultBoundsSound.Play();
    }
}
