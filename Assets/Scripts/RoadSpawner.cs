using UnityEngine;

public class RoadSpawner : MonoBehaviour {
    [SerializeField] private int startingLaneCount = 7;
    [SerializeField] private int maxSectionCount = 15;
    private int laneCount;

    [SerializeField] private GameObject lanePrefab;
    [SerializeField] private GameObject curbPrefab;
    private float curbHeight = 0.15f;

    void Start() {
        laneCount = startingLaneCount;
        for (int i = -2; i < maxSectionCount; i++) {
            CreateLane(i);
        }
    }

    void Update() {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).position -= new Vector3(0, 0, 25 * Time.deltaTime);
        }

        if (transform.GetChild(0).position.z <= -15) {
            Destroy(transform.GetChild(0).gameObject);
            CreateLane(maxSectionCount - 1);
        }
    }

    void CreateLane(int sectionsAhead) {
        float laneWidth = lanePrefab.transform.localScale.x;
        float laneLength = lanePrefab.transform.localScale.z;
        float laneOffset = -laneCount / 2 * laneWidth;

        Transform roadSection = new GameObject("Road Section").transform;
        roadSection.transform.parent = transform;
        roadSection.position = new Vector3(0, 0, sectionsAhead * laneLength);
        roadSection.rotation = Quaternion.identity;
        roadSection.localScale = new Vector3(1, 1, 1);

        GameObject newCurb = Instantiate(curbPrefab, roadSection);
        newCurb.transform.localPosition = new Vector3(
            laneOffset - (laneWidth / 2) - (curbPrefab.transform.localScale.x / 2),
            curbHeight,
            0
        );

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

        newCurb = Instantiate(curbPrefab, roadSection);
        newCurb.transform.localPosition = new Vector3(
            laneOffset + ((laneCount - 1) * laneWidth) + (laneWidth / 2) + (curbPrefab.transform.localScale.x / 2),
            curbHeight,
            0
        );
    }
}