using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LineChartRender : MaskableGraphic
{
    [Range(0f, 1f)]
    public float[] values = new float[2] { 1, 1 };// 折线图中的每个点的值

    public Color BottomColor = Color.white;
    public Color TopColor = Color.white;

    public bool useEdge = false;
    public bool useLine = false;

    public bool useStandardLine = false;

    [Range(0f, 1f)]
    public float standardValue = 0.5f;

    [Range(0f, 2.5f)]
    public float lineWidth = 1.0f;

    [Range(0f, 2.5f)]
    public float edgeWidth = 1.0f;

    public Color lineColor = Color.black;
    public Color edgeColor = Color.black;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Vector2 size = GetComponent<RectTransform>().rect.size;
        vh.Clear();
        int partCount = values.Length;
        float spx = size.x / (float)(partCount - 1);    // x坐标的间隔
        float nx = 0;
        float nx1 = 0;

        float height = size.y;
        float sph = size.y * 0.5f;  // 半边的高度和宽度
        float spw = size.x * 0.5f;
        for (int i = 0; i < partCount - 1; ++i)     // 对于每一个点
        {
            nx = spx * i - spw; // 得到当前点的位置
            nx1 = nx + spx; // 得到当前点宽度的位置

            float x1 = (float)i / partCount;
            float x2 = (float)(i + 1) / partCount;

            UIVertex v1 = UIVertex.simpleVert;
            UIVertex v2 = UIVertex.simpleVert;
            UIVertex v3 = UIVertex.simpleVert;
            UIVertex v4 = UIVertex.simpleVert;

            v1.position = new Vector2(nx, height * values[i] - sph);
            v2.position = new Vector3(nx, -sph);
            v3.position = new Vector3(nx1, -sph);
            v4.position = new Vector2(nx1, height * values[i + 1] - sph);

            v1.color = Color.Lerp(BottomColor, TopColor, values[i]);
            v2.color = Color.Lerp(BottomColor, TopColor, 0);
            v3.color = Color.Lerp(BottomColor, TopColor, 0);
            v4.color = Color.Lerp(BottomColor, TopColor, values[i + 1]);

            v1.uv0 = new Vector2(x1, values[i]);
            v2.uv0 = new Vector2(x1, 0);
            v3.uv0 = new Vector2(x2, 0);
            v4.uv0 = new Vector2(x2, values[i + 1]);

            if (useLine)
            {
                if (i == 0)
                {
                    vh.AddUIVertexQuad(GetLine(v1.position, v2.position, lineColor, lineWidth));
                }
                vh.AddUIVertexQuad(GetLine(v3.position, v4.position, lineColor, lineWidth));
            }
            vh.AddUIVertexQuad(new UIVertex[4] { v4, v3, v2, v1 });

            if (useEdge)
            {
                vh.AddUIVertexQuad(GetLine(v1.position, v4.position, edgeColor, edgeWidth));
            }

        }
        if (useStandardLine)
        {
            Vector2 startPos = new Vector2(-spw,standardValue*height-sph);
            Vector2 endPos = new Vector2(spw,standardValue*height-sph);
            vh.AddUIVertexQuad(GetLine(startPos, endPos, edgeColor, edgeWidth));
        }
    }

    private UIVertex[] GetLine(Vector2 start, Vector2 end, Color lcolor, float lineWidth)
    {
        UIVertex[] vs = new UIVertex[4];
        Vector2[] uv = new Vector2[4];
        // 线其实就是一个长方体，
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 0);
        uv[3] = new Vector2(1, 1);

        Vector2 v1 = end - start;
        Vector2 v2 = (v1.y == 0f) ? new Vector2(0f, 1f) : new Vector2(1f, -v1.x / v1.y);
        v2.Normalize();
        v2 *= lineWidth / 2f;

        Vector2[] pos = new Vector2[4];
        pos[0] = start + v2;
        pos[1] = end + v2;
        pos[2] = end - v2;
        pos[3] = start - v2;

        for (int i = 0; i < 4; ++i)
        {
            UIVertex v = UIVertex.simpleVert;
            v.color = lcolor;
            v.position = pos[i];
            v.uv0 = uv[i];
            vs[i] = v;
        }
        return vs;
    }
}
