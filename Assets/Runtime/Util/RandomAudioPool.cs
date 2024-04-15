using System.Collections.Generic;
using UnityEngine;

namespace SoldByWizards.Util
{
    public class RandomAudioPool : AudioPool
    {
        public List<AudioClip> Clips;
        private System.Random _random = new();

        public void PlayRandom()
        {
            if (Clips.Count == 0)
                return;
            var clip = Clips[_random.Next(0, Clips.Count)];
            Play(clip);
        }
    }
}
