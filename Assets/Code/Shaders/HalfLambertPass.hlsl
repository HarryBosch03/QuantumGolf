#include "./HalfLambert.hlsl"
#include "./HalfLambertStructure.hlsl"

Varyings HalfLambertVertex(Attributes input)
{
    Varyings o;
    
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, o);
    
    o.positionWS = TransformObjectToWorld(input.positionOS);
    o.positionCS = TransformWorldToHClip(o.positionWS);
    o.normalWS = TransformObjectToWorldNormal(input.normalOS);
    o.uv = input.uv;
    return o;
}

half4 HalfLambertFragment(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);
    
    float3 albedo = _Color.rgb;
    float alpha = _Color.a;
    
    SurfaceData surface;
    surface.albedo = albedo;
    surface.positionWS = input.positionWS;
    surface.normalWS = input.normalWS;
    
    half3 res = LitHalfLambert(surface);
                
    return float4(res, alpha);
}