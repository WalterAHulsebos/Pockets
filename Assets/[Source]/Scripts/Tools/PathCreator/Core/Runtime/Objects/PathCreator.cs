using System;
using System.Collections.Generic;
using UnityEngine;
//using Utilites.CGTK;

#if ODIN_INSPECTOR

using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;

#endif

namespace PathCreation
{
    public sealed class PathCreator : MonoBehaviour
    {
        /// This class stores data for the path editor, and provides accessors to get the current vertex and bezier path.
        /// Attach to a GameObject to create a new path editor.
        public event Action pathUpdated;

        [HideInInspector]
        [SerializeField] PathCreatorData editorData;
        
        [HideInInspector]
        [SerializeField] bool initialized;

        GlobalDisplaySettings globalEditorDisplaySettings;

        // Vertex path created from the current bezier path
        public VertexPath path
        {
            get
            {
                if (!initialized)
                {
                    InitializeEditorData (false);
                }
                return editorData.vertexPath;
            }
        }

        // The bezier path created in the editor
        public BezierPath bezierPath
        {
            get
            {
                if (!initialized)
                {
                    InitializeEditorData (false);
                }
                return editorData.bezierPath;
            }
            set
            {
                if (!initialized)
                {
                    InitializeEditorData (false);
                }
                editorData.bezierPath = value;
            }
        }

        #region Internal methods

        /// Used by the path editor to initialise some data
        public void InitializeEditorData (bool in2DMode)
        {
            if (editorData == null)
            {
                editorData = new PathCreatorData ();
            }
            editorData.bezierOrVertexPathModified -= OnPathUpdated;
            editorData.bezierOrVertexPathModified += OnPathUpdated;

            editorData.Initialize (transform.position, in2DMode);
            initialized = true;
        }

        public PathCreatorData EditorData
        {
            get
            {
                return editorData;
            }

        }

        private void OnPathUpdated()
        {
            if (pathUpdated != null)
            {
                pathUpdated();
            }
            
            for (int i = 0; i < path.NumVertices; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= path.NumVertices)
                {
                    if (path.isClosedLoop)
                    {
                        nextIndex %= path.NumVertices;
                    }
                    else
                    {
                        break;
                    }
                }
                
                //CGDebug.DrawLine(path.vertices[i], path.vertices[nextIndex]);
            }
        }

        #if UNITY_EDITOR

        // Draw the path when path objected is not selected (if enabled in settings)
        private void OnDrawGizmos ()
        {
            if (path == null) 
                return;
            
            globalEditorDisplaySettings = globalEditorDisplaySettings ?? GlobalDisplaySettings.Load();

            if (!globalEditorDisplaySettings.alwaysDrawPath) 
                return;
            
            // Only draw path gizmo if the path object is not selected
            // (editor script is resposible for drawing when selected)
            GameObject selectedObj = UnityEditor.Selection.activeGameObject;
            
            if (selectedObj == gameObject) 
                return;
            
            Gizmos.color = globalEditorDisplaySettings.bezierPath;

            for (int i = 0; i < path.NumVertices; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= path.NumVertices)
                {
                    if (path.isClosedLoop)
                    {
                        nextIndex %= path.NumVertices;
                    }
                    else
                    {
                        break;
                    }
                }
                Gizmos.DrawLine(path.vertices[i], path.vertices[nextIndex]);
            }
        }
        #endif

        #endregion
    }
}