Texture2D<float4> _DrawTex;
float2 _DrawTexSize;
int _TextureFilterMode;

float4 TextureSampler(int2 id)
{
    float4 col = float4(0, 0, 0, 0);
    float2 scale = float2(_Resolution) / float2(_DrawTexSize);

    if (_TextureFilterMode == 0)
    {
        col = _DrawTex[UVToID(IdToUV(id), _DrawTexSize)];
    }
    else
    {
        float sumWeight = 0;
        for (int x = (int) (scale.x * - 0.5); x <= (int) (scale.x * 0.5); x++)
        {
            for (int y = (int) (scale.y * - 0.5); y <= (int) (scale.y * 0.5); y++)
            {
                int2 samplePoint = (UVToID(IdToUV(id)) + int2(x, y)) / scale;
                samplePoint.x = samplePoint.x <= 0 ? 0 : samplePoint.x;
                samplePoint.x = samplePoint.x >= _DrawTexSize.x ? _DrawTexSize.x - 1 : samplePoint.x;
                samplePoint.y = samplePoint.y <= 0 ? 0 : samplePoint.y;
                samplePoint.y = samplePoint.y >= _DrawTexSize.y ? _DrawTexSize.y - 1 : samplePoint.y;
                float2 weight2 = 1 - min(abs(float2(x, y)) / scale, 1) * 0.5;
                weight2 = pow(weight2, scale);
                float weight = (weight2.x + weight2.y);
                col += _DrawTex[samplePoint] * weight;
                sumWeight += weight;
            }
        }
        col /= sumWeight;
    }
    
    return col;
}

float4 NormalTextureSampler(int2 id)
{
    float2 normal = float2(0.0, 0.0);
    int2 offsetId = id;
    offsetId.x = id.x + 1 >= _Resolution.x ? _Resolution.x - 1 : id.x + 1;
    normal.x = rgbToBright(TextureSampler(offsetId).xyz);

    offsetId = id;
    offsetId.x = id.x - 1 <= 0 ? 0 : id.x - 1;
    normal.x -= rgbToBright(TextureSampler(offsetId).xyz);

    offsetId = id;
    offsetId.y = id.y + 1 >= _Resolution.y ? _Resolution.y - 1 : id.y + 1;
    normal.y = rgbToBright(TextureSampler(offsetId).xyz);

    offsetId = id;
    offsetId.y = id.y - 1 <= 0 ? 0 : id.y - 1;
    normal.y -= rgbToBright(TextureSampler(offsetId).xyz);

    return float4(normal.x, normal.y, 1, 1);
}
