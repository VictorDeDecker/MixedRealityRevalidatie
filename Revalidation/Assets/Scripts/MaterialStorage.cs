using System.Collections.Generic;
using UnityEngine;

public class MaterialStorage : MonoBehaviour
{
    public List<Material> Materials = new List<Material>();
    public Dictionary<string, Material> MaterialDictionary = new();

    private void Start()
    {
        if (Materials.Count > 0)
        {
            for (int i = 0; i < Materials.Count; i++)
            {
                MaterialDictionary.Add(Materials[i].name, Materials[i]);
            }
        }
    }

    public List<Material> GetHighlightedMaterials()
    {
        return new List<Material>()
        {
            MaterialDictionary["Pink"],
            MaterialDictionary["Red"],
            MaterialDictionary["Green"],
            MaterialDictionary["Yellow"]
        };
    }
}
