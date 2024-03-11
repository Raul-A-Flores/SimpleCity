using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace SimpleCity.AI {

    public class RoadHelper : MonoBehaviour
    {
        [SerializeField]
        protected List<Marker> pedestrianMarkers;
        [SerializeField]
        protected bool isCorner;
        [SerializeField]
        protected bool hasCrossWalks;

        float approximateThresholdCorner = 0.3f;

        public virtual Marker GetPositionForPedestrianSpawn(Vector3 structurePosition)
        {
            return GetClosestMarkerTo(structurePosition, pedestrianMarkers);
        }

        private Marker GetClosestMarkerTo(Vector3 structurePosition, List<Marker> pedestrianMarkers, bool isCorner = false)
        {
            if(isCorner)
            {
                foreach (var marker in pedestrianMarkers)
                {
                    var direction = marker.Position - structurePosition;
                    direction.Normalize();
                    if(Mathf.Abs(direction.x) < approximateThresholdCorner || Mathf.Abs(direction.z) < approximateThresholdCorner)
                    {
                        return marker;  
                    }

                }
                return null;
                
            }
            else
            {
                Marker closestMarker = null;
                float distance = float.MaxValue;
                foreach (var marker in pedestrianMarkers)
                {
                    var markerDistance = Vector3.Distance(structurePosition, marker.Position);
                    if(distance > markerDistance)
                    {
                        distance = markerDistance;
                        closestMarker = marker;
                    }

                }
                return closestMarker;
            }
        }

        public Vector3 GetClosestPedestrianPosition(Vector3 currentPosition)
        {
            return GetClosestMarkerTo(currentPosition, pedestrianMarkers, isCorner).Position;

        }

        public List<Marker> GetAllPedestrianMarkers()
        {
            return pedestrianMarkers;
        }
    }

}

