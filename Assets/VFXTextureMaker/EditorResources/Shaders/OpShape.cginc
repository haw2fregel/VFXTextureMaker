float _ShapeCurveArray[2048];
float2 _ShapeCenter;
float _ShapeDistancePower;
int _PolygonCount;
float _PolygonBump;


float ShapeDistancePower(float2 uv)
{
    float dist = lengthN(uv - _ShapeCenter, _ShapeDistancePower) * 2;
    dist = saturate(dist);
    int samplePoint = (int) (dist * 511);
    samplePoint = clamp(samplePoint, 0, 511);

    float drawColor1 = _ShapeCurveArray[samplePoint];
    float drawColor2 = _ShapeCurveArray[min(511, samplePoint + 1)];
    float drawColor = lerp(drawColor1, drawColor2, frac(dist * 511));
    
    return drawColor;
}

float ShapePolygon(float2 p)
{
    p -= _ShapeCenter;
    p.x = p.x == 0 ? EPSILON : p.x;
    p.y = p.y == 0 ? EPSILON : p.y;
    float arctan2 = atan2(p.x, p.y) + PI;
    float r = PI * 2.0 / float(_PolygonCount);
    float dist = cos(floor(0.5 + arctan2 / r) * r - arctan2) * max(length(p) - _PolygonBump, EPSILON) / (1.0 - _PolygonBump);

    int samplePoint = (int) (dist * 511);
    samplePoint = clamp(samplePoint, 0, 511);

    float drawColor1 = _ShapeCurveArray[samplePoint];
    float drawColor2 = _ShapeCurveArray[min(511, samplePoint + 1)];
    float drawColor = lerp(drawColor1, drawColor2, frac(dist * 511));
    
    return drawColor;
}
