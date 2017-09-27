Shader "Custom/ToonTerrainFirstPass" {
Properties {
 
    // Control Texture ("Splat Map")
    [HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
     
    // Terrain textures - each weighted according to the corresponding colour
    // channel in the control texture
    [HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
    [HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
    [HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
    [HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
     
    // Used in fallback on old cards & also for distant base map
    [HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
     
    // Let the user assign a lighting ramp to be used for toon lighting
    _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
     
    // Colour of toon outline
    _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    _Outline ("Outline width", Range (.002, 0.03)) = .005
}
     
SubShader {
    Tags {
        "SplatCount" = "4"
        "Queue" = "Geometry-100"
        "RenderType" = "Opaque"
    }
     
    // TERRAIN PASS 
    CGPROGRAM
    #pragma surface surf ToonRamp exclude_path:prepass vertex:vert
    // Access the Shaderlab properties
    uniform sampler2D _Control;
    uniform sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
    uniform sampler2D _Ramp;
 
    // Custom lighting model that uses a texture ramp based
    // on angle between light direction and normal
    inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
    {
        #ifndef USING_DIRECTIONAL_LIGHT
        lightDir = normalize(lightDir);
        #endif
        // Wrapped lighting
        half d = dot (s.Normal, lightDir) * 0.5 + 0.5;
        // Applied through ramp
        half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
        half4 c;
        c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
        c.a = 0;
        return c;
    }



    // Surface shader input structure
    struct Input {
        float2 uv_Control : TEXCOORD0;
        float4 vertex;
        float3 normal;
    };


    void vert(inout appdata_base v, out Input o) {
    	UNITY_INITIALIZE_OUTPUT(Input,o);
    	o.normal = abs(v.normal);
    	o.normal.x *= o.normal.x;
    	o.normal.y *= o.normal.y;
    	o.normal.z *= o.normal.z;
    	o.normal   /= o.normal.x + o.normal.y + o.normal.z;

    	o.vertex = v.vertex;
    }


    inline fixed3 sampleTriplanar(float4 vert, float3 norm, sampler2D _tex) {
		return norm.x * tex2D(_tex, float2(vert.y, vert.z)).rgb
			+  norm.y * tex2D(_tex, float2(vert.x, vert.z)).rgb
			+  norm.z * tex2D(_tex, float2(vert.x, vert.y)).rgb;
    }

   	inline fixed advancedBlend(fixed alpha, fixed3 col) {
   		//until further notice, use terrain brightness as a standin of alpha channel
   		//fixed alpha2 = sqrt((col.r*col.r + col.g*col.g + col.b*col.b)*0.333333)*0.7 + 0.3;//0.55 + 0.45;
   		//fixed alpha2 = (col.r > col.g ? (col.r > col.b ? col.g : col.b) : (col.g > col.b ? col.g : col.b) ) * 0.6 + 0.4;
   		fixed alpha2 = ((col.r + col.g + col.b)*0.333333)*0.5 + 0.5;
   		return tanh(80*(alpha2+0.05-(1-alpha)*1.1))*0.5 + 0.5;
   	}


    // Surface Shader function
    void surf (Input IN, inout SurfaceOutput o) {
        fixed4 splat_control = tex2D (_Control, IN.uv_Control);
        fixed3 col;
        fixed4 fixed_transp;//ensure sum of this vector is equal to sums of splat_control
        //col  = splat_control.r * tex2D (_Splat0, IN.uv_Splat0).rgb;
        //col += splat_control.g * tex2D (_Splat1, IN.uv_Splat0).rgb;
        //col += splat_control.b * tex2D (_Splat2, IN.uv_Splat0).rgb;
        //col += splat_control.a * tex2D (_Splat3, IN.uv_Splat0).rgb;

        //col  = splat_control.r * sampleTriplanar(IN.vertex, IN.normal, _Splat0);
        //col += splat_control.g * sampleTriplanar(IN.vertex, IN.normal, _Splat1);
        //col += splat_control.b * sampleTriplanar(IN.vertex, IN.normal, _Splat2);
        //col += splat_control.a * sampleTriplanar(IN.vertex, IN.normal, _Splat3);
        fixed3 tmp = sampleTriplanar(IN.vertex, IN.normal, _Splat0);
        fixed_transp.r = advancedBlend(splat_control.r, tmp);
        col = tmp*fixed_transp.r;

        tmp = sampleTriplanar(IN.vertex, IN.normal, _Splat1);
        fixed_transp.g = advancedBlend(splat_control.g, tmp);
        col += tmp*fixed_transp.g;

        tmp = sampleTriplanar(IN.vertex, IN.normal, _Splat2);
        fixed_transp.b = advancedBlend(splat_control.b, tmp);
        col += tmp*fixed_transp.b;

        tmp = sampleTriplanar(IN.vertex, IN.normal, _Splat3);
        fixed_transp.a = advancedBlend(splat_control.a, tmp);
        col += tmp*fixed_transp.a;

        o.Albedo = col * ((splat_control.r + splat_control.g + splat_control.b + splat_control.a) / (fixed_transp.r + fixed_transp.g + fixed_transp.b + fixed_transp.a));
        o.Alpha = 0.0;
    }
    ENDCG
 
    // Use the Outline Pass from the default Toon shader
    UsePass "Toon/Basic Outline/OUTLINE"
 
} // End SubShader
 
// Specify dependency shaders   
Dependency "AddPassShader" = "Shaders/ToonTerrainAddPass"
Dependency "BaseMapShader" = "Toon/Lit Outline"
 
// Fallback to Diffuse
Fallback "Diffuse"
 
} // Ehd Shader