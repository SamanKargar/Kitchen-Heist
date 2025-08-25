using UnityEngine;

namespace _Game.Scripts.Characters.AICharacter
{
    [ExecuteInEditMode]
    public class AISensor : MonoBehaviour
    {
        [Header("Mesh Settings")] [Space(5)]
        
        [SerializeField] private float distance = 10f;
        [SerializeField] private float angle = 30f;
        [SerializeField] private float height = 1f;
        [SerializeField] private Color meshColor = Color.cyan;

        [Space(4)] [Header("Sensor Settings")] [Space(4)]
        
        [SerializeField] private int scanFrequency = 30;
        [SerializeField] private LayerMask detectableLayer;
        [SerializeField] private LayerMask occlusionLayer;
        
        private Mesh _mesh;
        private readonly Collider[] _colliders = new Collider[10];
        private GameObject _detectedTarget;

        private int _count;
        private float _scanInterval;
        private float _scanTimer;

        private void Start()
        {
            _scanInterval = 1f / scanFrequency;
        }

        private void Update()
        {
            _scanTimer -= Time.deltaTime;
            if (_scanTimer < 0)
            {
                _scanTimer += _scanInterval;
                Scan();
            }
        }

        private void Scan()
        {
            _count = Physics.OverlapSphereNonAlloc(transform.position, distance, _colliders, detectableLayer, QueryTriggerInteraction.Collide);
            
            for (int i = 0; i < _count; i++)
            {
                GameObject target = _colliders[i].gameObject;
                _detectedTarget = IsInSight(target) ? target : null;
            }
        }

        private void OnValidate()
        {
            _mesh = CreateWedgeMesh();
            _scanInterval = 1f / scanFrequency;
        }

        private bool IsInSight(GameObject target)
        {
            Vector3 origin = transform.position;
            Vector3 destination = target.transform.position;
            Vector3 direction = destination - origin;

            if (direction.y < 0 || direction.y > height)
                return false;

            // Angle checking
            direction.y = 0;
            float deltaAngle = Vector3.Angle(direction, transform.forward);
            if (deltaAngle > angle) {
                return false;
            }
            
            // Check distance if needed (optional, based on range)
            float distanceToTarget = Vector3.Distance(origin, destination);
            if (distanceToTarget > distance) {
                return false;
            }
            
            // Occlusion layer checking
            origin.y += height / 2;
            destination.y = origin.y;
            if (Physics.Linecast(origin, destination, occlusionLayer)) {
                return false;
            }

            return true;
        }

        public GameObject GetDetectedTarget()
        {
            return _detectedTarget;
        }

        #region - Mesh / Gizmos -

        private Mesh CreateWedgeMesh()
        {
            _mesh = new Mesh();

            const int SEGMENTS = 10;
            const int NUM_TRIANGLES = (SEGMENTS * 4) + 2 + 2;
            const int NUM_VERTICES = NUM_TRIANGLES * 3;
            Vector3[] vertices = new Vector3[NUM_VERTICES];
            int[] triangles = new int[NUM_VERTICES];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
            Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

            Vector3 topCenter = bottomCenter + Vector3.up * height;
            Vector3 topLeft = bottomLeft + Vector3.up * height;
            Vector3 topRight = bottomRight + Vector3.up * height;

            int vert = 0;

            // Left side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            // Right side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            float currentAngle = -angle;
            float deltaAngle = (angle * 2) / SEGMENTS;

            for (int i = 0; i < SEGMENTS; i++)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

                topLeft = bottomLeft + Vector3.up * height;
                topRight = bottomRight + Vector3.up * height;
                
                // Far side
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                // Top
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;

                // Bottom
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;
                
                currentAngle += deltaAngle;
            }

            for (int i = 0; i < NUM_VERTICES; i++)
                triangles[i] = i;

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();

            return _mesh;
        }

        private void OnDrawGizmos()
        {
            if (_mesh)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
            }
            
            Gizmos.color = Color.red;
            if (_detectedTarget)
                Gizmos.DrawSphere(_detectedTarget.transform.position, 0.3f);
        }

        #endregion
    }
}