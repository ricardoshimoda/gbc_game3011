Shader "Custom/ArcSurface"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _ArcStart ("ValidStart",Range(0,360)) = 0.0
        _ArcEnd   ("ValidEnd",  Range(0,360)) = 0.0
        _Color ("Color", Color) = (1,1,1,1)
        _BgColor  ("ErrorColor", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"
         "Queue" = "Transparent+1" 
          }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _BgColor;
        float _ArcStart;
        float _ArcEnd;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 bc = tex2D (_MainTex, IN.uv_MainTex) * _BgColor;
            float arcS = (_ArcStart * UNITY_PI) / 180;
            float arcE = (_ArcEnd * UNITY_PI) / 180;

            float coordx = IN.uv_MainTex.x - 0.5;
            float coordy = IN.uv_MainTex.y - 0.5;

            float arcP = atan(coordy/coordx);
            if (coordx < 0) {
                arcP += UNITY_PI;
            } else {
                if (coordy < 0) {
                    arcP += UNITY_TWO_PI;
                }
            }
            if (arcS < arcE && arcP > arcS && arcP < arcE){
                o.Albedo = _Color.rgb;
            }
            else {
                o.Albedo = _BgColor.rgb;
            }
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
