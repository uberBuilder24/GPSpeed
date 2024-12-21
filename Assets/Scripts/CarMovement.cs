using UnityEngine;

public class CarMovement : MonoBehaviour {
    [SerializeField] private WorldSpawner worldSpawner;
    private int currentLane;
    private Vector3 targetPosition;

    void Start() {
        currentLane = Mathf.CeilToInt(worldSpawner.startingLaneCount / 2f);
        targetPosition = transform.position;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (currentLane - 1 > 0) {
                currentLane--;
                targetPosition -= new Vector3(worldSpawner.laneWidth, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentLane + 1 <= worldSpawner.laneCount) {
                currentLane++;
                targetPosition += new Vector3(worldSpawner.laneWidth, 0, 0);
            }
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);
    }
}