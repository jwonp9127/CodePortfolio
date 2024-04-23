using System;

public class Crop : IDataContent
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public int[] GrowthDays { get; private set; }
    public string[] VariationPrefabPaths { get; private set; }
    public string HarvestPrefabPath { get; private set; }
    public string HarvestQuantity { get; private set; }

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
