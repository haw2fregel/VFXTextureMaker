#define k uint3(0x456789abu, 0x6789ab45u, 0x89ab4567u)
#define UINT_MAX 0xffffffffu
#define u uint3(1, 2, 3)

inline uint2 uhash22(uint2 n)
{
    n ^= n.yx << uint2(u.xy);
    n ^= n.yx >> uint2(u.xy);
    n *= k.xy;
    n ^= n.yx << uint2(u.xy);
    return n *= k.xy;
}

inline float2 hash22(float2 p)
{
    return float2(uhash22((uint2)p) / float2(UINT_MAX, UINT_MAX));
}

inline float hash21(float2 p)
{
    return float(uhash22((uint2)p).x / float(UINT_MAX));
}


inline float gtable21(float2 lattice, float2 p)
{
    uint ind = uhash22((uint2)lattice).x >> 29;
    float uvu = 0.92387953 * (ind < 4u ? p.x : p.y);
    float vcc = 0.38268343 * (ind < 4u ? p.x : p.y);
    return ((ind & 1u) == 0u ? uvu : - uvu) + ((ind & 2u) == 0u ? vcc : - vcc);
}

inline float2 gtable22(float2 lattice, float2 p)
{
    uint ind = uhash22((uint2)lattice).x >> 29;
    float uvu = 0.92387953 * (ind < 4u ? p.x : p.y);
    float vcc = 0.38268343 * (ind < 4u ? p.x : p.y);
    return float2((ind & 1u) == 0u ? uvu : - uvu, (ind & 2u) == 0u ? vcc : - vcc);
}