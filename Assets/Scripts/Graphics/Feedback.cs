using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mechanizer
{
    [Serializable]
    public class Feedback
    {
        [Serializable]
        public class SpawnObject
        {
            public string name;
            public bool inheritSpawnRotation;
            public GameObject prefab;
            public Transform spawn;
            public void Invoke(GameObject ctx)
            {
                if (prefab == null || spawn == null)
                {
                    return;
                }

                GameObject instance = GameObject.Instantiate(prefab, spawn.position, prefab.transform.rotation);
                if (inheritSpawnRotation)
                    instance.transform.rotation = spawn.transform.rotation;
            }
        }

        [Header("Sounds")]
        public bool useRandomSound = false;
        public List<AudioClip> sounds;

        [Header("Particles")]
        public bool useRandomSpawn = false;
        public List<SpawnObject> spawns;

        [Header("Screenshake")]
        public bool useScreenshake = false;
        public float screenshakeStrength = 10f;
        public float screenshakeDuration = 0.2f;
        private void DoSound(GameObject ctx)
        {
            if (sounds == null || sounds.Count == 0)
                return;

            if (useRandomSound)
            {
                int randSoundIndex = UnityEngine.Random.Range(0, sounds.Count);
                AudioClip sound = sounds[randSoundIndex];
                SoundSettings settings = SoundSettings.CreateDefaultSound(sound);
                settings.useVolumeLoss = true;
                settings.transform = ctx.transform;
                SoundManager.PlaySound(settings);
            }
            else
            {
                for (int i = 0; i < sounds.Count; i++)
                {
                    AudioClip sound = sounds[i];
                    SoundSettings settings = SoundSettings.CreateDefaultSound(sound);
                    settings.useVolumeLoss = true;
                    settings.transform = ctx.transform;
                    SoundManager.PlaySound(settings);
                }
            }
        }

        private void DoParticles(GameObject ctx)
        {
            if (spawns == null || spawns.Count == 0)
                return;
            if (useRandomSpawn)
            {
                int randParticleIndex = UnityEngine.Random.Range(0, spawns.Count);
                var spawn = spawns[randParticleIndex];
                spawn.Invoke(ctx);
            }
            else
            {
                for (int i = 0; i < spawns.Count; i++)
                {
                    var spawn = spawns[i];
                    spawn.Invoke(ctx);
                }
            }
        }

        private void DoScreenshake(GameObject ctx)
        {
            if (!useScreenshake)
                return;
            Helpers.MainCameraFollow.Shake(screenshakeStrength, screenshakeDuration);
        }

        public void CreateFeedback(GameObject ctx)
        {
            DoSound(ctx);
            DoParticles(ctx);
            DoScreenshake(ctx);
        }
    }
}
