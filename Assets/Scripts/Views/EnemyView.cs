using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public Animator animator;


    //Sight
    // editor header
    [Header("Sight")]
    private MeshFilter sightZoneMeshFilter;
    private Mesh SightZoneMesh;
    private float sightZoneRadius, sightZoneAngle;
    public Material SightZoneMaterial;
    public LayerMask SightZoneLayers;
    public int SightZoneResolution = 30;

    public void Initialize(EnemyModel model)
    {
        sightZoneAngle = model.sightAngle;
        sightZoneRadius = model.sightRadius;
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (sightZoneMeshFilter == null)
        {
            sightZoneMeshFilter = new GameObject("SightZone").AddComponent<MeshFilter>();
            SightZoneMesh = new Mesh();
            sightZoneMeshFilter.transform.SetParent(transform);
            sightZoneMeshFilter.gameObject.AddComponent<MeshRenderer>().material = SightZoneMaterial;
            sightZoneMeshFilter.transform.SetLocalPositionAndRotation(new Vector3(0f, 1f, 0f), Quaternion.Euler(0f, 0f, 0f));
            sightZoneAngle *= Mathf.Deg2Rad; // convert the angle to radians
        }
    }

    private void Update()
    {
        DrawSightZone();
    }

    public void Attack()
    {
        TriggerAttackAnimation();
    }

    public void UpdateMovement(float speed)
    {
        animator.SetBool("IsRunning", speed > 0.01f);
        animator.SetFloat("Speed", speed);
    }

    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    private void DrawSightZone()
    {
        int[] triangles = new int[(SightZoneResolution - 1) * 3];
        Vector3[] vertices = new Vector3[SightZoneResolution + 1];
        vertices[0] = Vector3.zero;
        float currentAngle = -sightZoneAngle / 2;
        float angleIncrement = sightZoneAngle / (SightZoneResolution - 1);

        Vector3 raycastTransform = transform.position + Vector3.up;

        for (int i = 0; i < SightZoneResolution; i++)
        {
            float sine = Mathf.Sin(currentAngle);
            float cosine = Mathf.Cos(currentAngle);
            Vector3 raycastDirection = (transform.forward * cosine) + (transform.right * sine);
            Vector3 vertForward = (Vector3.forward * cosine) + (Vector3.right * sine);

            Debug.DrawRay(raycastTransform, raycastDirection * sightZoneRadius, Color.red);

            if (Physics.Raycast(raycastTransform, raycastDirection, out RaycastHit hit, sightZoneRadius, SightZoneLayers))
            {
                vertices[i + 1] = vertForward * hit.distance;
            }
            else
            {
                vertices[i + 1] = vertForward * sightZoneRadius;
            }

            currentAngle += angleIncrement;
        }

        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }

        SightZoneMesh.Clear();
        SightZoneMesh.vertices = vertices;
        SightZoneMesh.triangles = triangles;
        sightZoneMeshFilter.mesh = SightZoneMesh;
    }
}
