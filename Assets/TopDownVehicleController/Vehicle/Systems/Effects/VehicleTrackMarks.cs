using UnityEngine;
using System.Collections.Generic;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;
using TopDownVehicleController.Vehicle.Systems.Suspension;
using TopDownVehicleController.Vehicle.Configurations;
using TopDownVehicleController.Vehicle.Systems.Detection;

namespace TopDownVehicleController.Vehicle.Systems.Effects
{
    /// <summary>
    /// Generates independent track marks with lifetime-based fading.
    /// Uses suspension contacts without modifying vehicle physics.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VehicleTrackMarks :
        MonoBehaviour, IVehicleSystem, IContextReceiver
    {
        [Header("Surface Track Marks")]
        [SerializeField] private VehicleSurfaceDetector surfaceDetector;
        [SerializeField] private TrackMarkSurfaceProfile surfaceProfile;

        [Header("Suspension")]
        [SerializeField] private VehicleSuspension suspension;

        [Header("Left Track")]
        [SerializeField] private Transform frontLeft;
        [SerializeField] private Transform backLeft;

        [Header("Right Track")]
        [SerializeField] private Transform frontRight;
        [SerializeField] private Transform backRight;

        [Header("Track Marks")]
        [SerializeField] private Material trackMarkMaterial;
        [SerializeField, Min(0.01f)] private float trackWidth = 0.5f;
        [SerializeField, Min(0.01f)] private float segmentDistance = 0.15f;
        [SerializeField, Min(0.001f)] private float groundOffset = 0.025f;
        [SerializeField, Min(0.01f)] private float textureRepeatDistance = 0.5f;
        [SerializeField, Min(2)] private int maxSegments = 200;

        [Header("Track Mark Lifetime")]
        [SerializeField, Min(0.1f)] private float markLifetime = 10f;
        [SerializeField, Min(0.01f)] private float fadeDuration = 3f;

        [Header("Continuity")]
        [SerializeField, Min(0.1f)] private float maxConnectionDistance = 2f;

        private VehicleContext context;
        private TrackStrip leftStrip;
        private TrackStrip rightStrip;

        private struct TrackSample
        {
            public Vector3 Center;
            public Vector3 Normal;
            public Vector3 Forward;
            public float Distance;

            public TrackSample(
                Vector3 center,
                Vector3 normal,
                Vector3 forward,
                float distance)
            {
                Center = center;
                Normal = normal;
                Forward = forward;
                Distance = distance;
            }
        }

        private struct TrackSegment
        {
            public TrackSample Start;
            public TrackSample End;
            public float CreatedAt;
            public Color32 Color;

            public TrackSegment(TrackSample start, TrackSample end,
            float createdAt, Color32 color)
            {
                Start = start;
                End = end;
                CreatedAt = createdAt;
                Color = color;
            }
        }

        private sealed class TrackStrip
        {
            public readonly List<TrackSegment> Segments =
                new List<TrackSegment>();

            public readonly List<Vector3> Vertices =
                new List<Vector3>();

            public readonly List<Vector2> UVs =
                new List<Vector2>();

            public readonly List<Color32> Colors =
                new List<Color32>();

            public readonly List<int> Triangles =
                new List<int>();

            public Mesh Mesh;
            public GameObject MeshObject;

            public bool HasPreviousSample;
            public TrackSample PreviousSample;
            public float TotalDistance;
        }

        public void SetContext(VehicleContext vehicleContext)
        {
            context = vehicleContext;
        }

        public void Initialize()
        {
            if (suspension == null)
                suspension = GetComponent<VehicleSuspension>();

            if (context == null ||
                suspension == null ||
                frontLeft == null ||
                backLeft == null ||
                frontRight == null ||
                backRight == null ||
                trackMarkMaterial == null)
            {
                Debug.LogError(
                    "[TopDownVehicleController] VehicleTrackMarks is missing references.",
                    this);

                enabled = false;
                return;
            }

            leftStrip = CreateStrip("LeftTrackMarks");
            rightStrip = CreateStrip("RightTrackMarks");
        }

        public void Update() { }

        public void FixedTick()
        {
            if (context == null || leftStrip == null || rightStrip == null)
                return;

            float currentTime = Time.time;

            UpdateStrip(leftStrip, frontLeft, backLeft, currentTime);
            UpdateStrip(rightStrip, frontRight, backRight, currentTime);

            UpdateLifetime(leftStrip, currentTime);
            UpdateLifetime(rightStrip, currentTime);

            RebuildMesh(leftStrip, currentTime);
            RebuildMesh(rightStrip, currentTime);
        }

        public void LateTick() { }

        private TrackStrip CreateStrip(string objectName)
        {
            GameObject stripObject = new GameObject(objectName);
            stripObject.transform.position = Vector3.zero;

            MeshFilter filter = stripObject.AddComponent<MeshFilter>();
            MeshRenderer renderer = stripObject.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.name = objectName;
            mesh.MarkDynamic();

            filter.sharedMesh = mesh;
            renderer.sharedMaterial = trackMarkMaterial;

            return new TrackStrip
            {
                Mesh = mesh,
                MeshObject = stripObject
            };
        }

        private void UpdateStrip(
            TrackStrip strip,
            Transform frontPoint,
            Transform backPoint,
            float currentTime)
        {
            bool frontGrounded = suspension.TryGetContact(
                frontPoint, out SuspensionContactData frontContact);

            bool backGrounded = suspension.TryGetContact(
                backPoint, out SuspensionContactData backContact);

            if (!frontGrounded || !backGrounded)
            {
                strip.HasPreviousSample = false;
                return;
            }

            Vector3 center =
                (frontContact.Point + backContact.Point) * 0.5f;

            Vector3 normal =
                frontContact.Normal + backContact.Normal;

            if (normal.sqrMagnitude < 0.001f)
                normal = Vector3.up;
            else
                normal.Normalize();

            Vector3 forward = Vector3.ProjectOnPlane(
                frontContact.Point - backContact.Point,
                normal);

            if (forward.sqrMagnitude < 0.001f)
            {
                forward = Vector3.ProjectOnPlane(
                    context.Transform.forward,
                    normal);
            }

            if (forward.sqrMagnitude < 0.001f)
                return;

            forward.Normalize();
            center += normal * groundOffset;

            if (!strip.HasPreviousSample)
            {
                strip.PreviousSample = new TrackSample(
                    center, normal, forward, strip.TotalDistance);

                strip.HasPreviousSample = true;
                return;
            }

            float distance = Vector3.Distance(
                strip.PreviousSample.Center, center);

            if (distance > maxConnectionDistance)
            {
                strip.HasPreviousSample = false;
                return;
            }

            if (distance < segmentDistance)
                return;

            strip.TotalDistance += distance;

            TrackSample currentSample = new TrackSample(
                center,
                normal,
                forward,
                strip.TotalDistance);

            Color32 markColor = new Color32(255, 255, 255, 255);

            if (surfaceDetector != null && surfaceDetector.CurrentSurface == null)
            {
                strip.HasPreviousSample = false;
                return;
            }

            if (surfaceDetector != null &&
                surfaceProfile != null &&
                surfaceDetector.CurrentSurface != null)
            {
                SurfaceType surfaceType =
                    surfaceDetector.CurrentSurface.SurfaceType;

                if (surfaceProfile.TryGetSettings(
                    surfaceType,
                    out TrackMarkSurfaceProfile.SurfaceMarkSettings settings))
                {
                    if (!settings.EnableMarks)
                    {
                        strip.HasPreviousSample = false;
                        return;
                    }

                    markColor = settings.Color;
                }
            }

            strip.Segments.Add(new TrackSegment(
                strip.PreviousSample,
                currentSample,
                currentTime,
                markColor));

            strip.PreviousSample = currentSample;

            while (strip.Segments.Count > maxSegments)
                strip.Segments.RemoveAt(0);
        }

        private void UpdateLifetime(
            TrackStrip strip,
            float currentTime)
        {
            float lifetime = Mathf.Max(0.1f, markLifetime);

            while (strip.Segments.Count > 0)
            {
                TrackSegment oldest = strip.Segments[0];

                if (currentTime - oldest.CreatedAt < lifetime)
                    break;

                strip.Segments.RemoveAt(0);
            }
        }

        private void RebuildMesh(
            TrackStrip strip,
            float currentTime)
        {
            strip.Vertices.Clear();
            strip.UVs.Clear();
            strip.Colors.Clear();
            strip.Triangles.Clear();

            float lifetime = Mathf.Max(0.1f, markLifetime);
            float fade = Mathf.Clamp(
                fadeDuration, 0.01f, lifetime);

            float fadeStart = lifetime - fade;
            int capacityFadeSegments = Mathf.Max(1, Mathf.CeilToInt(maxSegments * 0.15f));

            for (int i = 0; i < strip.Segments.Count; i++)
            {
                TrackSegment segment = strip.Segments[i];

                float age = currentTime - segment.CreatedAt;

                float alpha = age <= fadeStart
                    ? 1f
                    : Mathf.Clamp01((lifetime - age) / fade);

                // Fade the oldest marks before the segment budget evicts them.
                float capacityAlpha = Mathf.Clamp01(
                    (maxSegments - strip.Segments.Count + i) / (float)capacityFadeSegments);
                alpha = Mathf.Min(alpha, capacityAlpha);

                Color32 vertexColor = segment.Color;
                // Surface profiles supply RGB tint; lifetime owns vertex opacity.
                vertexColor.a = (byte)Mathf.RoundToInt(255f * alpha);

                AddSegmentGeometry(
                    strip, segment, vertexColor);
            }

            strip.Mesh.Clear();

            if (strip.Vertices.Count == 0)
                return;

            strip.Mesh.SetVertices(strip.Vertices);
            strip.Mesh.SetUVs(0, strip.UVs);
            strip.Mesh.SetColors(strip.Colors);
            strip.Mesh.SetTriangles(strip.Triangles, 0);
            strip.Mesh.RecalculateBounds();
        }

        private void AddSegmentGeometry(
            TrackStrip strip,
            TrackSegment segment,
            Color32 color)
        {
            int vertexIndex = strip.Vertices.Count;

            Vector3 startSide = Vector3.Cross(
                segment.Start.Normal,
                segment.Start.Forward).normalized;

            Vector3 endSide = Vector3.Cross(
                segment.End.Normal,
                segment.End.Forward).normalized;

            float halfWidth = trackWidth * 0.5f;

            Vector3 startOffset = startSide * halfWidth;
            Vector3 endOffset = endSide * halfWidth;

            strip.Vertices.Add(
                segment.Start.Center - startOffset);

            strip.Vertices.Add(
                segment.Start.Center + startOffset);

            strip.Vertices.Add(
                segment.End.Center - endOffset);

            strip.Vertices.Add(
                segment.End.Center + endOffset);

            float startV =
                segment.Start.Distance / textureRepeatDistance;

            float endV =
                segment.End.Distance / textureRepeatDistance;

            strip.UVs.Add(new Vector2(0f, startV));
            strip.UVs.Add(new Vector2(1f, startV));
            strip.UVs.Add(new Vector2(0f, endV));
            strip.UVs.Add(new Vector2(1f, endV));

            for (int i = 0; i < 4; i++)
                strip.Colors.Add(color);

            strip.Triangles.Add(vertexIndex);
            strip.Triangles.Add(vertexIndex + 2);
            strip.Triangles.Add(vertexIndex + 1);

            strip.Triangles.Add(vertexIndex + 1);
            strip.Triangles.Add(vertexIndex + 2);
            strip.Triangles.Add(vertexIndex + 3);
        }

        private void OnDisable()
        {
            if (leftStrip != null)
                leftStrip.HasPreviousSample = false;

            if (rightStrip != null)
                rightStrip.HasPreviousSample = false;
        }

        private void OnDestroy()
        {
            DestroyStrip(leftStrip);
            DestroyStrip(rightStrip);
        }

        private static void DestroyStrip(TrackStrip strip)
        {
            if (strip == null)
                return;

            if (strip.MeshObject != null)
                Destroy(strip.MeshObject);

            if (strip.Mesh != null)
                Destroy(strip.Mesh);
        }
    }
}
