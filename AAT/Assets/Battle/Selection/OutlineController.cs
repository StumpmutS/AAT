using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [Serializable]
    private class ShaderInfo
    {
        [SerializeField] private OutlineDefaults outlineDefaults;
        public OutlineDefaults OutlineDefaults => outlineDefaults;
        [SerializeField] private Color defaultColor;
        public Color DefaultColor => defaultColor;
        [SerializeField] private bool interactedColorChange;
        public bool InteractedColorChange => interactedColorChange;
        [SerializeField] private Shader baseShader;
        public Shader BaseShader => baseShader;
        [SerializeField] private Shader interactedShader;
        public Shader InteractedShader => interactedShader;
        [SerializeField] private int outlineMatIndex;
        public int OutlineMatIndex => outlineMatIndex;
    }
    
    [SerializeField] private List<ShaderInfo> shaderInfo;
    [SerializeField] private List<Renderer> renderers;
    
    public Color OutlineColor
    {
        set
        {
            foreach (var (mat, info) in _rendererInfo.Values.SelectMany(v => v))
            {
                if (!info.InteractedColorChange) continue;
                mat.color = value;
            }
        }
    }

    private Dictionary<Renderer, List<Tuple<Material, ShaderInfo>>> _rendererInfo = new();
    private static readonly int ZTest = Shader.PropertyToID("_ZTest");

    private void Awake()
    {
        foreach (var renderer in renderers)
        {
            foreach (var info in shaderInfo)
            {
                SetupMaterial(renderer, info);
            }
        }
    }

    private void SetupMaterial(Renderer renderer, ShaderInfo info)
    {
        if (renderer.materials.Length <= info.OutlineMatIndex) return;
        if (!_rendererInfo.ContainsKey(renderer)) _rendererInfo[renderer] = new List<Tuple<Material, ShaderInfo>>();

        var mat = Instantiate(renderer.materials[info.OutlineMatIndex]);
        mat.shader = info.BaseShader;
        _rendererInfo[renderer].Add(new Tuple<Material, ShaderInfo>(mat, info));
    }

    private void Start()
    {
        foreach (var renderer in renderers)
        {
            SetRendererMaterials(renderer);
        }
    }

    private void SetRendererMaterials(Renderer renderer)
    {
        var mats = renderer.materials;
        foreach (var (mat, info) in _rendererInfo[renderer])
        {
            mats[info.OutlineMatIndex] = mat;
        }
        renderer.materials = mats;
    }

    public void Activate()
    {
        foreach (var (mat, info) in _rendererInfo.Values.SelectMany(v => v))
        {
            mat.shader = info.InteractedShader;
            mat.SetFloat(ZTest, (float) info.OutlineDefaults.MaskZTest);
        }
    }

    public void Deactivate()
    {
        foreach (var (mat, info) in _rendererInfo.Values.SelectMany(v => v))
        {
            mat.shader = info.BaseShader;
            if (!info.InteractedColorChange) continue;
            mat.color = info.DefaultColor;
        }
    }
}
