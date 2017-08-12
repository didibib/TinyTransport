Shader "Custom/BlowUp" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_Amount("Extrusion Amount", Range(0,1)) = 0.0	
	}

	SubShader{
		Tags
		{ 
			"Queue" = "Geometry"
			"RenderType" = "Opaque" 
		}

		CGINCLUDE
		#define _GLOSSYENV 1
		ENDCG

		CGPROGRAM
		#pragma target 3.0
		#include "UnityPBSLighting.cginc"
		#pragma surface surf Standard vertex:vert

		struct Input {
			float2 uv_MainTex;
		};

		float _Amount;

		void vert(inout appdata_full v) {
			v.vertex.xyz += v.normal *_Amount;
		}
		
		sampler2D _MainTex;
		sampler2D _Metallic;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 maintex = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = maintex.rgb;
			o.Occlusion = maintex.g;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
