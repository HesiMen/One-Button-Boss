Shader "Unlit/Transition_Texture_Agnostic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CubicPulse("Cubic Pulse", COLOR) = (1,1,1,1)
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
            // make fog work

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float cubicPulse( float c, float w, float x )
            {
                x = abs(x - c);
                if( x>w ) return 0.0;
                x /= w;
                return 1.0 - x*x*(3.0-2.0*x);
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _CubicPulse;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * cubicPulse(_CubicPulse.r, _CubicPulse.g, _CubicPulse.b);
            }
            ENDCG
        }
    }
}
