Shader "Custom/BlendedPanoramicSkybox"
{
    Properties
    {
        _Tex1 ("Day Skybox", 2D) = "white" {}
        _Tex2 ("Night Skybox", 2D) = "black" {}
        _Blend ("Blend", Range(0,1)) = 0
        _Exposure ("Exposure", Range(0,8)) = 1
        _Rotation ("Rotation", Range(0,360)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Tex1;
            sampler2D _Tex2;
            float _Blend;
            float _Exposure;
            float _Rotation;

            struct appdata { float4 vertex : POSITION; };
            struct v2f { float3 dir : TEXCOORD0; float4 pos : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.dir = normalize(mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv;
                uv.x = (atan2(i.dir.x, i.dir.z) / (2 * UNITY_PI) + 0.5 + _Rotation/360) % 1;
                uv.y = saturate(i.dir.y * 0.5 + 0.5);

                fixed4 col1 = tex2D(_Tex1, uv);
                fixed4 col2 = tex2D(_Tex2, uv);

                return lerp(col1, col2, _Blend) * _Exposure;
            }
            ENDCG
        }
    }
}
