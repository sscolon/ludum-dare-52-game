using UnityEngine;

namespace Mechanizer
{
    public struct SoundSettings
    {
        public AudioClip clip;
        public Transform transform;
        public AudioType type;
        public bool isLooping;
        public bool isContinuous;
        public bool overrideMainTheme;
        public bool useVolumeLoss;
        public bool usePitchVariation;

        public static SoundSettings DefaultSound = new SoundSettings
        {
            type = AudioType.Sound,
            isLooping = false,
            isContinuous = false,
            overrideMainTheme = false,
            useVolumeLoss = true,
            usePitchVariation = true,
        };

        public static SoundSettings TrueSound = new SoundSettings
        {
            type = AudioType.Sound,
            isLooping = false,
            isContinuous = false,
            overrideMainTheme = false,
            useVolumeLoss = false,
            usePitchVariation = true,
        };

        public static SoundSettings MainMusic = new SoundSettings
        {
            type = AudioType.Music,
            isLooping = true,
            isContinuous = true,
            overrideMainTheme = true,
            useVolumeLoss = false,
            usePitchVariation = false,
        };

        public static SoundSettings AmbientSound = new SoundSettings
        {
            type = AudioType.Ambient,
            isLooping = true,
            isContinuous = false,
            overrideMainTheme = false,
            useVolumeLoss = false,
            usePitchVariation = false,
        };

        public static SoundSettings CreateDefaultSound(AudioClip clip)
        {
            SoundSettings settings = DefaultSound;
            settings.clip = clip;
            return settings;
        }
        public static SoundSettings CreateTrueSound(AudioClip clip)
        {
            SoundSettings settings = TrueSound;
            settings.clip = clip;
            return settings;
        }

        public static SoundSettings CreateMainMusic(AudioClip clip)
        {
            SoundSettings settings = MainMusic;
            settings.clip = clip;
            return settings;
        }

        public static SoundSettings CreateAmbientSound(AudioClip clip)
        {
            SoundSettings settings = AmbientSound;
            settings.clip = clip;
            return settings;
        }
    }
}
