// This shader is the one intended for cel shading/rendering silhouette lines

Shader "Unlit/LineRenderer"
{
    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		LengthOfVertex("Length from vertex", Range(0,1)) = 0.3
		ContourColor("Color of contour", Color) = (0,0,0,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;


            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG

			ZWrite On		//1a)
			Cull Back		//1a)
        }


		//Probably need another pass
		
				Pass
			{
				CGPROGRAM
				// use "vert" function as the vertex shader
				#pragma vertex vert
				// use "frag" function as the pixel (fragment) shader
				#pragma fragment frag

				// vertex shader inputs
				struct appdata
			{
				float3 normal : NORMAL;		//Normal 
				float4 vertex : POSITION; // vertex position
				float2 uv : TEXCOORD0; // texture coordinate
			};

			// vertex shader outputs ("vertex to fragment")
			struct v2f
			{
				float2 uv : TEXCOORD0; // texture coordinate
				float4 vertex : SV_POSITION; // clip space position
			};

			float LengthOfVertex;
			// vertex shader
			v2f vert(appdata v)
			{
				v2f o;
				// transform position to clip space
				// (multiply with model*view*projection matrix)
				o.vertex = UnityObjectToClipPos(v.vertex);
				// just pass the texture coordinate
				o.uv = v.uv;
				//o.vertex.xyz += v.normal * LengthOfVertex;
				return o;
			}

			// texture we will sample
			sampler2D _MainTex;

			// pixel shader; returns low precision ("fixed4" type)
			// color ("SV_Target" semantic)
			fixed4 frag(v2f i) : SV_Target
			{
				// sample texture and return it
				fixed4 col = tex2D(_MainTex, i.uv);
			return col;
			}
				ENDCG

				ZWrite On		//1a)
				Cull Back		//1a)
			}

			Pass
			{
				CGPROGRAM
				// use "vert" function as the vertex shader
				#pragma vertex vert
				// use "frag" function as the pixel (fragment) shader
				#pragma fragment frag

				// vertex shader inputs
				struct appdata
			{
				float3 normal : NORMAL;		//Normal 
				float4 vertex : POSITION; // vertex position
				float2 uv : TEXCOORD0; // texture coordinate
			};

			// vertex shader outputs ("vertex to fragment")
			struct v2f
			{
				float2 uv : TEXCOORD0; // texture coordinate
				float4 vertex : SV_POSITION; // clip space position
			};

			float LengthOfVertex;
			// vertex shader
			v2f vert(appdata v)
			{
				v2f o;
				// transform position to clip space
				// (multiply with model*view*projection matrix)
				o.vertex = UnityObjectToClipPos(v.vertex);
				// just pass the texture coordinate
				o.uv = v.uv;
				o.vertex.xyz += v.normal * LengthOfVertex;
				return o;
			}

			//Contourcolor
			fixed4 ContourColor;

			// pixel shader; returns low precision ("fixed4" type)
			// color ("SV_Target" semantic)
			fixed4 frag(v2f i) : SV_Target
			{
				//Return the color of the contour
				return ContourColor;
			}
				ENDCG

				ZTest LEqual 		//1a)
				Cull Front		//1a)
			}
    }
}