using UnityEngine;
using System.Collections.Generic;
using UnitySpriteCutter.Cutters;
using UnitySpriteCutter.Tools;

namespace UnitySpriteCutter
{

    /// <summary>
    /// Holds gameObject destined to cut and performs all operations that modifies its parameters / components.
    /// </summary>
    internal class _SpriteCutterGameObject
    {

        public GameObject gameObject
        {
            get;
            private set;
        }

        private _SpriteCutterGameObject()
        {
        }

        public static _SpriteCutterGameObject CreateAs(GameObject origin)
        {
            _SpriteCutterGameObject result = new _SpriteCutterGameObject();
            result.gameObject = origin;
            return result;
        }

        public static _SpriteCutterGameObject CreateNew(GameObject origin, bool secondSide)
        {
            Debug.Log("CREATED NEW");
            _SpriteCutterGameObject result = new _SpriteCutterGameObject();
            result.gameObject = new GameObject(origin.name + (!secondSide ? "_firstSide" : "_secondSide"));
            result.CopyGameObjectParametersFrom(origin);
            result.CopyTransformFrom(origin.transform);
            return result;
        }

        public static _SpriteCutterGameObject CreateAsInstantiatedCopyOf(GameObject origin, bool secondSide)
        {
            //Debug.Log("CREATED COPY");
            _SpriteCutterGameObject result = new _SpriteCutterGameObject();
            result.gameObject = GameObject.Instantiate(origin);
            result.gameObject.name = origin.name + (!secondSide ? "_firstSide" : "_secondSide");
            result.gameObject.transform.SetParent(MeatFabManager.Instance.ParentOfSlicedObjects);
            return result;
        }

        void CopyGameObjectParametersFrom(GameObject other)
        {
            gameObject.isStatic = other.isStatic;
            gameObject.layer = other.layer;
            gameObject.tag = other.tag;
        }

        void CopyTransformFrom(Transform transform)
        {
            gameObject.transform.position = transform.position;
            gameObject.transform.rotation = transform.rotation;
            gameObject.transform.localScale = transform.localScale;
        }

        public void AssignMeshFilter(Mesh mesh)
        {
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;
        }

        public void AssignMeshRendererFrom(SpriteRenderer spriteRenderer)
        {
            _RendererParametersRepresentation tempParameters = new _RendererParametersRepresentation();
            tempParameters.CopyFrom(spriteRenderer);
            AssignMeshRendererFrom(tempParameters);
        }

        public void AssignMeshRendererFrom(MeshRenderer meshRenderer)
        {
            _RendererParametersRepresentation tempParameters = new _RendererParametersRepresentation();
            tempParameters.CopyFrom(meshRenderer);
            AssignMeshRendererFrom(tempParameters);
        }

        public void AssignMeshRendererFrom(_RendererParametersRepresentation tempParameters)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            tempParameters.PasteTo(meshRenderer);
        }

        public void BuildCollidersFrom(List<_PolygonColliderParametersRepresentation> representations)
        {
            foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
            {
                Collider2D.Destroy(collider);
            }
            foreach (_PolygonColliderParametersRepresentation representation in representations)
            {
                PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
                representation.PasteTo(collider);
            }
        }
    }

}