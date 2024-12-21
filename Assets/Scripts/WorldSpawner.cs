using UnityEngine;

public class WorldSpawner : MonoBehaviour {
    [SerializeField] private int maxSectionCount = 25;
    [HideInInspector] public int laneCount;
    public int startingLaneCount = 7;

    [SerializeField] private GameObject lanePrefab;
    [SerializeField] private GameObject curbPrefab;
    [SerializeField] private GameObject grassPrefab;
    [HideInInspector] public float laneWidth;
    [HideInInspector] public float laneLength;
    private float curbHeight = 0.15f;

    void Awake() {
        laneCount = startingLaneCount;
        laneWidth = lanePrefab.transform.localScale.x;
        laneLength = lanePrefab.transform.localScale.z;
    }

    void Start() {
        for (int i = -2; i < maxSectionCount; i++) {
            CreateSection(i);
        }
    }

    void FixedUpdate() {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).position -= new Vector3(0, 0, 25 * Time.fixedDeltaTime);
        }
        if (transform.GetChild(0).position.z <= -15) {
            Destroy(transform.GetChild(0).gameObject);
            CreateSection(maxSectionCount - 1);
        }
    }

    void CreateSection(int sectionsAhead) {
        float laneOffset = -laneCount / 2 * laneWidth;

        Transform newSection = new GameObject("World Section").transform;
        newSection.transform.parent = transform;
        newSection.position = new Vector3(0, 0, sectionsAhead * laneLength);
        newSection.rotation = Quaternion.identity;
        newSection.localScale = new Vector3(1, 1, 1);

        // Road Section

        Transform roadSection = new GameObject("Road").transform;
        roadSection.transform.parent = newSection;
        roadSection.localPosition = new Vector3(0, 0, 0);
        roadSection.localRotation = Quaternion.identity;
        roadSection.localScale = new Vector3(1, 1, 1);

        for (int x = 0; x < laneCount; x++) {
            GameObject newLane = Instantiate(lanePrefab, roadSection);
            newLane.transform.localPosition = new Vector3(laneOffset + (x * laneWidth), 0, 0);

            if (x == laneCount - 1) {
                Transform lineParent = newLane.transform.GetChild(0);
                for (int c = 0; c < lineParent.childCount; c++) {
                    lineParent.GetChild(c).gameObject.SetActive(false);
                }
            }
        }

        GameObject leftCurb = Instantiate(curbPrefab, roadSection);
        leftCurb.transform.localPosition = new Vector3(
            laneOffset - (laneWidth / 2) - (curbPrefab.transform.localScale.x / 2),
            curbHeight,
            0
        );
        GameObject rightCurb = Instantiate(curbPrefab, roadSection);
        rightCurb.transform.localPosition = new Vector3(
            laneOffset + ((laneCount - 1) * laneWidth) + (laneWidth / 2) + (curbPrefab.transform.localScale.x / 2),
            curbHeight,
            0
        );

        // Scenery

        GameObject leftGrass = Instantiate(grassPrefab, roadSection);
        leftGrass.transform.localPosition = new Vector3(laneOffset - (laneWidth / 2) - curbPrefab.transform.localScale.x - (grassPrefab.transform.localScale.x / 2), 0, 0);
        GameObject rightGrass = Instantiate(grassPrefab, roadSection);
        rightGrass.transform.localPosition = new Vector3(laneOffset + ((laneCount - 1) * laneWidth) + (laneWidth / 2) + curbPrefab.transform.localScale.x + (grassPrefab.transform.localScale.x / 2), 0, 0);
    }
}