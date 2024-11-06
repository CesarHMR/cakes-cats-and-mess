using UnityEngine;

namespace audio
{
    [CreateAssetMenu(fileName = "New Sound", menuName = "Sound")]
    public class Sound : ScriptableObject
    {
        public string Id { get {  return id; } }
        public AudioClip AudioClip { get { return audioClip; } }
        public float Volume { get { return volume; } }
        public float Pitch { get { return pitch; } }
        public bool Loop { get { return loop; } }
        public int SourceAmounts { get { return sourceAmounts; } }

        [SerializeField] string id;
        [SerializeField] AudioClip audioClip;
        [SerializeField] [Range(0, 2)] float volume = 1f;
        [SerializeField] [Range(0, 2)] float pitch = 1f;
        [SerializeField] bool loop = false;
        [SerializeField] int sourceAmounts = 1;
    }
}