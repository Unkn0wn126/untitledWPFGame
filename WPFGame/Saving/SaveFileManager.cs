using Engine.Saving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace WPFGame.Saving
{
    public static class SaveFileManager
    {
        /// <summary>
        /// Saves the game to a given path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="save"></param>
        public static void SaveGame(string path, Save save)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, save);
            }
        }

        /// <summary>
        /// Loads a game based on the file path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Save LoadGame(string path)
        {
            Save save;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var binaryFormatter = new BinaryFormatter();
                save = (binaryFormatter.Deserialize(fs)) as Save;
            }

            return save;
        }
    }
}
