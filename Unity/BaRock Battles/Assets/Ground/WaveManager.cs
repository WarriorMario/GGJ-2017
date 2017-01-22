using UnityEngine;

public class WaveManager : MonoBehaviour {

    private const int WAVE_POINTS = 32;

    private struct WavePoint
    {
        public Vector4 posTime;
        public float radius;
        public Vector4 dir;
        public WavePoint(Vector2 p, float t, float d, float r, Vector2 dir, bool directional, float c)
        {
            this.posTime = new Vector4(p.x, p.y, t, d);
            this.radius = r;
            this.dir = new Vector4(dir.x, dir.y, directional ? 1 : 0, c);
        }
    }

    private Material _mat;
    private Camera _camera;

    private WavePoint[] _points = new WavePoint[WAVE_POINTS];
    private uint _pointsId = 0;

    private Vector4[] _spawnArray = new Vector4[WAVE_POINTS];
    private float[] _radiusArray = new float[WAVE_POINTS];
    private Vector4[] _dirArray = new Vector4[WAVE_POINTS];

    // Use this for initialization
    void Start () {
        _mat = GetComponent<MeshRenderer>().sharedMaterial;
        _camera = FindObjectOfType<Camera>();
    }

    public void AddWave(Vector3 position, float duration, float radius, bool directional, Vector3 direction, float cone)
    {
        Vector3 worldPoint = position;
        Vector3 localPoint = transform.worldToLocalMatrix * worldPoint;
        Vector2 shaderPoint = new Vector2(localPoint.x, localPoint.z);

        float shaderTime = Time.time;
        float shaderDuration = duration;
        float shaderRadius = radius;

        Vector2 shaderDir = new Vector2(direction.x, direction.z);

        _points[_pointsId] = new WavePoint(shaderPoint, shaderTime, shaderDuration, shaderRadius, shaderDir, directional, cone);

        _spawnArray[_pointsId] = _points[_pointsId].posTime;
        _radiusArray[_pointsId] = _points[_pointsId].radius;
        _dirArray[_pointsId] = _points[_pointsId].dir;

        _pointsId = (_pointsId + 1) % WAVE_POINTS;

        _mat.SetVectorArray("_SpawnParams", _spawnArray);
        _mat.SetFloatArray("_Radius", _radiusArray);
        _mat.SetVectorArray("_DirParams", _dirArray);
    }
	
	// Debug
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                Vector3 worldPoint = hit.point;
                Vector3 localPoint = hit.transform.worldToLocalMatrix * worldPoint;
                Vector2 shaderPoint = new Vector2(localPoint.x, localPoint.z);

                float shaderTime = Time.time;
                float shaderDuration = 4.0f;
                float shaderRadius = 10.0f;

                Vector2 shaderDir = new Vector2(1, 0);
                bool directional = false;

                float cone = 10;

                _points[_pointsId] = new WavePoint(shaderPoint, shaderTime, shaderDuration, shaderRadius, shaderDir, directional, cone);
                
                _spawnArray[_pointsId] = _points[_pointsId].posTime;
                _radiusArray[_pointsId] = _points[_pointsId].radius;
                _dirArray[_pointsId] = _points[_pointsId].dir;

                _pointsId = (_pointsId + 1) % WAVE_POINTS;

                _mat.SetVectorArray("_SpawnParams", _spawnArray);
                _mat.SetFloatArray("_Radius", _radiusArray);
                _mat.SetVectorArray("_DirParams", _dirArray);
            }
        }
    }
}
