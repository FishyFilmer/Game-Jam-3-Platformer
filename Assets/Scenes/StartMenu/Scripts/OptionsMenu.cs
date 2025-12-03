using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public void SetMaster (float masterVolume){
        audioMixer.SetFloat("MasterVolume", masterVolume);

    }

    public void SetEffects (float effectsVolume){
        audioMixer.SetFloat("EffectVolume", effectsVolume);
    }

    public void SetMusic (float musicVolume){
        audioMixer.SetFloat("MusicVolume", musicVolume);
    }
}
