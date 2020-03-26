Shader "Custom/ArcSlot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ArcStart ("ValidStart",Range(0,360)) = 0.0
        _ArcEnd   ("ValidEnd",  Range(0,360)) = 0.0
        _Color    ("ValidColor", Color) = (0,0,0,1)
        _BgColor  ("ErrorColor", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque"
            "Queue" = "Transparent+1" 
             }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float4 _BgColor;
            float _ArcStart;
            float _ArcEnd;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.color = _Color;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float arcS = (_ArcStart * UNITY_PI) / 180;
                float arcE = (_ArcEnd * UNITY_PI) / 180;
                float newY = i.uv.y-0.5;
                float newX = i.uv.x-0.5;

                float arcP = atan(newY/newX);
                if (newX < 0) {
                    arcP += UNITY_PI;
                } else {
                    if (newY < 0) {
                        arcP += UNITY_TWO_PI;
                    }
                }

                fixed4  col = i.color;
                if (arcS < arcE && arcP > arcS && arcP < arcE){
                    col = _Color;
                }
                else{
                    col = _BgColor;
                }

                return col;
            }
            ENDCG
        }
    }
}