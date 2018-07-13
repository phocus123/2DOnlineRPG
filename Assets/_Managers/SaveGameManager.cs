using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using RPG.Characters;
using System;

namespace RPG.Core
{
    public class SaveGameManager : MonoBehaviour
    {
        GameManager gameManager;
        Dictionary<int, string> abilityDict = new Dictionary<int, string>();
        int playerXP;

        public Dictionary<int, string> AbilityDict
        {
            get { return abilityDict; }
            set { abilityDict = value; }
        }

        public int PlayerXP
        {
            get { return playerXP; }
            set { playerXP = value; }
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();

            Load();
        }

        public void OnApplicationQuit()
        {
            Save();
        }

        public void Save()
        {
            string destination = Application.persistentDataPath + "/playerInfo.dat";
            FileStream file;

            if (File.Exists(destination))
            {
                file = File.OpenWrite(destination);
            }
            else
            {
                file = File.Create(destination);
            }

            PlayerData data = new PlayerData();
            data.AbilityDict = abilityDict;
            data.PlayerExperience = PlayerXP;

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }

        public void Load()
        {
            string destination = Application.persistentDataPath + "/playerInfo.dat";
            FileStream file;

            if (File.Exists(destination))
            {
                file = File.OpenRead(destination);
            }
            else
            {
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            abilityDict = data.AbilityDict;
            PlayerXP = data.PlayerExperience;

            if (abilityDict != null)
            {
                var uiManager = gameManager.uiManager;

                foreach (var item in abilityDict)
                {
                    Ability a = Array.Find(gameManager.MasterAbilityList, x => x.name == item.Value);

                    uiManager.ActionButtons[item.Key - 1].Ability = a;
                    uiManager.ActionButtons[item.Key - 1].Icon.sprite = a.Icon;
                    uiManager.ActionButtons[item.Key - 1].Icon.color = Color.white;
                }
            }
        }
    }
}