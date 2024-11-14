using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickerWheel : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform linesParent;
    [SerializeField] private Transform PickerWheelTransform;
    [SerializeField] private Transform wheelCircle;
    [SerializeField] public GameObject wheelPiecePrefab;
    [SerializeField] public Pieces wheelPiecePrefab1;
    [SerializeField] private Transform wheelPiecesParent;

    [Header("Picker Wheel Settings:")]
    [SerializeField][Range(0.2f, 2f)] private float wheelSize = 1f;
    public DataSpin dataSpin;

    private bool _isSpinning = false;
    public bool IsSpinning => _isSpinning;

    private Vector2 pieceMinSize = new Vector2(81f, 146f);
    private Vector2 pieceMaxSize = new Vector2(144f, 213f);
    private int piecesMin = 2;
    private int piecesMax = 12;

    public float pieceAngle;
    private float halfPieceAngle;
    private float halfPieceAngleWithPaddings;

    public List<Pieces> piecesList;

    private void Start()
    {
        pieceAngle = 360 / dataSpin.dataList.Count;
        halfPieceAngle = pieceAngle / 2f;
        halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);
        Generate();
    }

    private void Generate()
    {
        wheelPiecePrefab = InstantiatePiece();
        RectTransform rt = wheelPiecePrefab.transform.GetChild(0).GetComponent<RectTransform>();

        float pieceWidth = Mathf.Lerp(pieceMinSize.x, pieceMaxSize.x, 1f - Mathf.InverseLerp(piecesMin, piecesMax, dataSpin.dataList.Count));
        float pieceHeight = Mathf.Lerp(pieceMinSize.y, pieceMaxSize.y, 1f - Mathf.InverseLerp(piecesMin, piecesMax, dataSpin.dataList.Count));
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);

        for (int i = 0; i < dataSpin.dataList.Count; i++)
        {
            DrawPiece(i);
        }

        Destroy(wheelPiecePrefab);
    }

    private void DrawPiece(int index)
    {
        SpinItem piece = dataSpin.dataList[index];

        Pieces pie = InstantiatePiece1();

        pie.Icon = piece.icon;
        pie.Amount = piece.Amount;

        Transform pieceTrns = pie.transform.GetChild(0);

        pieceTrns.GetChild(1).GetComponent<Image>().sprite = piece.icon;
        pieceTrns.GetChild(2).GetComponent<Text>().text = piece.Amount.ToString();

        Transform lineTrns = Instantiate(linePrefab, linesParent.position, Quaternion.identity, linesParent).transform;
        lineTrns.RotateAround(wheelPiecesParent.position, Vector3.back, (pieceAngle * index) + halfPieceAngle);
        pie.transform.RotateAround(wheelPiecesParent.position, Vector3.back, pieceAngle * index);


        piecesList.Add(pie);
    }

    private GameObject InstantiatePiece()
    {
        return Instantiate(wheelPiecePrefab, wheelPiecesParent.position, Quaternion.identity, wheelPiecesParent);
    }

    private Pieces InstantiatePiece1()
    {
        return Instantiate(wheelPiecePrefab1, wheelPiecesParent.position, Quaternion.identity, wheelPiecesParent);
    }

    private void OnValidate()
    {
        if (PickerWheelTransform != null)
            PickerWheelTransform.localScale = new Vector3(wheelSize, wheelSize, 1f);
    }

}
