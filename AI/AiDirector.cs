using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


namespace SimpleCity.AI
{
    public class AiDirector : MonoBehaviour
    {
   
        public PlacementManager placementManager;
        public GameObject[] pedestrianPrefabs;

        AdjacencyGraph graph = new AdjacencyGraph();

        public void SpawnAllAgents()
        {
            foreach (var house in placementManager.GetAllHouses())
            {
                TrySpawningAnAgent(house, placementManager.GetRandomSpecialStrucutre());
            }

            foreach (var specialStructure in placementManager.GetAllSpecialStructures())
            {
                TrySpawningAnAgent(specialStructure, placementManager.GetRandomHouseStructure());
            }

        }

        private void TrySpawningAnAgent(StructureModel startStructure, StructureModel endStructure)
        {
            if(startStructure !=null && endStructure != null)
            {
                var startPosition = ((INeedingRoad)startStructure).RoadPosition;
                var endPosition = ((INeedingRoad)endStructure).RoadPosition;

                var startMarker = placementManager.GetStructureAt(startPosition).GetPedestrianSpawnMarker(startStructure.transform.position);
                var endMarker = placementManager.GetStructureAt(endPosition).GetPedestrianSpawnMarker(endStructure.transform.position);

                var agent = Instantiate(GetRandomPedestrian(), startPosition, Quaternion.identity);
                var path = placementManager.GetPathBetween(startPosition, endPosition, true);
                if (path.Count > 0)
                {
                    path.Reverse();
                    List<Vector3> agentPath = GetPedestrianPath(path, startMarker.Position, endMarker.Position);
                    var aiAgent = agent.GetComponent<AIAgent>();
                    aiAgent.Initialize(agentPath);
                   // aiAgent.Initialize(new List<Vector3>(path.Select(x => (Vector3)x).ToList()));
                }
            }
        }

        private List<Vector3> GetPedestrianPath(List<Vector3Int> path, Vector3 startPosition, Vector3 endPosition)
        {

            graph.ClearGraph();
            CreateAGraph(path);
            Debug.Log(graph);
            return AdjacencyGraph.AStarSearch(graph,startPosition, endPosition);
        }

        private void CreateAGraph(List<Vector3Int> path)
        {
            Dictionary<Marker, Vector3> tempDictionary = new Dictionary<Marker, Vector3>();

            for (int i = 0; i < path.Count; i++)
            {
                var currentPosition = path[i];
                var roadStructure = placementManager.GetStructureAt(currentPosition);
                var markersList = roadStructure.GetPedestrianMarkers();
                bool limitDistance = markersList.Count == 4;
                tempDictionary.Clear();

                foreach (var marker in markersList)
                {
                    graph.AddVertex(marker.Position);
                    foreach (var markerNeighborPos in marker.GetAdjacentPositions())
                    {
                        graph.AddEdge(marker.Position, markerNeighborPos);
                    }

                    if(marker.OpenForConnections && i+1 < path.Count)
                    {
                        var nextRoadStructure = placementManager.GetStructureAt(path[i + 1]);
                        if (limitDistance)
                        {
                            tempDictionary.Add(marker, nextRoadStructure.GetNearestMarkterTo(marker.Position));
                        }
                        else
                        {
                            graph.AddEdge(marker.Position, nextRoadStructure.GetNearestMarkterTo(marker.Position));
                        }
                    }
                }
                if(limitDistance && tempDictionary.Count == 4)
                {
                    var distanceSortedMarkers = tempDictionary.OrderBy(x => Vector3.Distance(x.Key.Position, x.Value)).ToList();


                    for (int j = 0; j < 2; j++)
                    {
                        graph.AddEdge(distanceSortedMarkers[j].Key.Position, distanceSortedMarkers[j].Value);
                    }
                }

            }
        }

        private UnityEngine.Object GetRandomPedestrian()
        {
            return pedestrianPrefabs[UnityEngine.Random.Range(0, pedestrianPrefabs.Length)];
        }

        private void Update()
        {
            foreach (var vertex in graph.GetVertices())
            {
                foreach (var vertexNeighbor in graph.GetConnectedVerticesTo(vertex))
                {
                    Debug.DrawLine(vertex.Position + Vector3.up, vertexNeighbor.Position +Vector3.up, Color.red);
                }

            }
        }
    }

}
