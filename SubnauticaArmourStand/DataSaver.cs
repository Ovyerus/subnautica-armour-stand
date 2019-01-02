using System.IO;
using System.Collections.Generic;
using Oculus.Newtonsoft.Json;
using SMLHelper.V2.Utility;

namespace SubnauticaArmourStand
{
    class DataSaver
    {
        public Dictionary<string, int> Data;
        private readonly string ID;

        public DataSaver(string id)
        {
            ID = id;
        }

        private string SaveDirectory => Path.Combine(SaveUtils.GetCurrentSaveDataDir(), "ArmourStand");
        private string SaveFile => Path.Combine(SaveDirectory, ID + ".json");

        public void Save(Dictionary<string, int> data)
        {
            if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory);

            string serialised = JsonConvert.SerializeObject(data);
            File.WriteAllText(SaveFile, serialised);

            Logger.Log($"Successfully saved items for {ID}.");
        }

        public bool Load()
        {
            if (!File.Exists(SaveFile))
            {
                Logger.Log($"Failed to load items for {ID} due to no save file.");
                return false;
            }

            string serialised = File.ReadAllText(SaveFile);
            Dictionary<string, int> data = JsonConvert.DeserializeObject<Dictionary<string, int>>(serialised);

            // Failed to load for some reason.
            if (data == null)
            {
                Logger.Log($"Failed to load items for {ID} due to deserialisation error.");
                return false;
            }

            Data = data;
            return true;
        }
    }
}
