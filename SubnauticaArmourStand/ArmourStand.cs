using System.Collections.Generic;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using UnityEngine;

namespace SubnauticaArmourStand
{
    class ArmourStand : Buildable
    {
        public override string AssetsFolder => Main.AssetFolder;
        public override TechGroup GroupForPDA => TechGroup.InteriorModules;
        public override TechCategory CategoryForPDA => TechCategory.InteriorModule;
        public override string HandOverText => "UseArmourStand";
        public override string IconFileName => "ArmourStand.png";

        public ArmourStand() : base("ArmourStand", "Armour Stand", "Show off and store your various suits.") { }

        protected override TechData GetBlueprintRecipe()
        {
            List<Ingredient> ingredients = new List<Ingredient>()
            {
                new Ingredient(TechType.Titanium, 2),
                new Ingredient(TechType.Glass, 1)
            };
            TechData recipe = new TechData()
            {
                craftAmount = 1,
                Ingredients = ingredients
            };


            return recipe;
        }

        public new void Patch() {
            LanguageHandler.SetLanguageLine(HandOverText, "Access Armour Stand");

            base.Patch();
        }

        public override GameObject GetGameObject() {
            //GameObject armourStand = Object.Instantiate(Main.ArmourStandModel);
            GameObject armourStand = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.LabTrashcan)); // Temp model
            Constructable armourStandConstructable = armourStand.AddComponent<Constructable>();

            // Remove trashcan and storage we dont need. Just the model kthx.
            GameObject.DestroyImmediate(armourStand.GetComponent<Trashcan>());
            GameObject.DestroyImmediate(armourStand.GetComponent<StorageContainer>());

            armourStand.AddComponent<ArmourStandBehaviour>();

            // Placement rules.
            armourStandConstructable.allowedOnConstructables = false;
            armourStandConstructable.allowedOnCeiling = false;
            armourStandConstructable.allowedOnWall = false;
            armourStandConstructable.allowedOnGround = true;

            armourStandConstructable.allowedInSub = true;
            armourStandConstructable.allowedInBase = true;
            armourStandConstructable.allowedOutside = false;

            armourStandConstructable.rotationEnabled = true;

            armourStandConstructable.techType = TechType;
            armourStandConstructable.model = armourStand.transform.GetChild(0).gameObject;

            return armourStand;
        }
    }
}
