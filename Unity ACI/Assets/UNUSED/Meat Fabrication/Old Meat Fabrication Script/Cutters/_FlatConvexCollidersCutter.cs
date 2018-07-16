using UnityEngine;
using System.Collections.Generic;
using UnitySpriteCutter.Tools;

namespace UnitySpriteCutter.Cutters
{

    internal static class _FlatConvexCollidersCutter
    {

        public class CutResult
        {
            public List<_PolygonColliderParametersRepresentation> firstSideColliderRepresentations;
            public List<_PolygonColliderParametersRepresentation> secondSideColliderRepresentations;

            public bool DidNotCut()
            {
                return (firstSideColliderRepresentations.Count == 0 || secondSideColliderRepresentations.Count == 0);
            }
        }

        public static CutResult Cut(Vector2 lineStart, Vector2 lineEnd, Collider2D[] colliders)
        {
            CutResult result = new CutResult();
            result.firstSideColliderRepresentations = new List<_PolygonColliderParametersRepresentation>();
            result.secondSideColliderRepresentations = new List<_PolygonColliderParametersRepresentation>();

            foreach (Collider2D collider in colliders)
            {

                List<Vector2[]> paths = _ColliderPathsCreator.GetPolygonColliderPathsFrom(collider);
                foreach (Vector2[] path in paths)
                {
                    _ShapeCutter.CutResult cutResult = _ShapeCutter.CutShapeIntoTwo(lineStart, lineEnd, path);

                    if (cutResult.firstSidePoints.Length > 0)
                    {
                        _PolygonColliderParametersRepresentation repr = new _PolygonColliderParametersRepresentation();
                        repr.CopyParametersFrom(collider);
                        repr.paths.Add(cutResult.firstSidePoints);
                        result.firstSideColliderRepresentations.Add(repr);
                    }
                    if (cutResult.secondSidePoints.Length > 0)
                    {
                        _PolygonColliderParametersRepresentation repr = new _PolygonColliderParametersRepresentation();
                        repr.CopyParametersFrom(collider);
                        repr.paths.Add(cutResult.secondSidePoints);
                        result.secondSideColliderRepresentations.Add(repr);
                    }

                }
            }

            return result;
        }

    }

}