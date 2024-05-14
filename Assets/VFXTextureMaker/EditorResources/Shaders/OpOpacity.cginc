float _Opacity;
float _OpacityPower;
float4 _DrawChannel;
int _Blend;
float _Contrast;
bool _IsMultiplyAlpha;
bool _IsOneMinus;

inline float4 Contrast(float4 baseColor, float midpoint = 0.5)
{
    return saturate((baseColor - midpoint) * _Contrast + midpoint);
}

float4 Blend(float4 baseColor, float4 drawColor, float4 maskColor = float4(1, 1, 1, 1))
{
    drawColor = saturate(drawColor);
    drawColor = _IsOneMinus ? float4(1, 1, 1, 1) - drawColor : drawColor;
    drawColor = pow(drawColor, _OpacityPower.xxxx);
    drawColor = Contrast(drawColor, 0.0);

    float4 col = lerp(drawColor.xxxx, drawColor.yyyy, step(float4(1, 1, 1, 1), _DrawChannel));
    col = lerp(col, drawColor.zzzz, step(float4(2, 2, 2, 2), _DrawChannel));
    col = lerp(col, drawColor.wwww, step(float4(3, 3, 3, 3), _DrawChannel));
    col = lerp(col, rgbToBright(drawColor.xyz).xxxx, step(float4(4, 4, 4, 4), _DrawChannel));
    col = lerp(col, float4(1, 1, 1, 1), step(float4(5, 5, 5, 5), _DrawChannel));
    col = lerp(col, float4(0, 0, 0, 0), step(float4(6, 6, 6, 6), _DrawChannel));
    
    float4 opacity = _Opacity.xxxx * rgbToBright(maskColor.rgb).xxxx;
    opacity = _IsMultiplyAlpha ? opacity * drawColor.wwww : opacity;
    opacity = lerp(opacity.xxxx, float4(0, 0, 0, 0), step(float4(7, 7, 7, 7), _DrawChannel));

    switch(_Blend)
    {
        case 0:
            baseColor = lerp(baseColor, col, opacity);
            break;
        case 1:
            baseColor += col * opacity;
            break;
        case 2:
            baseColor -= col * opacity;
            break;
        case 3:
            baseColor = lerp(baseColor, baseColor * col, opacity);
            break;
        case 4:
            baseColor = lerp(baseColor, baseColor / col, opacity);
            break;
        case 5:
            baseColor = max(baseColor, col * opacity);
            break;
        case 6:
            baseColor = min(baseColor, col * opacity);
            break;
        case 7:
            baseColor = lerp(baseColor, 1.0 - (1.0 - baseColor) * (1.0 - col), opacity);
            break;
        case 8:
            float4 overlay1 = 1.0 - 2.0 * (1.0 - baseColor) * (1.0 - col);
            float4 overlay2 = 2.0 * baseColor * col;
            col = overlay2 * step(baseColor, float4(0.5, 0.5, 0.5, 0.5)) + (1 - step(baseColor, float4(0.5, 0.5, 0.5, 0.5))) * overlay1;
            baseColor = lerp(baseColor, col, opacity);
            break;
        case 9:
            baseColor = lerp(baseColor, abs(col - baseColor), opacity);
            break;
    }

    return saturate(baseColor);
}