using UnityEngine;

using TopDownVehicleController.Vehicle.Data;

namespace TopDownVehicleController.Vehicle.Configurations
{
    /// <summary>
    /// Defines the audio configuration associated with a surface type.
    /// </summary>
    [System.Serializable]
    public class SurfaceAudioConfig
    {
        [SerializeField]
        private SurfaceType surfaceType;

        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        [Range(0f, 1f)]
        private float volume = 1f;

        [SerializeField]
        [Min(0f)]
        private float pitch = 1f;

        public SurfaceType SurfaceType => surfaceType;

        public AudioClip AudioClip => audioClip;

        public float Volume => volume;

        public float Pitch => pitch;
    }
}