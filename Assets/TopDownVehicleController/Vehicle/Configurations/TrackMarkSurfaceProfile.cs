using System;
using System.Collections.Generic;
using UnityEngine;

using TopDownVehicleController.Vehicle.Data;

namespace TopDownVehicleController.Vehicle.Configurations
{
    [CreateAssetMenu(
        fileName = "TrackMarkSurfaceProfile",
        menuName = "TopDownVehicleController/Track Mark Surface Profile")]
    public sealed class TrackMarkSurfaceProfile : ScriptableObject
    {
        [Serializable]
        public struct SurfaceMarkSettings
        {
            public SurfaceType SurfaceType;
            public bool EnableMarks;
            public Color Color;
        }

        [SerializeField]
        private List<SurfaceMarkSettings> surfaces =
            new List<SurfaceMarkSettings>();

        public bool TryGetSettings(
            SurfaceType surfaceType,
            out SurfaceMarkSettings settings)
        {
            foreach (SurfaceMarkSettings entry in surfaces)
            {
                if (entry.SurfaceType != surfaceType)
                    continue;

                settings = entry;
                return true;
            }

            settings = default;
            return false;
        }
    }
}