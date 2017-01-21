 Shader "Custom/WavyGround" {
    Properties {
		_Tess("Tessellation", Range(1,32)) = 4
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		[Normal]_NormalTex("Normal Map", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		[MaterialToggle]_Directional("Directional", Float) = 0
		_Cone("Cone Width", Range(1, 30)) = 1
		_Direction("Direction", Vector) = (1, 0, 0, 0)
	}
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 300
            
        CGPROGRAM
        #pragma surface surf Standard addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
        #pragma target 4.6

		#define MIN_DISP 0.1
		#define MAX_DISP 0.45
		#define MIN_FREQ 1
		#define MAX_FREQ 10
		#define MIN_RAD 2
		#define MAX_RAD 4
		#define MIN_SPEED 10
		#define MAX_SPEED 30
		#define CONE_MAX 30
		#define TESS_AMOUNT 12

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
		};

        float4 tessFixed()
        {
            return TESS_AMOUNT;
        }

		uniform float _Tess;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform sampler2D _NormalTex;
		uniform float _Glossiness;
		uniform float _Metallic;
		uniform int _Directional;
		uniform float _Cone;
		uniform float3 _Direction;

		float3 WaveDisplacement(float3 vertex) {
			float3 wavePoint = vertex.xyz;

			float t = _Directional * _Cone / CONE_MAX;
			float displacement = lerp(MAX_DISP, MIN_DISP, t);
			float frequency = lerp(MIN_FREQ, MAX_FREQ, t);
			float radius = lerp(MIN_RAD, MAX_RAD, t);
			float speed = lerp(MIN_SPEED, MAX_SPEED, t);

			float intensity = 1 - smoothstep(0, 1, length(wavePoint) / radius - 0.5);
			UNITY_BRANCH
				if (_Directional) intensity *= smoothstep(0, 1, pow(max(0, dot(normalize(_Direction), normalize(wavePoint))), _Cone));

			return vertex + float3(0, 1, 0) * sin(length(wavePoint) * frequency - _Time.y * speed) * displacement * intensity;
		}

		void disp(inout appdata_full v)
		{
			float3 bitangent = cross(v.normal, v.tangent.xyz);

			float3 pos = WaveDisplacement(v.vertex.xyz);

			float3 posX = WaveDisplacement(v.vertex.xyz + v.tangent.xyz * 0.01);
			float3 posY = WaveDisplacement(v.vertex.xyz + bitangent * 0.01);

			float3 newTangent = (posX - pos); // leaves just 'tangent'
			float3 newBitangent = (posY - pos); // leaves just 'bitangent'

			v.vertex.xyz = pos;
			v.normal = cross(newTangent, newBitangent);
		}

        void surf (Input IN, inout SurfaceOutputStandard o) {
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