using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housePrefab, specialPrefab, bigStructurePrefab;
    public PlacementManager placementManager;

    private float[] houseWeights, specialWeights, bigStructureWeights;

    private void Start()
    {
        houseWeights = housePrefab.Select(prefabstats => prefabstats.weight).ToArray();
        specialWeights = specialPrefab.Select(prefabstats => prefabstats.weight).ToArray();
        bigStructureWeights = bigStructurePrefab.Select(prefabstats => prefabstats.weight).ToArray();


    }

    public void PlaceHouse(Vector3Int position)
    {
        if(CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(houseWeights);
            placementManager.PlaceObjectOnTheMap(position, housePrefab[randomIndex].prefab, CellType.Structure);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    public void PlaceSpecial(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(specialWeights);
            placementManager.PlaceObjectOnTheMap(position, specialPrefab[randomIndex].prefab, CellType.Structure);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    private int GetRandomWeightedIndex(float[] weights)
    {
        float sum = 0f;
        for(int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }
        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            // 0 -> weight[0]
            if(randomValue >= tempSum && randomValue < tempSum + weights[i])
            {
                return i;
            }
            tempSum += weights[i];
        }
        return 0;
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)
    {
        if  (DefaultCheck(position) == false)
        {
            return false;
        }

        if( RoadCheck(position)==false)
        {
            return false;   
        }
        return true;
     
       
 
    }

    private bool RoadCheck(Vector3Int position)
    {
        if (placementManager.GetNeighborsTypesFor(position, CellType.Road).Count <= 0)
        {
            Debug.Log("Must be placed near a road");
            return false;
        }
        return true;
    }

    private bool DefaultCheck(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
        {
            Debug.Log("Is Out of Bounds");
            return false;
        }
        if (placementManager.CheckIFPositionIsFree(position) == false)
        {
            Debug.Log("Is Out of already taken");
            return false;
        }
        return true;
    }

    internal void PlaceBigStructure(Vector3Int position)
    {
        int width = 2;
        int height = 2;
        if(CheckBigStructure(position, width, height))
        {

            int randomIndex = GetRandomWeightedIndex(bigStructureWeights);
            placementManager.PlaceObjectOnTheMap(position, bigStructurePrefab[randomIndex].prefab, CellType.Structure, width, height);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    private bool CheckBigStructure(Vector3Int position, int width, int height)
    {
        bool nearRoad = false;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var newPosition = position += new Vector3Int(x, 0, z);
               
                if(DefaultCheck(newPosition)==false)
                {
                    return false;
                    
                }
                
          
                if (nearRoad == false)
                {
                    nearRoad = RoadCheck(newPosition);

                }
            }
        }

    return nearRoad;
    }
}

[Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
[Range(0,1)]
    public float weight;
}
