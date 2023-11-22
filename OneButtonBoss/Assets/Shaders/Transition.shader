Shader "Unlit/Transition"
{
    Properties
    {
        [PerRenderData] _MainTex ("Texture", 2D) = "white" {}
        _Fill ("Fill", Range(0,1)) = 0
        [PerRenderData] _Color ("Color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest [unity_GUIZTestMode]

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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _Fill;
            fixed4 _Color;
            float4 _ClipRect;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                #ifdef UNITY_UI_CLIP_RECT  
                    col.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                clip(col.a - 0.001);

                if(col.r < _Fill)
                    return _Color * i.color;
                
                return 0;
            }
            ENDCG
        }
    }
}
