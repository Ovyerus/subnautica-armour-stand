using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

namespace SubnauticaArmourStand
{
    [ProtoContract]
    class ArmourStandBehaviour : HandTarget, IHandTarget, IProtoEventListener, IProtoTreeEventListener
    {
        public Equipment Armour { get; private set; }
        public ChildObjectIdentifier ArmourRoot;
        private Constructable WorldInstance;
        private DataSaver SaveData;
        private string ID;
        private static readonly string[] SlotNames = new string[5] {
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

            if (SaveData == null)
            {
                string id = GetComponentInParent<PrefabIdentifier>().Id;
                SaveData = new DataSaver(id);
                ID = id;
            }

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
            // Don't allow deconstruction if has items.
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

        // On save
        public void OnProtoSerialize(ProtobufSerializer _)
        {
            Dictionary<string, int> saveDict = new Dictionary<string, int>();

            foreach (string slot in SlotNames)
            {
                InventoryItem item = Armour.GetItemInSlot(slot);

                if (item == null) saveDict.Add(slot, (int)TechType.None);
                else saveDict.Add(slot, (int)item.item.GetTechType());
            }

            SaveData.Save(saveDict);
        }

        public void OnProtoDeserialize(ProtobufSerializer _)
        {
            if (Armour == null) Init();

            Armour.Clear();
        }

        public void OnProtoSerializeObjectTree(ProtobufSerializer _) { }
  
        // On load
        public void OnProtoDeserializeObjectTree(ProtobufSerializer _)
        {
            if (SaveData.Load())
            {
                foreach (string slot in SlotNames)
                {
                    Armour.AddSlot(slot);

                    TechType slotItem = (TechType)SaveData.Data.GetOrDefault(slot, (int)TechType.None);

                    if (slotItem == TechType.None) continue;

                    //Pickupable itemPickup = new Pickupable();
                    //itemPickup.SetTechTypeOverride(slotItem);
                    //InventoryItem item = new InventoryItem(itemPickup);

                    GameObject itemObject = CraftData.InstantiateFromPrefab(slotItem);
                    Pickupable itemPickupable = itemObject.GetComponent<Pickupable>();
                    InventoryItem item = new InventoryItem(itemPickupable);

                    Armour.AddItem(slot, item, true);
                    typeof(Pickupable).GetMethod("Deactivate", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(itemPickupable, null);
                }
            }
            else SetSlots();

            Logger.Log($"Successfully loaded items for {ID}");
        }
    }
}
