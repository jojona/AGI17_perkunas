// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// This shader is the one intended for cel shading/rendering silhouette lines

Shader "Custom/LineRenderer" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		LengthOfVertex("Length from vertex", Range(0,1)) = 0 //This is practically useless now.
		Scaling("Scaling",Range(0.0,1.2)) = 0.23
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		ZWrite On		//1a)
		Cull Back		//1a)
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_CBUFFER_START(Props)
	// put more per-instance properties here
	UNITY_INSTANCING_CBUFFER_END


		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
	ENDCG

		// Now render front faces
		ZTest LEqual 		//1c)
		Cull Front		//1c)
		CGPROGRAM
#include "UnityCG.cginc"
#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;

	float LengthOfVertex;
	struct Input {
		float2 uv_MainTex;
	};

	float Scaling;
	void vert(inout appdata_full v) {
		/*
		float2 cameraDir = normalize(UNITY_MATRIX_IT_MV[2].xz); //Push it away from our camera
		v.vertex.xz += cameraDir*LengthOfVertex;
		
		v.vertex.xyz += v.normal*LengthOfVertex;
		v.vertex.xyz *= float3(Scaling,Scaling,Scaling);
		

		*/

		//v.vertex.xyz += v.normal*LengthOfVertex; //We don't need this anymore since it destroys our lowpoly style. 

		float3 cameraDir = normalize(UNITY_MATRIX_IT_MV[3].xyz - v.vertex.xyz); //A normalized vector to our camera
		v.vertex.xyz += cameraDir*Scaling; //Scale it 

	}
	// Green front faces
	void surf(Input IN, inout SurfaceOutput o) {

		o.Albedo = fixed3(0, 0, 0);

	}
	ENDCG
	}
		FallBack "Diffuse"




}