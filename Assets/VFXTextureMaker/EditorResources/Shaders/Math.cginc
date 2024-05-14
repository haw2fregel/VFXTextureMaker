#define PI 3.14159
#define EPSILON 1e-10

inline float lengthN(float2 p, float n)
{
    float2 tmp = pow(abs(p) + 0.0001, n.xx);
    return pow(tmp.x + tmp.y, 1.0 / n);
}

inline float sumAbs(float2 p)
{
    return abs(p.x) + abs(p.y);
}

inline float maxAbs(float2 p)
{
    return max(abs(p.x), abs(p.y));
}

inline float minAbs(float2 p)
{
    return min(abs(p.x), abs(p.y));
}

inline float2 modulo(float2 p, float2 period)
{
    return (p % period + period) % period;
}

inline float rgbToBright(float3 col)
{
    return (0.299 * col.x) + (0.587 * col.y) + (0.114 * col.z);
}