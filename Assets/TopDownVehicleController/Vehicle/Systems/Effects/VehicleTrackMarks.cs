
using System.Collections.Generic;
using UnityEngine;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;
using TopDownVehicleController.Vehicle.Systems.Suspension;

namespace TopDownVehicleController.Vehicle.Systems.Effects
{
    /// <summary>
    /// Generates independent, continuous track marks using dynamic meshes.
    /// Ground contacts are provided by VehicleSuspension.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VehicleTrackMarks :
        MonoBehaviour, IVehicleSystem, IContextReceiver
    {
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

        private VehicleContext context;
        private TrackStrip leftStrip;
        private TrackStrip rightStrip;

        private sealed class TrackStrip
        {
            public readonly List<Vector3> Centers = new List<Vector3>();
            public readonly List<Vector3> Normals = new List<Vector3>();
            public readonly List<Vector3> Directions = new List<Vector3>();
            public readonly List<float> Distances = new List<float>();

            public Mesh Mesh;
            public MeshRenderer Renderer;
            public bool HasPreviousPoint;
            public Vector3 PreviousPoint;
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

            if (suspension == null ||
                frontLeft == null || backLeft == null ||
                frontRight == null || backRight == null ||
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

            UpdateStrip(leftStrip, frontLeft, backLeft);
            UpdateStrip(rightStrip, frontRight, backRight);
        }

        public void LateTick() { }

        private TrackStrip CreateStrip(string objectName)
        {
            GameObject stripObject = new GameObject(objectName);

            // Keep the mesh in world space, independent of vehicle motion.
            stripObject.transform.position = Vector3.zero;

            MeshFilter filter = stripObject.AddComponent<MeshFilter>();
            MeshRenderer renderer = stripObject.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.name = objectName;

            filter.sharedMesh = mesh;
            renderer.sharedMaterial = trackMarkMaterial;

            return new TrackStrip
            {
                Mesh = mesh,
                Renderer = renderer
            };
        }

        private void UpdateStrip(
            TrackStrip strip,
            Transform frontPoint,
            Transform backPoint)
        {
            bool frontGrounded = suspension.TryGetContact(
                frontPoint, out SuspensionContactData frontContact);

            bool backGrounded = suspension.TryGetContact(
                backPoint, out SuspensionContactData backContact);

            if (!frontGrounded || !backGrounded)
            {
                strip.HasPreviousPoint = false;
                return;
            }

            Vector3 center = (frontContact.Point + backContact.Point) * 0.5f;

            Vector3 normal = (
                frontContact.Normal + backContact.Normal).normalized;

            if (normal.sqrMagnitude < 0.001f)
                normal = Vector3.up;

            Vector3 forward = Vector3.ProjectOnPlane(
                frontContact.Point - backContact.Point, normal).normalized;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.ProjectOnPlane(
                    context.Transform.forward, normal).normalized;

            center += normal * groundOffset;

            if (!strip.HasPreviousPoint)
            {
                strip.PreviousPoint = center;
                strip.HasPreviousPoint = true;
                AddPoint(strip, center, normal, forward);
                return;
            }

            float distance = Vector3.Distance(strip.PreviousPoint, center);

            if (distance < segmentDistance)
                return;

            strip.TotalDistance += distance;
            strip.PreviousPoint = center;

            AddPoint(strip, center, normal, forward);
            RebuildMesh(strip);
        }

        private void AddPoint(
            TrackStrip strip,
            Vector3 center,
            Vector3 normal,
            Vector3 forward)
        {
            strip.Centers.Add(center);
            strip.Normals.Add(normal);
            strip.Directions.Add(forward);
            strip.Distances.Add(strip.TotalDistance);

            int maxPoints = maxSegments + 1;

            while (strip.Centers.Count > maxPoints)
            {
                strip.Centers.RemoveAt(0);
                strip.Normals.RemoveAt(0);
                strip.Directions.RemoveAt(0);
                strip.Distances.RemoveAt(0);
            }
        }

        private void RebuildMesh(TrackStrip strip)
        {
            int count = strip.Centers.Count;

            if (count < 2)
                return;

            Vector3[] vertices = new Vector3[count * 2];
            Vector2[] uvs = new Vector2[count * 2];
            int[] triangles = new int[(count - 1) * 6];

            for (int i = 0; i < count; i++)
            {
                Vector3 sideways = Vector3.Cross(
                    strip.Normals[i], strip.Directions[i]).normalized;

                Vector3 offset = sideways * trackWidth * 0.5f;

                vertices[i * 2] = strip.Centers[i] - offset;
                vertices[i * 2 + 1] = strip.Centers[i] + offset;

                float v = strip.Distances[i] / textureRepeatDistance;

                uvs[i * 2] = new Vector2(0f, v);
                uvs[i * 2 + 1] = new Vector2(1f, v);
            }

            for (int i = 0; i < count - 1; i++)
            {
                int vertexIndex = i * 2;
                int triangleIndex = i * 6;

                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 2;
                triangles[triangleIndex + 2] = vertexIndex + 1;

                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + 2;
                triangles[triangleIndex + 5] = vertexIndex + 3;
            }

            strip.Mesh.Clear();
            strip.Mesh.vertices = vertices;
            strip.Mesh.uv = uvs;
            strip.Mesh.triangles = triangles;
            strip.Mesh.RecalculateNormals();
            strip.Mesh.RecalculateBounds();
        }

        private void OnDisable()
        {
            if (leftStrip != null)
                leftStrip.HasPreviousPoint = false;

            if (rightStrip != null)
                rightStrip.HasPreviousPoint = false;
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

            if (strip.Renderer != null)
                Destroy(strip.Renderer.gameObject);

            if (strip.Mesh != null)
                Destroy(strip.Mesh);
        }
    }
}
