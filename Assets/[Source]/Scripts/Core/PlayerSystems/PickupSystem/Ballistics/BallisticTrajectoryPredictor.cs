using System;
using JetBrains.Annotations;

namespace Core.PlayerSystems.Ballistics
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    
    [Serializable]
    public class BallisticTrajectoryPredictor
    {   
        #region Variables
        
        /// <summary>
        /// The accuracy of the prediction. This controls the distance between steps in calculation.
        /// </summary>
        [Range(0.5f, 0.9999f)]
        public float accuracy = 0.98f;
        
        /// <summary>
        /// How many steps the prediction can take before stopping.
        /// </summary>
        public int iterationLimit = 500;

        /// <summary>
        /// Stop the prediction where the line hits an object?
        /// </summary>
        public bool checkForCollision = true;
        
        /// <summary>
        /// If checkForCollision is set to true this will perform a bounce at the impact location using the objects' physics material.
        /// </summary>
        public bool bounceOnCollision = true;
        
        /// <summary>
        /// The layer mask to used when calculating prediction. /n
        /// This setting only matters when checkForCollision is on.
        /// </summary>
        public LayerMask raycastMask = 1 >> 9;

        public Vector3? endMark;
        
        /// <summary>
        /// Duration the prediction line lasts for. When predicting every frame its a good idea to update this value to Time.unscaledDeltaTime every frame.
        /// </summary>
        public float debugLineDuration = 4f;

        private enum PredictionMode
        {
            DrawTrajectory,
            PositionsOnly,
            All,
        }
        
        [NonSerialized] public RaycastHit hitInfo3D;
        
        #endregion

        /*
        public void DrawTrajectory(List<Vector3> pointList)
        {
            DrawTrajectory(pointList: pointList, lineRenderer: new GameObject(){name = "Debug Line"}.AddComponent<LineRenderer>());
        }
        */
        
        public void DrawTrajectory(List<Vector3> pointList, LineRenderer lineRenderer)
        {
            if (lineRenderer == null){ return;}
            
            if (lineRenderer.sortingLayerID != 0 || lineRenderer.sortingOrder != 0)
            {
                Debug.LogFormat("LineRenderer sortingLayer = {0}", lineRenderer.sortingLayerID);
                Debug.LogFormat("LineRenderer sortingOrder = {0}", lineRenderer.sortingOrder);
            }

            if (lineRenderer.sharedMaterial != null)
            {
                //lineRenderer.sharedMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
            }

            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());
        }
        
        ///<summary>
        ///Given these values, perform velocity prediction in 3D. 
        ///Results can be found in the instance of the class' hitInfo and predictionPoints variables.
        ///</summary>
        public List<Vector3> PredictTrajectory(Vector3 startPos, Vector3 velocity, Vector3 gravity, [CanBeNull] PhysicMaterial physicMaterial = null, float linearDrag = 0f, LineRenderer lineRenderer = null)
        {
            return PerformPrediction(
                startPos: startPos, 
                velocity: velocity, 
                gravity: gravity, 
                linearDrag: linearDrag, 
                physicMaterial : physicMaterial,
                lineRenderer : lineRenderer);
        }

        ///<summary>
        ///Given a RigidBody, perform velocity prediction in 3D.
        ///Results can be found in the instance of the class' hitInfo and predictionPoints variables.
        ///</summary>
        public List<Vector3> PredictTrajectory(Rigidbody rigidbody, [CanBeNull] PhysicMaterial physicMaterial = null, LineRenderer lineRenderer = null)
        {
            return PerformPrediction(
                startPos: rigidbody.position, 
                velocity: rigidbody.velocity, 
                gravity: Physics.gravity, 
                linearDrag: rigidbody.drag, 
                physicMaterial: physicMaterial,
                lineRenderer: lineRenderer);
        }
        
        ///<summary>
        ///Given these values, get an array of points representing the trajectory in 3D without needing to create an instance of the class or use it as a component.
        ///</summary>
        public List<Vector3> GetTrajectoryPoints(Vector3 startPos, Vector3 velocity, Vector3 gravity, [CanBeNull] PhysicMaterial physicMaterial = null, float linearDrag = 0f)
        {
            return PerformPrediction
            (
                startPos: startPos, 
                velocity: velocity, 
                gravity: gravity, 
                linearDrag: linearDrag, 
                physicMaterial : physicMaterial,
                predictionMode: PredictionMode.PositionsOnly
            );
        }
        
        ///<summary>
        ///Given these values, get an array of points representing the trajectory in 3D without needing to create an instance of the class or use it as a component.
        ///</summary>
        public List<Vector3> GetTrajectoryPoints(Rigidbody rigidbody, [CanBeNull] PhysicMaterial physicMaterial = null)
        {
            return PerformPrediction
            (
                startPos: rigidbody.position, 
                velocity: rigidbody.velocity, 
                gravity: Physics.gravity, 
                physicMaterial: physicMaterial,
                linearDrag: rigidbody.drag, 
                predictionMode: PredictionMode.PositionsOnly
            );
        }
        
        private List<Vector3> PerformPrediction(Vector3 startPos, Vector3 velocity, Vector3 gravity, [CanBeNull] PhysicMaterial physicMaterial, float linearDrag = 0f, LineRenderer lineRenderer = null, PredictionMode predictionMode = PredictionMode.All)
        {
            List<Vector3> predictionPoints = new List<Vector3>();
            Vector3 direction = Vector3.zero;
            Vector3 endPos;
            int currentIteration = 0;
            
            float lineDistance = 0f;
            bool done = false;
    
            float invertedAccuracy = (1f - accuracy);
            
            Vector3 gravAdd = (gravity * invertedAccuracy);
            float dragMultiplier = Mathf.Clamp01(1f - (linearDrag * invertedAccuracy));
            
            while(!done && currentIteration < iterationLimit)
            {
                velocity += gravAdd;
                velocity *= dragMultiplier;
                
                endPos = startPos + (velocity * invertedAccuracy);
                direction = endPos - startPos;
                predictionPoints.Add(startPos);
    
                float dist = Vector3.Distance(startPos, endPos);
                lineDistance += dist;
                
                if(checkForCollision)
                {
                    Ray ray = new Ray(startPos, direction);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, dist, raycastMask))
                    {                        
                        hitInfo3D = hit;
                        predictionPoints.Add(hit.point);
                        if(bounceOnCollision)
                        {                                
                            if(physicMaterial != null)
                            {
                                endPos = hit.point;
                                velocity = Vector3.Reflect(velocity, hit.normal) * physicMaterial.bounciness;
                                if (physicMaterial.bounciness <= 0.01f)
                                {
                                    endMark = endPos;
                                    done = true;
                                }
                            }
                            else
                            {
                                endMark = endPos;
                                done = true;
                            }
                        }
                        else
                        {
                            endMark = endPos;
                            done = true;
                        }
                    }
                }

                #if UNITY_EDITOR
                Debug.DrawRay(startPos, direction, Color.red, debugLineDuration);
                #endif

                startPos = endPos;
                currentIteration++;
            }

            if ((predictionMode == PredictionMode.DrawTrajectory || predictionMode == PredictionMode.All)
                && lineRenderer != null)
            {
                DrawTrajectory(predictionPoints, lineRenderer);
            }
            
            return predictionPoints;
        }
        
    }
}
