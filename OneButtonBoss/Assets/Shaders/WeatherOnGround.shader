Shader "Unlit/WeatherOnGround"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTexture("Dissolve Texture", 2D) = "black" {}
        _DissolveAmount("DissolveAmount", Range(0,1)) = 0.5
        _SnowMultiplier("Snow", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1: TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DissolveTexture;
            float4 _DissolveTexture_ST;
            float _DissolveAmount;
            float _SnowMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _DissolveTexture);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 dissolveTex = tex2D(_DissolveTexture, i.uv1);
                //dissolveTex - _DissolveAmount;
                fixed areaToColor = col.g - col.r;
                areaToColor *= (_SnowMultiplier * 10);
                areaToColor = areaToColor + col;

                fixed4 lerpResult = lerp(col, areaToColor, _SnowMultiplier);
                return lerpResult;
            }
            ENDCG
        }
    }
}
