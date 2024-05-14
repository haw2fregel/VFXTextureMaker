float2 _UVPivot;
float2 _UVScale;
float _UVRotate;
float2 _UVBend;
int _UVPolar;

float2 UVScale(float2 uv)
{
    return (uv - _UVPivot) * _UVScale + _UVPivot;
}

float2 UVRotate(float2 uv)
{
    float rotate = _UVRotate * (3.1415926f / 180.0f);
    float2 result = uv - _UVPivot;
    float s = sin(rotate);
    float c = cos(rotate);
    float2x2 rMatrix = float2x2(c, -s, s, c);
    rMatrix *= 0.5;
    rMatrix += 0.5;
    rMatrix = rMatrix * 2 - 1;
    result.xy = mul(result.xy, rMatrix);
    result += _UVPivot;
    return result;
}

float2 UVRotate(float2 uv, float2 center, float value)
{
    float rotate = value * (3.1415926f / 180.0f);
    float2 result = uv - center;
    float s = sin(rotate);
    float c = cos(rotate);
    float2x2 rMatrix = float2x2(c, -s, s, c);
    rMatrix *= 0.5;
    rMatrix += 0.5;
    rMatrix = rMatrix * 2 - 1;
    result.xy = mul(result.xy, rMatrix);
    result += center;
    return result;
}

uint2 UVRotate(uint2 id, float2 center, float value)
{
    float2 uv = IdToUV(id);
    float rotate = value * (3.1415926f / 180.0f);
    float2 result = uv - center;
    float s = sin(rotate);
    float c = cos(rotate);
    float2x2 rMatrix = float2x2(c, -s, s, c);
    rMatrix *= 0.5;
    rMatrix += 0.5;
    rMatrix = rMatrix * 2 - 1;
    result.xy = mul(result.xy, rMatrix);
    result += center;
    return UVToID(result);
}

float2 UVBend(float2 uv)
{
    return uv + float2(abs(uv.y - _UVPivot.x) * _UVBend.x, abs(uv.x - _UVPivot.y) * _UVBend.y);
}

float2 UVPolar(float2 uv)
{
    float2 delta = uv - float2(0.5, 0.5);
    delta.x = delta.x == 0 ? EPSILON : delta.x;
    delta.y = delta.y == 0 ? EPSILON : delta.y;
    float radius = lengthN(delta, 2) * 2;
    float angle = atan2(delta.x, delta.y) * (1.0 / 6.28) + 0.5;
    return _UVPolar == 0 ? uv : float2(radius, angle);
}