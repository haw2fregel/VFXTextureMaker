float _ShiftHue;
float _ShiftSaturation;
float _ShiftValue;
float _Temperature;
float _Tint;

float3 HSVShift(float3 rgb)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 P = lerp(float4(rgb.zy, K.wz), float4(rgb.yz, K.xy), step(rgb.z, rgb.y));
    float4 Q = lerp(float4(P.xyw, rgb.x), float4(rgb.x, P.yzx), step(P.x, rgb.x));
    float D = Q.x - min(Q.w, Q.y);
    float3 hsv = float3(abs(Q.z + (Q.w - Q.y) / (6.0 * D + EPSILON)), D / (Q.x + EPSILON), Q.x);

    hsv.x = frac(hsv.x + _ShiftHue);
    hsv.y = saturate(hsv.y + _ShiftSaturation);
    hsv.z = saturate(hsv.z + _ShiftValue);

    float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
    return hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
}

float3 WhiteBalance(float3 color)
{
    float t1 = clamp(_Temperature, -1.67, 1.67) * 10 / 6;
    float t2 = clamp(_Tint, -1.67, 1.67) * 10 / 6;

    float x = 0.31271 - t1 * (t1 < 0 ? 0.1 : 0.05);
    float standardIlluminantY = 2.87 * x - 3 * x * x - 0.27509507;
    float y = standardIlluminantY + t2 * 0.05;

    float3 w1 = float3(0.949237, 1.03542, 1.08728);

    float Y = 1;
    float X = Y * x / y;
    float Z = Y * (1 - x - y) / y;
    float L = 0.7328 * X + 0.4296 * Y - 0.1624 * Z;
    float M = -0.7036 * X + 1.6975 * Y + 0.0061 * Z;
    float S = 0.0030 * X + 0.0136 * Y + 0.9834 * Z;
    float3 w2 = float3(L, M, S);

    float3 balance = float3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);

    float3x3 LIN_2_LMS_MAT = {
        3.90405e-1, 5.49941e-1, 8.92632e-3,
        7.08416e-2, 9.63172e-1, 1.35775e-3,
        2.31082e-2, 1.28021e-1, 9.36245e-1
    };

    float3x3 LMS_2_LIN_MAT = {
        2.85847e+0, -1.62879e+0, -2.48910e-2,
        -2.10182e-1,  1.15820e+0,  3.24281e-4,
        -4.18120e-2, -1.18169e-1,  1.06867e+0
    };

    float3 lms = mul(LIN_2_LMS_MAT, color);
    lms *= balance;
    return mul(LMS_2_LIN_MAT, lms);
}

float3 ColorBalance(float3 color)
{
    color = saturate(color);
    color = WhiteBalance(color);
    color = HSVShift(color);
    return color;
}