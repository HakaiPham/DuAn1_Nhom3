using System.Collections;
using UnityEngine;

public class ThangMay : MonoBehaviour
{
    public float distanceUp = 2f;      // Khoảng cách di chuyển lên
    public float distanceDiagonal = 5f; // Khoảng cách di chuyển theo góc
    public float moveSpeed = 2f;        // Tốc độ di chuyển
    public float pauseDuration = 5f;    // Thời gian dừng

    private Vector3 startPosition;      // Vị trí bắt đầu

    void Start()
    {
        startPosition = transform.position; // Lưu lại vị trí bắt đầu
        StartCoroutine(MoveUpAndDiagonal()); // Bắt đầu coroutine di chuyển
    }

    IEnumerator MoveUpAndDiagonal()
    {
        while (true)
        {
            // Di chuyển lên khoảng distanceUp
            Vector3 targetPositionUp = startPosition + Vector3.up * distanceUp;
            float elapsedTime = 0f;

            while (elapsedTime < distanceUp / moveSpeed)
            {
                transform.position = Vector3.Lerp(startPosition, targetPositionUp, elapsedTime * moveSpeed / distanceUp);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPositionUp;

            // Dừng lại trong 5 giây
           // yield return new WaitForSeconds(pauseDuration);

            // Di chuyển theo góc 45 độ khoảng distanceDiagonal
            Vector3 targetPositionDiagonal = targetPositionUp + new Vector3(distanceDiagonal * Mathf.Cos(45 * Mathf.Deg2Rad), distanceDiagonal * Mathf.Sin(45 * Mathf.Deg2Rad), 0);
            elapsedTime = 0f;

            while (elapsedTime < distanceDiagonal / moveSpeed)
            {
                transform.position = Vector3.Lerp(targetPositionUp, targetPositionDiagonal, elapsedTime * moveSpeed / distanceDiagonal);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPositionDiagonal;

            // Dừng lại trong 5 giây
            yield return new WaitForSeconds(pauseDuration);

            // Quay trở lại vị trí ban đầu (diagonal -> up)
            elapsedTime = 0f;
            while (elapsedTime < distanceDiagonal / moveSpeed)
            {
                transform.position = Vector3.Lerp(targetPositionDiagonal, targetPositionUp, elapsedTime * moveSpeed / distanceDiagonal);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPositionUp;

            // Di chuyển xuống trở lại vị trí bắt đầu
            elapsedTime = 0f;
            while (elapsedTime < distanceUp / moveSpeed)
            {
                transform.position = Vector3.Lerp(targetPositionUp, startPosition, elapsedTime * moveSpeed / distanceUp);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = startPosition;

            // Dừng lại trong 5 giây
            yield return new WaitForSeconds(pauseDuration);
        }
    }
}
