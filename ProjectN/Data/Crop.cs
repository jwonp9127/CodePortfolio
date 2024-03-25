using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : IDataContent
{
    public string Name { get; set; }
    public int ID { get; set; }
    public int[] GrowthDays { get; set; }
    public string[] VariationPrefabPaths { get; set; }
    public string HarvestPrefabPath { get; set; }
    public string HarvestQuantity { get; set; }

    public Crop()
    {
        GrowthDays = new int[] { };
        VariationPrefabPaths = new string[] { };
    }
    
    public Crop(Crop crop)
    {
        Name = crop.Name;
        ID = crop.ID;
        GrowthDays = new int[crop.GrowthDays.Length];
        Array.Copy(crop.GrowthDays, GrowthDays, crop.GrowthDays.Length);
        VariationPrefabPaths = new string[crop.VariationPrefabPaths.Length];
        Array.Copy(crop.VariationPrefabPaths, VariationPrefabPaths, crop.VariationPrefabPaths.Length);
        HarvestPrefabPath = crop.HarvestPrefabPath;
        HarvestQuantity = crop.HarvestQuantity;
    }
}
