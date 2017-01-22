 Shader "Custom/WavyGround" {
    Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		[Normal]_NormalTex("Normal Map", 2D) = "bump" {}
		_HolesMask("Holes Mask", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_MinDisp("Min Displacement", Float) = 0.1
		_MaxDisp("Max Displacement", Float) = 0.45
		_MinFreq("Min Frequency", Float) = 5
		_MaxFreq("Max Frequency", Float) = 10
	}
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 300
            
        CGPROGRAM
        #pragma surface surf Standard addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
        #pragma target 4.6

		#define CONE_MAX 30
		#define TESS_AMOUNT 12

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
			float2 uv_HolesMask;
		};

        float4 tessFixed()
        {
            return TESS_AMOUNT;
        }

		uniform float _Tess;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform sampler2D _NormalTex;
		uniform sampler2D _HolesMask;
		uniform float4 _HolesMask_ST;
		uniform float _Glossiness;
		uniform float _Metallic;

		uniform float _MinDisp;
		uniform float _MaxDisp;
		uniform float _MinFreq;
		uniform float _MaxFreq;

		#define WAVE_POINTS 32

		CBUFFER_START(SpawnPoints)
			float4 _SpawnParams[WAVE_POINTS]; // xy: pos zw: time / duration
			float _Radius[WAVE_POINTS];
			float4 _DirParams[WAVE_POINTS]; // xy: Direction z: Directional w: Cone width
		CBUFFER_END

		float GetMaskValue(float2 uvs) {
			return tex2Dlod(_HolesMask, float4(uvs, 0, 1));
		}

		float3 WaveDisplacement(float3 vertex, float2 uvs, uint i) {
			// Origin
			float3 origin = _SpawnParams[i].xyy; origin.y = 0;
			float3 wavePoint = vertex.xyz - origin;

			float mask = tex2Dlod(_HolesMask, float4(uvs, 0, 1)).x;

			// Time and radius
			float timeWindow = clamp((_Time.y - _SpawnParams[i].z) / _SpawnParams[i].w, 0, 1);
			float radius = lerp(0, _Radius[i], timeWindow);

			// Params
			float t = _DirParams[i].z * _DirParams[i].w / CONE_MAX;

			float displacement = lerp(_MaxDisp, _MinDisp, t);
			float frequency = lerp(_MinFreq, _MaxFreq, t);

			// Intensity
			float p = smoothstep(0, 1, length(wavePoint) < radius ? 1 : 0);
			float intensity = smoothstep(0, 1, length(wavePoint) / radius * p);

			// Directionality
			float3 waveDir = _DirParams[i].xyy; waveDir.y = 0;
			UNITY_BRANCH
				if (_DirParams[i].z) intensity *= smoothstep(0, 1, pow(max(0, dot(waveDir, normalize(wavePoint))), _DirParams[i].w));

			return abs((1 - timeWindow) * float3(0, mask, 0) * sin(length(wavePoint) * frequency) * displacement * intensity);
		}

		float3 DistordedPoint(float3 p, float2 uvs) {
			float3 finalDisp = 0;

			for (uint i = 0; i < WAVE_POINTS; i++)
			{
				float3 disp = WaveDisplacement(p, uvs, i);
				finalDisp = max(finalDisp, disp);
			}

			return p + finalDisp;
		}


		void disp(inout appdata_full v)
		{
			
			float3 finalNormal = 0;

			float2 maskUVs = TRANSFORM_TEX(v.texcoord, _HolesMask);

			v.vertex.xyz = DistordedPoint(v.vertex.xyz, maskUVs);

			float3 bitangent = cross(v.normal, v.tangent.xyz);
			float3 posX = DistordedPoint(v.vertex.xyz + v.tangent.xyz, maskUVs);
			float3 posY = DistordedPoint(v.vertex.xyz + bitangent, maskUVs);

			float3 newTangent = (posX - v.vertex.xyz); // leaves just 'tangent'
			float3 newBitangent = (posY - v.vertex.xyz); // leaves just 'bitangent'

			v.normal = cross(newTangent, newBitangent);
		}

        void surf (Input IN, inout SurfaceOutputStandard o) {
			clip(tex2D(_HolesMask, IN.uv_MainTex) - 0.05);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}