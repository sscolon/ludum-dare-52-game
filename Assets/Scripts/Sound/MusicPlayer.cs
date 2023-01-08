using UnityEngine;

namespace Mechanizer
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _musicClip;
        private void Start()
        {
            SoundSettings settings = SoundSettings.MainMusic;
            settings.clip = _musicClip;
            SoundManager.PlayMainMusic(_musicClip);
        }
    }
}
