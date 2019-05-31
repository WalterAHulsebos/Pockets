using Sirenix.Utilities;

namespace Utilities.CGTK
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using System.Linq;
    using Utilities;
    using static CGDebug;
    
    public class GizmoDrawer : EnsuredSingleton<GizmoDrawer>
    {
        private readonly List<Gizmo> gizmos = new List<CGDebug.Gizmo>();
    
        private void Update()
        {
            foreach (Gizmo gizmo in gizmos)
            {
                gizmo.durationLeft -= Time.deltaTime;
            }
        }
    
        private void OnDrawGizmos()
        {
            foreach (Gizmo gizmo in gizmos)
            {
                Color prevColor = Gizmos.color;
                Matrix4x4 prevMatrix = Gizmos.matrix;
                Gizmos.color = gizmo.color;
    
                if (gizmo.matrix != default)
                {
                    Gizmos.matrix = gizmo.matrix;
                }
                gizmo.action();
    
                Gizmos.color = prevColor;
                Gizmos.matrix = prevMatrix;
    
                // we remove the drawing here and not in the update to ensure that
                // drawings with 0 duration will be drawn at least one time.
                if (gizmo.durationLeft <= 0)
                {
                    gizmos.Remove(gizmo);
                }
            }
        }
    
        public static void Draw(Gizmo drawing)
        {
            Instance.gizmos.Add(drawing);
        }
        
        public static void Draw(params Gizmo[] drawings)
        {
            Instance.gizmos.AddRange(drawings);
        }
    }
}