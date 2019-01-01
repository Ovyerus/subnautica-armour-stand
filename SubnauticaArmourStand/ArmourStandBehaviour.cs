using UnityEngine;

namespace SubnauticaArmourStand
{
    class ArmourStandBehaviour : HandTarget, IHandTarget, IProtoEventListener, IProtoTreeEventListener
    {
        public Equipment Armour { get; private set; }
        public ChildObjectIdentifier ArmourRoot = null;
        private Constructable WorldInstance = null;
        private static string[] SlotNames = new string[5] {
            "Head",
            "Body",
            "Gloves",
            "Foots",
            "Tank"
        };

        public override void Awake()
        {
            base.Awake();

            if (WorldInstance == null) WorldInstance = GetComponentInChildren<Constructable>();
            if (Armour == null) Init();
        }

        private void Init()
        {
            if (ArmourRoot == null)
            {
                var root = new GameObject("EquipmentRoot");
                root.transform.SetParent(transform, false);
                ArmourRoot = root.AddComponent<ChildObjectIdentifier>();
            }

            Armour = new Equipment(base.gameObject, ArmourRoot.transform);
            Armour.SetLabel("OpenStorage");

            Armour.onEquip += OnEquip;
            Armour.onUnequip += OnUnequip;

            SetSlots();
        }

        private void SetSlots()
        {
            Armour.AddSlots(SlotNames);
        }

        private void OnEquip(string slot, InventoryItem item) {
            // Don't allow deconsturction if full.
            WorldInstance.deconstructionAllowed = false;
        }

        private void OnUnequip(string slot, InventoryItem item)
        {
            bool empty = true;

            foreach (string slotName in SlotNames)
                empty &= Armour.GetTechTypeInSlot(slotName) == TechType.None;

            WorldInstance.deconstructionAllowed = empty;
        }

        public void OnHandHover(GUIHand hand)
        {
            if (!WorldInstance.constructed) return;

            HandReticle main = HandReticle.main;
            main.SetInteractText("UseArmourStand");
            main.SetIcon(HandReticle.IconType.Hand, 1f);
        }

        public void OnHandClick(GUIHand hand)
        {
            if (!WorldInstance.constructed) return;

            PDA pda = Player.main.GetPDA();
            Inventory.main.SetUsedStorage(Armour);
            pda.Open(PDATab.Inventory);
        }

        public void OnProtoSerialize(ProtobufSerializer serialiser) { }
        public void OnProtoDeserialize(ProtobufSerializer serialiser) { }

        public void OnProtoSerializeObjectTree(ProtobufSerializer serialiser) { }
        public void OnProtoDeserializeObjectTree(ProtobufSerializer serialiser) { }
    }
}
