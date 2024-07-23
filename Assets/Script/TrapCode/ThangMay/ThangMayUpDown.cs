using System.Collections;
using UnityEngine;

public class ThangMayUpDown : MonoBehaviour
{
    public float distanceUp = 2f;      // Khoảng cách di chuyển lên
    public float moveSpeed = 2f;       // Tốc độ di chuyển
    public float pauseDuration = 5f;   // Thời gian dừng

    private Vector3 startPosition;     // Vị trí bắt đầu

    void Start()
    {
        startPosition = transform.position; // Lưu lại vị trí bắt đầu
        StartCoroutine(MoveUpAndDown());    // Bắt đầu coroutine di chuyển
    }

    IEnumerator MoveUpAndDown()
    {
        while (true)
        {
            // Di chuyển lên khoảng distanceUp
            Vector3 targetPositionUp = startPosition + Vector3.up * distanceUp;
            float elapsedTime = 0f;

            while (elapsedTime < distanceUp / moveSpeed)
            {
                // Lerp từ vị trí bắt đầu đến vị trí lên trên
                transform.position = Vector3.Lerp(startPosition, targetPositionUp, elapsedTime * moveSpeed / distanceUp);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPositionUp;

            // Dừng lại trong pauseDuration giây
            yield return new WaitForSeconds(pauseDuration);

            // Di chuyển xuống trở lại vị trí bắt đầu
            elapsedTime = 0f;
            while (elapsedTime < distanceUp / moveSpeed)
            {
                // Lerp từ vị trí lên trên trở lại vị trí bắt đầu
                transform.position = Vector3.Lerp(targetPositionUp, startPosition, elapsedTime * moveSpeed / distanceUp);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = startPosition;

            // Dừng lại trong pauseDuration giây
            yield return new WaitForSeconds(pauseDuration);
        }
    }
}
