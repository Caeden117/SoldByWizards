using System.Collections.Generic;
using UnityEngine;

namespace SoldByWizards.Util
{
    public class RandomAudioPool : AudioPool
    {
        public List<AudioClip> Clips;

        public void PlayRandom()
        {
            if (Clips.Count == 0)
                return;

            var clip = Clips[Random.Range(0, Clips.Count)];
            Play(clip);
        }
    }
}
