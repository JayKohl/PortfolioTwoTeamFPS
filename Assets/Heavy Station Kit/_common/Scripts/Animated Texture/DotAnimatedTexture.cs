// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{

    // Sequence item Class
    [System.Serializable]
    public class dotATSequence : System.Object
    {
        public int totalFrames = 0;
        public int firstFrame = 0;
        public int startingFrame = 0;
        public bool randomly = false;
    }

    [ExecuteInEditMode]
    public class DotAnimatedTexture : MonoBehaviour
    {
#if UNITY_EDITOR
        [ReadOnlyAttribute]
#endif
        public int materialTotalFrames;
        public int activeSequence = 0;
        public List<dotATSequence> sequences;
        public float FPS = 10;
        public bool showWarnings = false;
        // Private properties
        private Renderer _renderer = null;
        private int materialTileRows = 0;
        private int materialTileCols = 0;
#if UNITY_EDITOR
        private int prevWarning = 0;
#endif
        private int prevIndex = -1;

        void Start()
        {
            Init();
        }

        void Init()
        {
            if (_renderer == null) { _renderer = GetComponent<Renderer>(); }
            Material rendererMaterial = (_renderer == null) ? null : _renderer.sharedMaterial;
            if (rendererMaterial == null)
            {
                materialTileRows = materialTileCols = 0;
            }
            else
            {
                Vector2 _size = rendererMaterial.mainTextureScale;
                materialTileRows = (_size.y > 0) ? Mathf.RoundToInt(1.0f / _size.y) : 1;
                materialTileCols = (_size.x > 0) ? Mathf.RoundToInt(1.0f / _size.x) : 1;
            }
#if UNITY_EDITOR
            if (materialTileRows * materialTileCols == 0) { ShowWarning(1, "Material was not set or its 'Tile' value is incorrect"); }
#endif
        }

        void Update()
        {
            ForceUpdate(false);
        }

        public void ForceUpdate(bool force = true)
        {
            bool isp = Application.isPlaying;
            if (!isp) { Init(); }
            // Validate frame grid size
            materialTotalFrames = materialTileRows * materialTileCols;
            if (materialTotalFrames == 0)
            {
#if UNITY_EDITOR
                if (!force) { ShowWarning(1, "Incorrect parameters 'Row Count' or 'Col Count'"); }
#endif
                return;
            }
            // Validate active sequence
            if ((sequences == null) || (activeSequence + 1 > sequences.Count))
            {
#if UNITY_EDITOR
                ShowWarning(2, (sequences == null) ? "Sequences was not set" : "Incorrect parameter 'Active Sequence'");
#endif
                return;
            }
            // Init parameters
            var totalFrames = sequences[activeSequence].totalFrames;
            var startingFrame = sequences[activeSequence].startingFrame;
            var firstFrame = sequences[activeSequence].firstFrame;
            var randomly = sequences[activeSequence].randomly;
            // Validate active sequence parameters
            if ((totalFrames == 0) || (totalFrames + firstFrame > materialTileCols * materialTileRows))
            {
#if UNITY_EDITOR
                ShowWarning(3, "Incorrect parameter 'Total Frames' or/and 'Base Frame' for sequence #" + activeSequence);
#endif
                return;
            }
            if (_renderer == null)
            {
#if UNITY_EDITOR
                ShowWarning(4, "Renderer component was not found");
#endif
            }
            else
            {
                // Size of frame
                Vector2 size = new Vector2(1.0f / materialTileCols, 1.0f / materialTileRows);
                // Calculate newIndex
                int newIndex;
                if (isp)
                {
                    newIndex = ((int)(Time.time * FPS) + startingFrame) % totalFrames;
                }
                else
                {
                    newIndex = startingFrame % totalFrames;
                    Vector2 _size = _renderer.sharedMaterial.mainTextureScale;
                    int _total = 0;
                    if (_size.x * _size.y != 0)
                    {
                        _total = Mathf.RoundToInt(1.0f / (_size.x * _size.y));
                    };
#if UNITY_EDITOR
                    if (_total == 0) { ShowWarning(5, "Material was not set or its 'Tile' value is incorrect"); }
#endif
                    materialTotalFrames = _total;
                }
                if (firstFrame + newIndex != prevIndex)
                {
                    prevIndex = firstFrame + newIndex;
                    if (randomly && isp) { newIndex = Mathf.RoundToInt(Random.value * (totalFrames - 1)); }
                    newIndex += firstFrame;
                    // Current offset
                    Vector2 offset = new Vector2((newIndex % materialTileCols) * size.x, (1.0f - size.y) - ((int)(newIndex / materialTileCols)) * size.y);
                    // Set offset and size for texture
                    if (isp)
                    {
                        _renderer.material.mainTextureOffset = offset;
                    }
                    else
                    {
#if UNITY_EDITOR
                        Material rendererMaterial = Material.Instantiate(_renderer.sharedMaterial);
                        rendererMaterial.name = _renderer.sharedMaterial.name;
                        rendererMaterial.mainTextureOffset = offset;
                        _renderer.material = rendererMaterial;
#endif
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void ShowWarning(int warn_id, string msg)
        {
            if (showWarnings && (prevWarning != warn_id))
            {
                Debug.LogWarning("DotAnimatedTexture (" + transform.name + "): " + msg);
                prevWarning = warn_id;
            }
        }
#endif

    }

}
