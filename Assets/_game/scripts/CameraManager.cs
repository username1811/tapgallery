using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [Title("Drag:")]
    public Vector3 dragOrigin;
    public bool isDraggingOver1;
    public float draggingOver1Distance;
    public Camera cam;
    [Title("Zoom:")]
    public float zoomMouseSpeed = 1.0f;  // Tốc độ zoom
    public float zoomFingerSpeed = 1.0f;  // Tốc độ zoom
    public float minZoom = 1.0f;    // Độ zoom nhỏ nhất
    public float maxZoom = 10.0f;   // Độ zoom lớn nhất
    public bool isDoingAnim;
    public float eatableZoom;
    public bool isZoomingFinger;
    [Title("Limit position:")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;


    private void Start()
    {
        isDraggingOver1 = false;
    }

    private void Update()
    {
        if (isDoingAnim) return;

        if (cam == null) cam = Camera.main;

        if (Pinch()) {
            isZoomingFinger = true;
            ClickManager.isCanClick = false;
            return;
        } 

        if(Input.touchCount == 0)
        {
            if (isZoomingFinger)
            {
                DOVirtual.DelayedCall(Time.deltaTime * 1f, () =>
                {
                    ClickManager.isCanClick = true;
                });
            }
            isZoomingFinger = false;
        }

        if (isZoomingFinger) return;
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 diff = (Vector2)dragOrigin - (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            if (diff.magnitude > draggingOver1Distance)
            {
                isDraggingOver1 = true;
            }
            cam.transform.position += (Vector3)diff;
            // Giới hạn vị trí theo min/max X và Y
            float clampedX = Mathf.Clamp(cam.transform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(cam.transform.position.y, minY, maxY);
            cam.transform.position = new Vector3(clampedX, clampedY, cam.transform.position.z);
        }
        if (Input.GetMouseButtonUp(0))
        {
            DOVirtual.DelayedCall(Time.deltaTime * 2f, () =>
            {
                isDraggingOver1 = false;
            });
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            cam.orthographicSize -= scroll * zoomMouseSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    [Button]
    public void Punch(Vector2 dir)
    {
        cam.transform.DOPunchPosition(-dir.normalized * 0.05f, 0.2f, 6, 0).SetEase(Ease.OutSine);
    }

    public void OnLoadLevel()
    {
        InitLimitPosition();
        MoveToCenter();
        RefreshCamDistance();
        DOVirtual.DelayedCall(0.6f, () =>
        {
            AnimZoomToEatableTile();
        });
    }

    public void InitLimitPosition()
    {
        // Kiểm tra xem danh sách arrowTiles có phần tử nào không
        if (LevelManager.Ins.currentLevel.tiles.Count > 0)
        {
            // Khởi tạo giá trị ban đầu từ phần tử đầu tiên
            minX = maxX = LevelManager.Ins.currentLevel.tiles[0].transform.position.x;
            minY = maxY = LevelManager.Ins.currentLevel.tiles[0].transform.position.y;

            // Lặp qua từng phần tử trong danh sách
            foreach (var tile in LevelManager.Ins.currentLevel.tiles)
            {
                Vector3 position = tile.transform.position;

                // Cập nhật min/max X và Y
                if (position.x < minX) minX = position.x;
                if (position.x > maxX) maxX = position.x;
                if (position.y < minY) minY = position.y;
                if (position.y > maxY) maxY = position.y;
            }
        }
        float padding = 5f;
        minX -= padding;
        maxX += padding;
        minY -= padding;
        maxY += padding;
    }

    private void MoveToCenter()
    {
        cam.transform.position = new Vector3((maxX + minX) / 2f, (maxY + minY) / 2f, cam.transform.position.z);
    }

    public void AnimZoomToEatableTile(Action OnComplete = null)
    {
        float duration = 1f;
        Ease ease = Ease.OutSine;
        isDoingAnim = true;
        DOVirtual.Float(cam.orthographicSize, eatableZoom, duration, v =>
        {
            cam.orthographicSize = v;
        }).SetEase(ease).OnComplete(() =>
        {
            isDoingAnim = false;
            OnComplete?.Invoke();
        });
        Vector3 targetMove = ArrowTile.GetEatableTile().transform.position;
        targetMove.z = -18f;
        cam.transform.DOMove(targetMove, duration).SetEase(ease);
    }

    public void RefreshCamDistance()
    {
        float objectWidth = maxX - minX;
        float objectHeight = maxY - minY;

        // Tính toán tỉ lệ khung hình của màn hình
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // Đặt orthographicSize của camera dựa trên chiều lớn hơn giữa chiều rộng và chiều cao
        cam.orthographicSize = Mathf.Max(objectWidth, objectHeight) * screenAspect + 5f;
    }

    bool Pinch()
    {
        // 2 fingers
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrePos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrePos = secondTouch.position - secondTouch.deltaPosition;

            float touchesPrePosDiff = (firstTouchPrePos - secondTouchPrePos).sqrMagnitude;
            float touchesCurPosDiff = (firstTouch.position - secondTouch.position).sqrMagnitude;

            float deltaPos = (firstTouch.deltaPosition - secondTouch.deltaPosition).sqrMagnitude;
            deltaPos = Mathf.Clamp(deltaPos, 0, 400f);
            float zoomModifier = deltaPos * zoomFingerSpeed * Time.deltaTime;

            if (touchesPrePosDiff > touchesCurPosDiff)
            {
                Camera.main.orthographicSize += zoomModifier;
            }
            else if (touchesPrePosDiff < touchesCurPosDiff)
            {
                Camera.main.orthographicSize -= zoomModifier;
            }
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
            return true;
        }
        return false;
    }
}
