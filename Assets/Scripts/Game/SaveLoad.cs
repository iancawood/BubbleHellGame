﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour
{

    public GameObject prefab;
    public Slider senSlider;
    public Slider audSlider;
    public Toggle vibToggle;

    // Use this for initialization
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        SettingsData data = new SettingsData();

        data.speed = senSlider.value;
        data.volume = audSlider.value;
        data.vibra = vibToggle.isOn;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            SettingsData data = (SettingsData)bf.Deserialize(file);
            file.Close();

            senSlider.value = data.speed;
            audSlider.value = data.volume;
            vibToggle.isOn = data.vibra;
        }
    }

    [Serializable]
    class SettingsData
    {
        public float speed;
        public float volume;
        public bool vibra;
    }

}