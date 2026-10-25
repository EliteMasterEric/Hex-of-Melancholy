using RoR2;
using RoR2.Items;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace HexOfMelancholy
{
    class MelancholyBehavior : BaseItemBodyBehavior
    {
        public static PostProcessProfile profile;
        public static ColorGrading grading;

        private GameObject volumeObject;

        [BaseItemBodyBehavior.ItemDefAssociationAttribute(useOnServer = false, useOnClient = true)]
        public static ItemDef GetItemDef()
        {
            // This behavior gets auto-attached if the given item is equipped.
            return Items.hexMelancholy;
        }

        private void OnEnable()
        {
            bool isCurrentPlayer = body.hasEffectiveAuthority && body.isPlayerControlled;
            if (!isCurrentPlayer) return;

            if (profile == null) BuildProfile();

            volumeObject = new GameObject("MelancholyPostProcess")
            {
                layer = LayerIndex.postProcess.intVal
            };
            volumeObject.SetActive(false);

            var volumeComponent = volumeObject.AddComponent<PostProcessVolume>();
            volumeComponent.isGlobal = true;
            volumeComponent.priority = 100f;
            volumeComponent.weight = 1.0f;
            volumeComponent.sharedProfile = profile;

            volumeObject.SetActive(true);
        }

        public override void OnInventoryRefresh()
        {
            if (volumeObject == null) return;
            
            Plugin.Instance.PluginLogger.LogInfo($"Hex of Melancholy equipped ({stack} stacks)");

            // Intensity based on stack count.
            grading.postExposure.Override(0.0f + (-0.15f * (stack - 1)));
            grading.contrast.Override(10.0f + (20.0f * (stack - 1)));

            // Grading gets cached, so we have to re-calculate it.
            PostProcessVolume.DispatchVolumeSettingsChangedEvent();
        }

        private void OnDisable()
        {
            Plugin.Instance.PluginLogger.LogInfo("Hex of Melancholy unequipped!");

            if (volumeObject != null)
            {
                Destroy(volumeObject);
            }
        }

        private static void BuildProfile()
        {
            MelancholyBehavior.profile = ScriptableObject.CreateInstance<PostProcessProfile>();
            MelancholyBehavior.grading = profile.AddSettings<ColorGrading>();

            grading.tonemapper.Override(Tonemapper.Custom);
            grading.toneCurveGamma.Override(2.0f);

            grading.postExposure.Override(0.0f);
            grading.colorFilter.Override(new Color(0.0f, 0.2f, 1.0f));
            grading.saturation.Override(100.0f);
            grading.contrast.Override(10.0f);
        }
    }
}