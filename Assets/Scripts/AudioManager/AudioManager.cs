using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;
    public Sound[] steps;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Initialization();
    }

    public void Play(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.Play();
                break;
            }
        }
    }

    public void Stop(string name) 
    {
        foreach(Sound sound in sounds)
        {
            if(sound.name == name)
            {
                sound.source.Stop();
                break;
            }
        }
    }
    
    private void Initialization()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.name = s.name;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        StepInitilization();
    }

    private void StepInitilization()
    {
        foreach(Sound s in steps)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.name = s.name;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlayStep()
    {
        Sound step = GetRandomStep();
        step.source.Play();
    }

    private Sound GetRandomStep()
    {
        return steps[Random.Range(0,steps.Length)];
    }
}
