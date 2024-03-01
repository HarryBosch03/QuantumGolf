#include "./HalfLambert.hlsl"
#include "./HalfLambertStructure.hlsl"

Varyings HalfLambertVertex(Attributes v)
{
    Varyings o;
    o.positionWS = TransformObjectToWorld(v.positionOS);
    o.positionCS = TransformWorldToHClip(o.positionWS);
    o.normalWS = TransformObjectToWorldNormal(v.normalOS);
    o.uv = v.uv;
    return o;
}

half4 HalfLambertFragment(Varyings input) : SV_Target
{
    float3 albedo = _Color.rgb;
    float alpha = _Color.a;
    
    SurfaceData surface;
    surface.albedo = albedo;
    surface.positionWS = input.positionWS;
    surface.normalWS = input.normalWS;
    
    half3 res = LitHalfLambert(surface);
                
    return float4(res, alpha);
}