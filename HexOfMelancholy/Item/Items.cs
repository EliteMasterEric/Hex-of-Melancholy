using HG;
using R2API;
using RoR2;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HexOfMelancholy
{
    public class Items
    {
        public static ItemDef hexMelancholy;
        
        public static void Initialize()
        {
            HexMelancholyAsHexItem();

            RoR2Application.onLoad += OnPostAppLoad;
        }

        private static void OnPostAppLoad()
        {
            // LogPlacementRules();
        }

        private static void HexMelancholyAsHexItem()
        {
            hexMelancholy = ScriptableObject.CreateInstance<ItemDef>();

            SetItemTierToHex(hexMelancholy);

            SetLocalizationKey(hexMelancholy, "ELITEMASTERERIC_MELANCHOLY");

            hexMelancholy.canRemove = true;
            hexMelancholy.hidden = false;

            hexMelancholy.pickupIconSprite = Assets.hexMelancholyIcon;
            hexMelancholy.pickupModelPrefab = Assets.hexMelancholyPickup;

            ItemAPI.Add(new CustomItem(hexMelancholy, BuildHexDisplayRules(Assets.hexMelancholyDisplay)));

            ApplyItemDisplay(Assets.hexMelancholyDisplay);
            ApplyLogbookDisplay(hexMelancholy);
        }

        private static void SetItemTierToHex(ItemDef itemDef)
        {
            SetItemTierByPath(itemDef, "RoR2/DLC4/HexDef.asset");
        }

        private static void SetItemTierToHallowed(ItemDef itemDef)
        {
            SetItemTierByPath(itemDef, "RoR2/DLC4/CursedTierDef.asset");
        }

        private static void SetItemTierToPage(ItemDef itemDef)
        {
            SetItemTierByPath(itemDef, "RoR2/DLC4/PageTierDef.asset");
        }

        private static void SetItemTierByPath(ItemDef itemDef, string path)
        {
            #pragma warning disable Publicizer001
            itemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>(path).WaitForCompletion();
            #pragma warning restore Publicizer001
        }

        private static void UsePlaceholderModel(ItemDef itemDef)
        {
            itemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            itemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
        }

        private static void LogPlacementRules()
        {
            // Debug function, lets you steal a specific item's rules.

            var template = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC4/HexCurse/HexCurse.asset").WaitForCompletion();

            // For each character, copy its item display rule set.
            // This is my first RoR2 mod I couldn't figure out how else to do this if you know the correct way PLEASE tell me.
            foreach (var bodyPrefab in BodyCatalog.allBodyPrefabs)
            {
                var characterModel = bodyPrefab.GetComponentInChildren<CharacterModel>();
                if (!characterModel || !characterModel.itemDisplayRuleSet) continue;

                var ruleSet = characterModel.itemDisplayRuleSet.GetItemDisplayRuleGroup(template.itemIndex);
                if (ruleSet.isEmpty) continue;

                foreach (var r in ruleSet.rules)
                {
                    Log.Info($"rules.Add(\"{characterModel.name}\", Rule(displayPrefab, \"{r.childName}\", " +
                        $"new Vector3({r.localPos.x}f, {r.localPos.y}f, {r.localPos.z}f), " +
                        $"new Vector3({r.localAngles.x}f, {r.localAngles.y}f, {r.localAngles.z}f), " +
                        $"new Vector3({r.localScale.x}f, {r.localScale.y}f, {r.localScale.z}f)));");
                }
            }
        }

        private static void ApplyItemDisplay(GameObject displayPrefab)
        {
            var itemDisplay = displayPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = displayPrefab.GetComponentsInChildren<Renderer>()
                .Select(renderer => new CharacterModel.RendererInfo
                {
                    renderer = renderer,
                    defaultMaterial = renderer.sharedMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                })
                .ToArray();
        }

        private static void ApplyLogbookDisplay(ItemDef itemDef)
        {
            var prefab = itemDef.pickupModelPrefab;
            if (prefab)
            {
                var mdlParams = prefab.EnsureComponent<ModelPanelParameters>();

                if (!mdlParams.focusPointTransform)
                {
                    mdlParams.focusPointTransform = new GameObject("FocusPoint").transform;
                    mdlParams.focusPointTransform.SetParent(itemDef.pickupModelPrefab.transform);
                }
                if (!mdlParams.cameraPositionTransform)
                {
                    mdlParams.cameraPositionTransform = new GameObject("CameraPosition").transform;
                    mdlParams.cameraPositionTransform.SetParent(itemDef.pickupModelPrefab.transform);
                }
            }
        }

        private static ItemDisplayRuleDict BuildPlaceholderDisplayRules()
        {
            var displayRules = new ItemDisplayRuleDict(null);
            return displayRules;
        }

        private static ItemDisplayRuleDict BuildHexDisplayRules(GameObject displayPrefab)
        {
            // DEFAULT
            var displayRules = new ItemDisplayRuleDict(BuildRule(displayPrefab, "Chest", new Vector3(0f, 0.2f, 0.2f), Vector3.zero, Vector3.one * 0.2f));

            // CHARACTERS *
            displayRules.Add("mdlAcolyteUnion", BuildRule(displayPrefab, "Chest", new Vector3(-0.0796f, 0.146f, 0.1377f), new Vector3(20.27105f, 180f, 0f), new Vector3(0.16915f, 0.16915f, 0.16915f)));
            displayRules.Add("mdlBandit2", BuildRule(displayPrefab, "Chest", new Vector3(-0.1162f, 0.2323f, 0.1467f), new Vector3(-1.067752E-05f, 150f, -6.861771E-06f), new Vector3(0.1123359f, 0.1123359f, 0.1123359f)));
            displayRules.Add("mdlBrother", BuildRule(displayPrefab, "LowerArmL", new Vector3(0.01846562f, 0.328752f, 0.0252654f), new Vector3(348.6916f, 38.38824f, 341.5876f), new Vector3(0.1f, 0.1f, 0.1f)));
            displayRules.Add("mdlBrotherLogbook", BuildRule(displayPrefab, "LowerArmL", new Vector3(0.01846562f, 0.328752f, 0.0252654f), new Vector3(348.6916f, 38.38824f, 341.5876f), new Vector3(0.1f, 0.1f, 0.1f)));
            displayRules.Add("mdlCaptain", BuildRule(displayPrefab, "Chest", new Vector3(-0.108f, 0.1822f, 0.1866f), new Vector3(-1.510187E-05f, 148.5006f, 9.359778E-06f), new Vector3(0.1341257f, 0.1341257f, 0.1341258f)));
            displayRules.Add("mdlChef", BuildRule(displayPrefab, "OvenDoor", new Vector3(-0.4751f, 0.1282f, 0.0431f), new Vector3(-3.415093E-06f, 180f, 3.415095E-06f), new Vector3(0.2045f, 0.2045002f, 0.2045001f)));
            displayRules.Add("mdlCommandoDualies", BuildRule(displayPrefab, "Chest", new Vector3(-0.1008f, 0.267f, 0.2021f), new Vector3(2.63528f, 165.5602f, 359.8929f), new Vector3(0.1f, 0.1f, 0.1f)));
            displayRules.Add("mdlCroco", BuildRule(displayPrefab, "Chest", new Vector3(0.907f, -0.53f, -2.178f), new Vector3(5.887945f, 334.4505f, 135.641f), new Vector3(1f, 0.9999994f, 1f)));
            displayRules.Add("mdlDrifter", BuildRule(displayPrefab, "Chest", new Vector3(0f, 0.201f, 0.201f), new Vector3(59.08591f, 148.8019f, 152.5459f), new Vector3(0.15147f, 0.15147f, 0.15147f)));
            displayRules.Add("mdlDroneTech", BuildRule(displayPrefab, "Chest", new Vector3(-0.0113f, -0.0939f, -0.1067f), new Vector3(294.7293f, 344.9807f, 15.31535f), new Vector3(0.1332682f, 0.1332682f, 0.1332682f)));
            displayRules.Add("mdlEnforcer", BuildRule(displayPrefab, "Chest", new Vector3(-0.1298f, 0.0359f, 0.2206f), new Vector3(4.434507E-13f, 152.9228f, 1.025943E-13f), new Vector3(0.1f, 0.1f, 0.09999996f)));
            displayRules.Add("mdlEngi", BuildRule(displayPrefab, "Chest", new Vector3(-0.2004f, 0.1233f, 0.1867f), new Vector3(335.1944f, 148.3727f, 10.98021f), new Vector3(0.2751999f, 0.2751999f, 0.2752002f)));
            displayRules.Add("mdlEngiTurret", BuildRule(displayPrefab, "LegBar1", new Vector3(0f, 0.115f, 0.14f), new Vector3(4.656899f, 180f, 180f), new Vector3(0.4343099f, 0.4343099f, 0.4343099f)));
            displayRules.Add("mdlEngiWalkerTurret", BuildRule(displayPrefab, "Head", new Vector3(-0.688f, 0.273f, 0.222f), new Vector3(350.5228f, 137.4514f, 1.298382E-06f), new Vector3(1f, 1f, 1f)));
            displayRules.Add("mdlFalseSon", BuildRule(displayPrefab, "Chest", new Vector3(-0.3109f, 0.1481f, 0.2457f), new Vector3(-8.67894E-13f, 150f, -1.503236E-12f), new Vector3(0.34593f, 0.34593f, 0.34593f)));
            displayRules.Add("mdlHuntress", BuildRule(displayPrefab, "Chest", new Vector3(-0.1439f, 0.1299f, 0.1233f), new Vector3(23.60964f, 119.0085f, -7.454135E-06f), new Vector3(0.1380912f, 0.1380912f, 0.1380912f)));
            displayRules.Add("mdlLoader", BuildRule(displayPrefab, "Chest", new Vector3(-0.1006f, 0.1782f, 0.1945f), new Vector3(6.403304E-07f, 180f, -4.28554E-07f), new Vector3(0.25754f, 0.2575398f, 0.2575397f)));
            displayRules.Add("mdlMage", BuildRule(displayPrefab, "Chest", new Vector3(-0.0737f, 0.1016f, 0.1336f), new Vector3(-1.419108E-05f, 157.0628f, 7.343848E-06f), new Vector3(0.13377f, 0.1337701f, 0.13377f)));
            displayRules.Add("mdlMerc", BuildRule(displayPrefab, "Chest", new Vector3(-0.084f, 0.1045f, 0.1878f), new Vector3(2.311238E-05f, 158.2706f, -1.008681E-05f), new Vector3(0.1373105f, 0.1373105f, 0.1373105f)));
            displayRules.Add("mdlRailGunner", BuildRule(displayPrefab, "Chest", new Vector3(-0.0712f, 0.0545f, 0.1393f), new Vector3(-4.268868E-07f, 180f, -1.838448E-07f), new Vector3(0.09841302f, 0.098413f, 0.09841298f)));
            displayRules.Add("mdlScav", BuildRule(displayPrefab, "Chest", new Vector3(4.026f, 1.2f, -8.191f), new Vector3(12.17085f, 329.157f, 1.310107E-06f), new Vector3(2.2375f, 2.237627f, 2.237519f)));
            displayRules.Add("mdlSeeker", BuildRule(displayPrefab, "Chest", new Vector3(-0.0897f, 0.0818f, 0.091f), new Vector3(4.698512f, 163.9156f, -1.606223E-07f), new Vector3(0.1397001f, 0.1397001f, 0.1397001f)));
            displayRules.Add("mdlSquidTurret", BuildRule(displayPrefab, "LegBar1", new Vector3(0f, 0.115f, 0.14f), new Vector3(4.656899f, 180f, 180f), new Vector3(0.4343099f, 0.4343099f, 0.4343099f)));
            displayRules.Add("mdlToolbot", BuildRule(displayPrefab, "Chest", new Vector3(-1.872f, 1.384f, 3.4f), new Vector3(-8.132429E-06f, 180f, -4.600729E-07f), new Vector3(1.442002f, 1.442f, 1.442f)));
            displayRules.Add("mdlTreebot", BuildRule(displayPrefab, "PlatformBase", new Vector3(-0.2956f, -0.0484f, 0.522f), new Vector3(0f, 145.7062f, 0f), new Vector3(0.23986f, 0.2398602f, 0.2398601f)));
            displayRules.Add("mdlVoidSurvivor", BuildRule(displayPrefab, "Chest", new Vector3(-0.1246f, 0.0421f, 0.1705f), new Vector3(347.4507f, 156.0442f, 4.154682E-06f), new Vector3(0.1867999f, 0.1867999f, 0.1867999f)));


            return displayRules;
        }

        private static ItemDisplayRule BuildRule(GameObject followerPrefab, string childName, Vector3 localPos, Vector3 localAngles, Vector3 localScale)
        {
            return new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = followerPrefab,
                childName = childName,
                localPos = localPos,
                localAngles = localAngles,
                localScale = localScale,
            };
        }

        private static void SetLocalizationKey(ItemDef itemDef, string baseKey)
        {
            itemDef.name = $"{baseKey}_NAME";
            itemDef.nameToken = $"{baseKey}_NAME";
            itemDef.pickupToken = $"{baseKey}_PICKUP";
            itemDef.descriptionToken = $"{baseKey}_DESC";
            itemDef.loreToken = $"{baseKey}_LORE";
        }
    }
}