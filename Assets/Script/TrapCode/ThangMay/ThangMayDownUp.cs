using System.Collections;
using UnityEngine;

public class ThangMayDownUp : MonoBehaviour
{
    public float distanceDown = 2f;    // Khoảng cách di chuyển xuống
    public float moveSpeed = 2f;       // Tốc độ di chuyển
    public float pauseDuration = 5f;   // Thời gian dừng

    private Vector3 startPosition;     // Vị trí bắt đầu

    void Start()
    {
        startPosition = transform.position; // Lưu lại vị trí bắt đầu
        StartCoroutine(MoveDownAndUp());    // Bắt đầu coroutine di chuyển
    }

    IEnumerator MoveDownAndUp()
    {
        while (true)
        {
            // Di chuyển xuống khoảng distanceDown
            Vector3 targetPositionDown = startPosition - Vector3.up * distanceDown;
            float elapsedTime = 0f;

            while (elapsedTime < distanceDown / moveSpeed)
            {
                // Lerp từ vị trí bắt đầu đến vị trí xuống dưới
                transform.position = Vector3.Lerp(startPosition, targetPositionDown, elapsedTime * moveSpeed / distanceDown);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPositionDown;

            // Dừng lại trong pauseDuration giây
            yield return new WaitForSeconds(pauseDuration);

            // Di chuyển lên trở lại vị trí bắt đầu
            elapsedTime = 0f;
            while (elapsedTime < distanceDown / moveSpeed)
            {
                // Lerp từ vị trí xuống dưới trở lại vị trí bắt đầu
                transform.position = Vector3.Lerp(targetPositionDown, startPosition, elapsedTime * moveSpeed / distanceDown);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = startPosition;

            // Dừng lại trong pauseDuration giây
            yield return new WaitForSeconds(pauseDuration);
        }
    }
}
