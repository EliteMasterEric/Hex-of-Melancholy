using R2API;
using RoR2;
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
        }

        private static void HexMelancholyAsHexItem()
        {
            hexMelancholy = ScriptableObject.CreateInstance<ItemDef>();

            SetItemTierToHex(hexMelancholy);

            SetLocalizationKey(hexMelancholy, "ELITEMASTERERIC_MELANCHOLY");

            hexMelancholy.canRemove = true;
            hexMelancholy.hidden = false;

            hexMelancholy.pickupIconSprite = Assets.hexMelancholyIcon;
            hexMelancholy.pickupModelPrefab = Assets.hexMelancholyPrefab;

            ItemAPI.Add(new CustomItem(hexMelancholy, BuildPlaceholderDisplayRules()));
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

        private static ItemDisplayRuleDict BuildPlaceholderDisplayRules()
        {
            var displayRules = new ItemDisplayRuleDict(null);
            return displayRules;
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