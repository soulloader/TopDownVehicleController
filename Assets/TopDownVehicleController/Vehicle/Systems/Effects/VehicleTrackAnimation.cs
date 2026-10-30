using UnityEngine;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;

namespace TopDownVehicleController.Vehicle.Systems.Effects
{
    /// <summary>
    /// Animates left and right track textures independently using UV scrolling.
    /// Uses actual forward velocity and yaw angular velocity without modifying physics.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VehicleTrackAnimation : MonoBehaviour, IVehicleSystem, IContextReceiver
    {
        [Header("Track Renderers")]
        [SerializeField] private Renderer leftTrackRenderer;
        [SerializeField] private Renderer rightTrackRenderer;

        [Header("UV Scrolling")]
        [SerializeField, Min(0f)] private float scrollSpeedPerMeter = 0.15f;
        [SerializeField, Min(0f)] private float turnScrollFactor = 0.25f;
        [SerializeField] private bool invertLeftDirection;
        [SerializeField] private bool invertRightDirection;

        private static readonly int BaseMapSt = Shader.PropertyToID("_BaseMap_ST");
        private VehicleContext context;
        private MaterialPropertyBlock leftBlock;
        private MaterialPropertyBlock rightBlock;
        private Vector4 leftBaseMapSt;
        private Vector4 rightBaseMapSt;
        private float leftOffset;
        private float rightOffset;
        private bool initialized;

        public void SetContext(VehicleContext vehicleContext)
        {
            context = vehicleContext;
        }

        public void Initialize()
        {
            initialized = false;
            if (context == null || context.Rigidbody == null ||
                leftTrackRenderer == null || rightTrackRenderer == null ||
                leftTrackRenderer == rightTrackRenderer)
            {
                Debug.LogError("[TopDownVehicleController] VehicleTrackAnimation requires a context and two distinct track Renderers.", this);
                enabled = false;
                return;
            }

            leftBlock = new MaterialPropertyBlock();
            rightBlock = new MaterialPropertyBlock();
            leftBaseMapSt = GetBaseMapSt(leftTrackRenderer);
            rightBaseMapSt = GetBaseMapSt(rightTrackRenderer);
            leftOffset = leftBaseMapSt.w;
            rightOffset = rightBaseMapSt.w;
            initialized = true;
        }

        public void Update() { }

        public void FixedTick()
        {
            if (!initialized)
                return;

            Rigidbody rb = context.Rigidbody;
            float forwardSpeed = Vector3.Dot(rb.velocity, context.Transform.forward);
            float turnSpeed = Vector3.Dot(rb.angularVelocity, context.Transform.up);

            float leftSpeed = forwardSpeed - turnSpeed * turnScrollFactor;
            float rightSpeed = forwardSpeed + turnSpeed * turnScrollFactor;

            float delta = Time.fixedDeltaTime * scrollSpeedPerMeter;
            leftOffset = Mathf.Repeat(leftOffset + leftSpeed * delta * (invertLeftDirection ? -1f : 1f), 1f);
            rightOffset = Mathf.Repeat(rightOffset + rightSpeed * delta * (invertRightDirection ? -1f : 1f), 1f);

            ApplyOffset(leftTrackRenderer, leftBlock, leftBaseMapSt, leftOffset);
            ApplyOffset(rightTrackRenderer, rightBlock, rightBaseMapSt, rightOffset);
        }

        public void LateTick() { }

        private static Vector4 GetBaseMapSt(Renderer renderer)
        {
            Material material = renderer.sharedMaterial;
            if (material == null || !material.HasProperty("_BaseMap"))
                return new Vector4(1f, 1f, 0f, 0f);

            Vector2 scale = material.GetTextureScale("_BaseMap");
            Vector2 offset = material.GetTextureOffset("_BaseMap");
            return new Vector4(scale.x, scale.y, offset.x, offset.y);
        }

        private static void ApplyOffset(Renderer renderer, MaterialPropertyBlock block, Vector4 originalSt, float offsetY)
        {
            renderer.GetPropertyBlock(block);
            block.SetVector(BaseMapSt, new Vector4(originalSt.x, originalSt.y, originalSt.z, offsetY));
            renderer.SetPropertyBlock(block);
        }

        private void OnDisable()
        {
            // Restore the original UV transform without altering shared materials.
            if (leftTrackRenderer != null && leftBlock != null)
                ApplyOffset(leftTrackRenderer, leftBlock, leftBaseMapSt, leftBaseMapSt.w);
            if (rightTrackRenderer != null && rightBlock != null)
                ApplyOffset(rightTrackRenderer, rightBlock, rightBaseMapSt, rightBaseMapSt.w);
            initialized = false;
        }
    }
}
