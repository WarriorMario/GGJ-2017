Shader "Sound Wave" {
	Properties{
		_Tess("Tessellation", Range(1,32)) = 4
		_MainTex("Base (RGB)", 2D) = "white" {}
	_NormalMap("Normalmap", 2D) = "bump" {}
	_Cones("Test", 2D) = "bump" {}
	_Displacement("Displacement", Range(0, 1.0)) = 0.3
		_Color("Color", color) = (1,1,1,0)
		_SpecColor("Spec color", color) = (0.5,0.5,0.5,0.5)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 300

		CGPROGRAM
#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
#pragma target 4.6

		struct appdata {
		float4 vertex : POSITION;
		float4 tangent : TANGENT;
		float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;
	};

	float _Tess;

	float4 tessFixed()
	{
		return _Tess;
	}
	float _Displacement;


#define MAXNUM 100
#define CONESIZE 0.9f
	uniform int _Cones_Length = 0;
	uniform float4 _Cones[100];
	uniform float4 _ConesMore[100];
	void disp(inout appdata v)
	{
		for (int i = 0; i < _Cones_Length; ++i)
		{
			float xCasted = i;
			float2 ss = float2(xCasted / 100.0f, 0.0f);
			float4 data = _Cones[i];
			float4 moreData = _ConesMore[i];
			float2 conePos = data.xy;
			float2 coneDir;
			coneDir.x = cos(data.z);
			coneDir.y = sin(data.z);

			float2 p = float2(v.texcoord.xy - conePos);
			float distance = length(p);
			if (distance < moreData.x&& distance > moreData.x*CONESIZE)
			{
				float2 nDir = normalize(p);
				float s = dot(nDir, coneDir);
				float coneAngle = data.w;
				if (s > coneAngle)
				{
					v.vertex.xyz += v.normal * 1.0f;
				}
			}
		}
	}

	struct Input {
		float2 uv_MainTex;
	};

	sampler2D _MainTex;
	sampler2D _NormalMap;
	fixed4 _Color;

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Specular = 0.2;
		o.Gloss = 1.0;
		o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
	}
	ENDCG
	}
		FallBack "Diffuse"
}