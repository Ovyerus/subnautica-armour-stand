using System.Collections.Generic;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;

namespace SubnauticaArmourStand
{
    class ArmourStand : Buildable
    {
        public override string AssetsFolder => Main.AssetFolder;
        public override TechGroup GroupForPDA => TechGroup.InteriorModules;
        public override TechCategory CategoryForPDA => TechCategory.InteriorModule;
        public override string HandOverText => "Access armour stand";
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

        public override GameObject GetGameObject() {
            GameObject armourStand = Object.Instantiate(Main.ArmourStandModel);
            Constructable armourStandConstructable = armourStand.AddComponent<Constructable>();

            armourStandConstructable.allowedOnWall = false;
            armourStandConstructable.allowedOnGround = true;
            armourStandConstructable.allowedInSub = true;
            armourStandConstructable.allowedOutside = true;
            armourStandConstructable.techType = this.TechType;
            armourStandConstructable.model = armourStand.transform.GetChild(0).gameObject;

            return armourStand;
        }
    }
}
