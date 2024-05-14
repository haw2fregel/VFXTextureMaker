float _NoiseLengthPow;
float2 _NoiseSelfWarp;
float2 _NoiseSelfWarpTile;
float2 _NoiseSelfWarpOffset;
float2 _NoiseTile;
float2 _NoiseOffset;
float4 _NoiseWeight;
bool _NoiseSeamless;
int _NoiseRandomSeed;
float _VoronoiEdgeSize;

float valueNoise21(float2 p, float2 period)
{
    float2 tile = floor(p);
    float4 hash = float4(0, 0, 0, 0);
    float2 grid = float2(0, 0);
    [unroll]
    for (int y = 0; y < 2; y++)
    {
        [unroll]
        for (int x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    float2 fade = frac(p);
    fade = fade * fade * (3.0f - 2.0f * fade);
    return lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);
}

float2 valueNoise22(float2 p, float2 period)
{
    float2 tile = floor(p);
    float2 hash = float2(0.0, 0.0);
    float4 hashx = float4(0, 0, 0, 0);
    float4 hashy = float4(0, 0, 0, 0);
    float2 grid = float2(0, 0);
    [unroll]
    for (int y = 0; y < 2; y++)
    {
        [unroll]
        for (int x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash = hash22(grid + _NoiseRandomSeed * period);
            hashx[x + 2 * y] = hash.x;
            hashy[x + 2 * y] = hash.y;
        }
    }
    float2 fade = frac(p);
    fade = fade * fade * (3.0f - 2.0f * fade);
    return float2(lerp(lerp(hashx[0], hashx[1], fade.x), lerp(hashx[2], hashx[3], fade.x), fade.y), lerp(lerp(hashy[0], hashy[1], fade.x), lerp(hashy[2], hashy[3], fade.x), fade.y));
}

float valueNoiseGrad21(float2 p, float2 period)
{
    float2 uv = p - float2(0.1, 0.0);
    float2 fade = frac(uv);
    float2 tile = floor(uv);
    float4 hash = float4(0, 0, 0, 0);
    float2 grid = float2(0, 0);
    float2 result = float2(0, 0);
    int y, x = 0;
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.x = lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    uv = p + float2(0.1, 0.0);
    fade = frac(uv);
    tile = floor(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.x -= lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    uv = p - float2(0.0, 0.1);
    fade = frac(uv);
    tile = floor(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.y = lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    uv = p + float2(0.0, 0.1);
    fade = frac(uv);
    tile = floor(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.y -= lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    return abs(dot(float2(1.0, 1.0), abs(result)));
}

float2 valueNoiseGrad22(float2 p, float2 period)
{
    float2 uv = p - float2(0.1, 0.0);
    float2 fade = frac(uv);
    float2 tile = floor(uv);
    float4 hash = float4(0, 0, 0, 0);
    float2 grid = float2(0, 0);
    float2 result = float2(0, 0);
    int y, x = 0;
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.x = lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    uv = p + float2(0.1, 0.0);
    fade = frac(uv);
    tile = floor(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.x -= lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    uv = p - float2(0.0, 0.1);
    fade = frac(uv);
    tile = floor(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.y = lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    uv = p + float2(0.0, 0.1);
    fade = frac(uv);
    tile = floor(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless ? modulo(grid, period) : grid;
            hash[x + 2 * y] = hash21(grid + _NoiseRandomSeed * period);
        }
    }
    fade = fade * fade * (3.0f - 2.0f * fade);
    result.y -= lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y);

    return result;
}

float complexValueNoise21(float2 p)
{
    float noise1 = valueNoise21(p, _NoiseTile);
    float2 warp1 = valueNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile) - float2(0.5, 0.5);
    float2 grad1 = valueNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile);
    float noise2 = valueNoise21(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float noise3 = valueNoise21(p + warp1 * _NoiseSelfWarp, _NoiseTile);

    float noise4 = 0.0;
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;
    [unroll]
    for (int i = 0; i < 6; i++)
    {
        noise4 += amplitude * valueNoise21(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float4 value = float4
    (
        noise1,
        noise2,
        noise3,
        noise4
    );
    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return abs(dot(weight4, value));
}

float2 complexValueNoise22(float2 p)
{
    float2 noise1 = valueNoise22(p, _NoiseTile);
    float2 warp1 = valueNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile) - float2(0.5, 0.5);
    float2 grad1 = valueNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile);
    float2 noise2 = valueNoise22(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float2 noise3 = valueNoise22(p + warp1 * _NoiseSelfWarp, _NoiseTile);

    float2 noise4 = float2(0.0, 0.0);
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;
    [unroll]
    for (int i = 0; i < 6; i++)
    {
        noise4 += amplitude * valueNoise22(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return noise1 * weight4.x + noise2 * weight4.y + noise3 * weight4.z + noise4 * weight4.w;
}

float complexValueNoiseGrad21(float2 p)
{
    float noise1 = valueNoiseGrad21(p, _NoiseTile);
    float2 warp1 = valueNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile) - float2(0.5, 0.5);
    float2 grad1 = valueNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile);
    float noise2 = valueNoiseGrad21(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float noise3 = valueNoiseGrad21(p + warp1 * _NoiseSelfWarp, _NoiseTile);

    float noise4 = 0.0;
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;
    [unroll]
    for (int i = 0; i < 6; i++)
    {
        noise4 += amplitude * valueNoiseGrad21(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float4 value = float4
    (
        noise1,
        noise2,
        noise3,
        noise4
    );
    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return abs(dot(weight4, value));
}

float2 complexValueNoiseGrad22(float2 p)
{
    float2 noise1 = valueNoiseGrad22(p, _NoiseTile);
    float2 warp1 = valueNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile) - float2(0.5, 0.5);
    float2 grad1 = valueNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile * _NoiseSelfWarpTile);
    float2 noise2 = valueNoiseGrad22(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float2 noise3 = valueNoiseGrad22(p + warp1.xx * _NoiseSelfWarp, _NoiseTile);

    float2 noise4 = float2(0.0, 0.0);
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;
    [unroll]
    for (int i = 0; i < 6; i++)
    {
        noise4 += amplitude * valueNoiseGrad22(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return noise1 * weight4.x + noise2 * weight4.y + noise3 * weight4.z + noise4 * weight4.w;
}

float ValueNoise(float2 uv)
{
    float noise = 0;
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    noise = complexValueNoise21(p);
    noise = saturate(noise);
    return noise;
}

float2 ValueNoiseDisplacement(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return complexValueNoise22(p) - float2(0.5, 0.5);
}

float ValueNoiseGrad(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return complexValueNoiseGrad21(p) * 3;
}

float2 ValueNoiseGradDistorion(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return complexValueNoiseGrad22(p);
}

float perlinNoise21(float2 p, float2 period)
{
    float2 tile = floor(p);
    float2 fade = frac(p);
    float4 hash = float4(0, 0, 0, 0);
    float2 grid = float2(0, 0);
    [unroll]
    for (int y = 0; y < 2; y++)
    {
        [unroll]
        for (int x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    return 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;
}

float2 perlinNoise22(float2 p, float2 period)
{
    float2 tile = floor(p);
    float2 hash = float2(0.0, 0.0);
    float4 hashx = float4(0, 0, 0, 0);
    float4 hashy = float4(0, 0, 0, 0);
    float2 fade = frac(p);
    float2 grid = float2(0, 0);
    [unroll]
    for (int y = 0; y < 2; y++)
    {
        [unroll]
        for (int x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash = gtable22(grid + _NoiseRandomSeed * period, fade - float2(x, y));
            hashx[x + 2 * y] = hash.x;
            hashy[x + 2 * y] = hash.y;
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    return float2(lerp(lerp(hashx[0], hashx[1], fade.x), lerp(hashx[2], hashx[3], fade.x), fade.y), lerp(lerp(hashy[0], hashy[1], fade.x), lerp(hashy[2], hashy[3], fade.x), fade.y));
}

float perlinNoiseGrad21(float2 p, float2 period)
{
    float2 uv = p - float2(0.1, 0.0);
    float2 tile = floor(uv);
    float2 fade = frac(uv);
    float4 hash = float4(0, 0, 0, 0);
    float2 grid = float2(0, 0);
    float2 result = float2(0, 0);
    int y, x = 0;
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.x = 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    uv = p + float2(0.1, 0.0);
    tile = floor(uv);
    fade = frac(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.x -= 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    uv = p - float2(0.0, 0.1);
    tile = floor(uv);
    fade = frac(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.y = 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    uv = p + float2(0.0, 0.1);
    tile = floor(uv);
    fade = frac(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.y -= 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    return abs(dot(float2(1.0, 1.0), abs(result)));
}

float2 perlinNoiseGrad22(float2 p, float2 period)
{
    float2 uv = p - float2(0.1, 0.0);
    float2 tile = floor(uv);
    float2 fade = frac(uv);
    float4 hash = float4(0, 0, 0, 0);
    float2 grid = float2(0, 0);
    float2 result = float2(0, 0);
    int y, x = 0;
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.x = 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    uv = p + float2(0.1, 0.0);
    tile = floor(uv);
    fade = frac(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.x -= 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    uv = p - float2(0.0, 0.1);
    tile = floor(uv);
    fade = frac(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.y = 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    uv = p + float2(0.0, 0.1);
    tile = floor(uv);
    fade = frac(uv);
    hash = float4(0, 0, 0, 0);
    grid = float2(0, 0);
    [unroll]
    for (y = 0; y < 2; y++)
    {
        [unroll]
        for (x = 0; x < 2; x++)
        {
            grid = tile + float2(x, y);
            grid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            hash[x + 2 * y] = gtable21(grid + _NoiseRandomSeed * period, fade - float2(x, y));
        }
    }
    fade = fade * fade * fade * (10.0 - 15.0 * fade + 6.0 * fade * fade);
    result.y -= 0.5 * lerp(lerp(hash[0], hash[1], fade.x), lerp(hash[2], hash[3], fade.x), fade.y) + 0.5;

    return result;
}

float complexPerlinNoise21(float2 p)
{
    float noise1 = perlinNoise21(p, _NoiseTile);
    float2 warp1 = perlinNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float2 grad1 = perlinNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float noise2 = perlinNoise21(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float noise3 = perlinNoise21(p + warp1.xx * _NoiseSelfWarp, _NoiseTile);

    float noise4 = 0.0;
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;
    [unroll]
    for (int jbxCount = 0; jbxCount < 6; jbxCount++)
    {
        noise4 += amplitude * perlinNoise21(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float4 value = float4
    (
        noise1,
        noise2,
        noise3,
        noise4
    );
    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return abs(dot(weight4, value));
}

float2 complexPerlinNoise22(float2 p)
{
    float2 noise1 = perlinNoise22(p, _NoiseTile);
    float2 warp1 = perlinNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float2 grad1 = perlinNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float2 noise2 = perlinNoise22(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float2 noise3 = perlinNoise22(p + warp1.xx * _NoiseSelfWarp, _NoiseTile);

    float2 noise4 = float2(0.0, 0.0);
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;

    [unroll]
    for (int jbxCount = 0; jbxCount < 6; jbxCount++)
    {
        noise4 += amplitude * perlinNoise22(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return noise1 * weight4.x + noise2 * weight4.y + noise3 * weight4.z + noise4 * weight4.w;
}

float complexPerlinNoiseGrad21(float2 p)
{
    float noise1 = perlinNoiseGrad21(p, _NoiseTile);
    float2 warp1 = perlinNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float2 grad1 = perlinNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float noise2 = perlinNoiseGrad21(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float noise3 = perlinNoiseGrad21(p + warp1.xx * _NoiseSelfWarp, _NoiseTile);

    float noise4 = 0.0;
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;
    [unroll]
    for (int jbxCount = 0; jbxCount < 6; jbxCount++)
    {
        noise4 += amplitude * perlinNoiseGrad21(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float4 value = float4
    (
        noise1,
        noise2,
        noise3,
        noise4
    );
    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return abs(dot(weight4, value));
}


float2 complexPerlinNoiseGrad22(float2 p)
{
    float2 noise1 = perlinNoiseGrad22(p, _NoiseTile);
    float2 warp1 = perlinNoise22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float2 grad1 = perlinNoiseGrad22(p * _NoiseSelfWarpTile + _NoiseSelfWarpOffset, _NoiseTile);
    float2 noise2 = perlinNoiseGrad22(p + grad1 * _NoiseSelfWarp, _NoiseTile);
    float2 noise3 = perlinNoiseGrad22(p + warp1.xx * _NoiseSelfWarp, _NoiseTile);

    float2 noise4 = float2(0.0, 0.0);
    float amplitude = 0.5;
    float frequency = 0.;
    float2 uv = p;
    float2 tile = _NoiseTile;

    [unroll]
    for (int jbxCount = 0; jbxCount < 6; jbxCount++)
    {
        noise4 += amplitude * perlinNoiseGrad22(uv, tile);
        uv *= 2.0f;
        tile *= 2.0f;
        amplitude *= 0.5f;
    }

    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    return noise1 * weight4.x + noise2 * weight4.y + noise3 * weight4.z + noise4 * weight4.w;
}

float PerlinNoise(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return complexPerlinNoise21(p);
}

float2 PerlinNoiseDisplacement(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return complexPerlinNoise22(p);
}

float PerlinNoiseGrad(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return complexPerlinNoiseGrad21(p) * 3;
}

float2 PerlinNoiseGradDisplacement(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return complexPerlinNoiseGrad22(p);
}


float4 sort(float4 list, float hash)
{
    float4 res = step(hash, list);
    return res.x == 1 ? float4(hash, list.xyz) :
    res.y == 1 ? float4(list.x, hash, list.yz) :
    res.z == 1 ? float4(list.xy, hash, list.z) :
    res.w == 1 ? float4(list.xyz, hash) :
    list;
}

float4 fdist24(float2 p, float2 period)
{
    float2 tile = floor(p + 0.5);
    float4 dist4 = float4(sumAbs(1.5 - abs(p - tile)).xxxx);
    float2 grid = float2(0, 0);
    float2 jglid = float2(0, 0);
    float2 jitter = float2(0, 0);
    [unroll]
    for (int i = 0; i <= 4; i++)
    {
        grid.y = tile.y + sign(fmod(i, 2.0) - 0.5) * ceil(i * 0.5);
        if (abs(grid.y - p.y) - 0.5 > dist4.y)
        {
            continue;
        }
        [unroll]
        for (int j = -2; j <= 2; j++)
        {
            grid.x = tile.x + j;
            jglid = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            jitter = hash22(jglid + _NoiseRandomSeed * period) - 0.5;
            dist4 = sort(dist4, lengthN(grid + jitter - p, _NoiseLengthPow));
        }
    }
    return dist4;
}

float DistanceNoise(float2 uv)
{
    float noise = 0;
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;
    noise = abs(dot(weight4, fdist24(p, _NoiseTile)));
    noise = saturate(noise);
    return noise;
}

float2 DistanceNoiseDisplacement(float2 uv)
{
    float noise = 0.0;
    float2 displacement = float2(0.0, 0.0);
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    float weight = abs(_NoiseWeight.x) + abs(_NoiseWeight.y) + abs(_NoiseWeight.z) + abs(_NoiseWeight.w);
    float4 weight4 = _NoiseWeight / weight;

    float2 distUV = p - float2(0.1, 0.0);
    displacement.x = abs(dot(weight4, fdist24(distUV, _NoiseTile)));

    distUV = p + float2(0.1, 0.0);
    displacement.x -= abs(dot(weight4, fdist24(distUV, _NoiseTile)));

    distUV = p - float2(0.0, 0.1);
    displacement.y = abs(dot(weight4, fdist24(distUV, _NoiseTile)));

    distUV = p + float2(0.0, 0.1);
    displacement.y -= abs(dot(weight4, fdist24(distUV, _NoiseTile)));

    return displacement;
}

float2 voronoi2(float2 p, float2 period)
{
    float2 tile = floor(p + 0.5);
    float dist = sqrt(2.0);
    float2 id = float2(0, 0);
    float2 grid = float2(0, 0);
    float2 glidj = float2(0, 0);
    float2 jitter = float2(0, 0);
    [unroll]
    for (int i = 0; i <= 2; i++)
    {
        grid.y = tile.y + sign(fmod(i, 2.0) - 0.5) * ceil(i * 0.5);
        if (abs(grid.y - p.y) - 0.5 > dist)
        {
            continue;
        }
        [unroll]
        for (int j = -1; j <= 1; j++)
        {
            grid.x = tile.x + j;
            glidj = _NoiseSeamless == 1 ? modulo(grid, period) : grid;
            jitter = hash22(glidj + _NoiseRandomSeed) - 0.5;
            dist = min(dist, lengthN(grid + jitter - p, _NoiseLengthPow));
            if (lengthN(grid + jitter - p, _NoiseLengthPow) <= dist)
            {
                dist = lengthN(grid + jitter - p, _NoiseLengthPow);
                id = glidj;
            }
        }
    }
    return id;
}

float voronoi21(float2 p)
{
    return hash21(voronoi2(p, _NoiseTile));
}

float2 voronoi22(float2 p)
{
    return hash22(voronoi2(p, _NoiseTile));
}

float voronoiEdge(float2 p)
{
    float result = 0;
    float center = voronoi21(p);
    float2 pixelSize = float2(1, 1) / _Resolution * _NoiseTile * _VoronoiEdgeSize;
    result += abs(center - voronoi21(p + float2(pixelSize.x, 0)));
    result += abs(center - voronoi21(p + float2(0, pixelSize.y)));
    result += abs(center - voronoi21(p - float2(pixelSize.x, 0)));
    result += abs(center - voronoi21(p - float2(0, pixelSize.y)));
    return step(0.001, result);
}

float Voronoi(float2 uv)
{
    float noise = 0;
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    noise = voronoi21(p);
    noise = saturate(noise);
    return noise;
}

float2 VoronoiDisplacement(float2 uv)
{
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    return voronoi22(p);
}

float VoronoiEdge(float2 uv)
{
    float noise = 0;
    float2 p = abs(uv * _NoiseTile + _NoiseOffset * _NoiseTile);
    noise = voronoiEdge(p);
    noise = saturate(noise);
    return noise;
}