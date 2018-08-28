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
        [SerializeField] int playerXP;
        //string gameDataProjectFilePath = "/StreamingAssets/PlayerGuild.json";
        [SerializeField] bool hasGuild;
        [SerializeField] Guild playerGuild;

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

        public bool HasGuild
        {
            get { return hasGuild; }
            set { hasGuild = value; }
        }

        public Guild PlayerGuild
        {
            get { return playerGuild; }
            set { playerGuild = value; }
        }

        private void Awake()
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
            data.PlayerExperience = playerXP;
            data.HasGuild = hasGuild;
            if (playerGuild != null)
            {
                data.GuildInstanceId = playerGuild.GetInstanceID();
            }

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
            playerXP = data.PlayerExperience;
            hasGuild = data.HasGuild;

            if (data.GuildInstanceId != 0)
            {
                Guild guild = Array.Find(gameManager.MasterGuildList, x => x.GetInstanceID() == data.GuildInstanceId);
                playerGuild = guild;
            }

            LoadActionButtons();
        }

        void LoadActionButtons()
        {
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