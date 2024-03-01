#include "./HalfLambertPass.hlsl"
#include "./Common.hlsl"

Varyings HLDitherVertex(Attributes v)
{
    return HalfLambertVertex(v);
}

half4 HLDitherFragment(Varyings input) : SV_Target
{
    float x = 1 - input.uv.y;
    x = 1 - pow(saturate((x - _Start) / (_End - _Start)), _Slope);
    float2 uv = input.positionCS.xy;
    float dither = Dither(uv);
    clip(x - dither);

    return HalfLambertFragment(input);
}