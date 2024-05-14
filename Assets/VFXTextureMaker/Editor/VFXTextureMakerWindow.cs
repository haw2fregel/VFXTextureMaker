using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace VFXTextureMaker
{
    public class VFXTextureMakerWindow : EditorWindow
    {
        TextureDataEditor _textureDataEditor;
        [SerializeField] ComputeShader _cs;
        [SerializeField] ComputeShader _textureSeetCs;
        [SerializeField] ComputeShader _copyRtCs;
        [SerializeField] Material _previewMaterial;
        Material _previewMat;
        [SerializeField] Texture2D _previewBg;

        float _borderHeight;
        float _borderWidth;

        Vector2 _currentScrollPosition;
        Vector2 _currentScrollPosition2;

        readonly string[] PreviewTarget = { "Result", "Select Layer" };
        readonly string[] PreviewChannel = { "RGBA", "R", "G", "B", "A" };
        int _previewTarget = 0;
        int _previewChannel = 0;
        bool _textureDataOption;
        bool _animationOption;

        [MenuItem("Tools/VFXTextureMaker")]
        public static void ShowWindow()
        {
            var window = GetWindow<VFXTextureMakerWindow>();
            window.titleContent = new GUIContent("VFXTextureMaker");
            window.Show();
        }
        public void OnEnable()
        {
            minSize = new Vector2(300, 500);
            _previewMat = Instantiate(_previewMaterial);

            _borderHeight = position.size.y * 0.40f;

            _textureDataEditor = CreateInstance<TextureDataEditor>();
            _textureDataEditor.OnChanged += Changed;

            Undo.undoRedoPerformed += () =>
            {
                _textureDataEditor.InitLayerList();
                _textureDataEditor.Blit(_cs);
                Repaint();
            };
        }
        public void OnDisable()
        {
            _textureDataEditor.OnDisable();
            DestroyImmediate(_textureDataEditor);
        }

        void OnFocus()
        {
            if (TextureDataEditor.TextureData == null) return;
            Changed();
        }
        void Changed()
        {
            _textureDataEditor.Blit(_cs);
            Repaint();
        }
        void FileDropdown(Rect buttonRect)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("New File"), on: false, func: () =>
            {
                var path = EditorUtility.SaveFilePanelInProject("Save TextureData", "TextureData", "asset", "", "Assets");
                if (string.IsNullOrWhiteSpace(path)) return;

                var pathTex = EditorUtility.SaveFilePanelInProject("Save Texture", "Texture", "png", "", "Assets");
                if (string.IsNullOrWhiteSpace(pathTex)) return;

                var newData = CreateInstance<TextureData>();

                var tex = new Texture2D(1, 1);
                byte[] pngData = tex.EncodeToPNG();
                File.WriteAllBytes(pathTex, pngData);
                DestroyImmediate(tex);

                AssetDatabase.CreateAsset(newData, path);
                AssetDatabase.Refresh();

                newData = null;

                _textureDataEditor.InitLayerList(AssetDatabase.LoadAssetAtPath(path, typeof(TextureData)) as TextureData);
                _textureDataEditor.TargetTexture = AssetDatabase.LoadAssetAtPath(pathTex, typeof(Texture2D)) as Texture2D;
                _textureDataEditor.Blit(_cs);
            });
            menu.AddItem(new GUIContent("Load"), on: false, func: () =>
            {
                string path = EditorUtility.OpenFilePanel("Load Texture Data", "Assets", "asset");
                if (path.Length == 0) return;

                path = path.Replace("\\", "/").Replace(Application.dataPath, "Assets");

                _textureDataEditor.InitLayerList(AssetDatabase.LoadAssetAtPath(path, typeof(TextureData)) as TextureData);
                _textureDataEditor.Blit(_cs);
            });
            menu.DropDown(buttonRect);
        }
        public void OnGUI()
        {
            var ev = Event.current;

            var rect = position;
            _borderHeight = Mathf.Clamp(_borderHeight, 150, position.height - 50);
            _borderWidth = Mathf.Clamp(_borderWidth, 150, position.width - 150);
            rect.y = 0;
            rect.x = 0;

            var rectToolbar = rect;
            rectToolbar.height = 20;
            GUI.Box(rectToolbar, new GUIContent(" "), EditorStyles.toolbar);

            var rectFile = rectToolbar;
            rectFile.width = 50;
            if (GUI.Button(rectFile, new GUIContent("File"), EditorStyles.toolbarButton))
            {
                FileDropdown(rectFile);
                return;
            }

            if (TextureDataEditor.TextureData == null) return;

            var rectConvert = rectFile;
            rectConvert.x += 50;
            rectConvert.width = 60;
            if (GUI.Button(rectConvert, new GUIContent("Render"), EditorStyles.toolbarButton))
            {
                if (_textureDataEditor.Animated)
                {
                    _textureDataEditor.AnimBlit(_cs, _textureSeetCs, true);
                }
                else
                {
                    _textureDataEditor.Blit(_cs, true);
                }
            }

            var rectOption = rectConvert;
            rectOption.xMax = position.width;
            rectOption.xMin = position.width - 60;
            if (GUI.Button(rectOption, new GUIContent("Setting"), EditorStyles.toolbarButton))
            {
                _textureDataOption = !_textureDataOption;
                if (_textureDataOption) _animationOption = false;
            }

            var rectAnimOption = rectOption;
            rectAnimOption.xMax = position.width - 60;
            rectAnimOption.xMin = position.width - 100;
            if (GUI.Button(rectAnimOption, new GUIContent("Anim"), EditorStyles.toolbarButton))
            {
                _animationOption = !_animationOption;
                if (_animationOption) _textureDataOption = false;
            }

            rect.y += 21;

            var rectDataList = rect;
            rectDataList.height = 20;

            EditorGUI.LabelField(rectDataList, _textureDataEditor.Name, EditorStyles.toolbarButton);

            if (_textureDataOption)
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    rect.y += 20;
                    var rectSo = rect;
                    rectSo.height = 20;
                    rectSo.xMax = 200;
                    EditorGUI.ObjectField(rectSo, TextureDataEditor.TextureData, typeof(ScriptableObject), false);
                    rectSo.height = 50;
                    rectSo.xMax = position.width;
                    rectSo.xMin = position.width - 50;
                    EditorGUI.ObjectField(rectSo, _textureDataEditor.TargetTexture, typeof(Texture2D), false);
                    rect.y += 50;
                    _textureDataEditor.TextureSize = EditorGUI.Vector2IntField(rect, new GUIContent("Resolution"), _textureDataEditor.TextureSize);
                    rect.y += 25;
                    if (check.changed)
                    {
                        Changed();
                        return;
                    }
                }
            }
            if (_animationOption)
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    rect.y += 20;
                    var rectAnimated = rect;
                    rectAnimated.height = 20;
                    _textureDataEditor.Animated = EditorGUI.Toggle(rectAnimated, new GUIContent("Animated"), _textureDataEditor.Animated);
                    if (_textureDataEditor.Animated)
                    {
                        var rectAnimLength = rectAnimated;
                        rectAnimLength.y += 20;
                        _textureDataEditor.AnimLength = EditorGUI.IntField(rectAnimLength, new GUIContent("Anim Length"), _textureDataEditor.AnimLength);
                        _textureDataEditor.AnimLength = _textureDataEditor.AnimLength <= 0 ? 1 : _textureDataEditor.AnimLength;
                        var rectCurrentFrame = rectAnimLength;
                        rectCurrentFrame.y += 20;
                        _textureDataEditor.CurrentFrame = EditorGUI.IntSlider(rectCurrentFrame, new GUIContent("Current Frame"), _textureDataEditor.CurrentFrame, 0, _textureDataEditor.AnimLength - 1);
                        _textureDataEditor.CurrentFrame = _textureDataEditor.CurrentFrame >= _textureDataEditor.AnimLength ? _textureDataEditor.AnimLength - 1 : _textureDataEditor.CurrentFrame;
                        rect.y += 40;
                    }
                    if (check.changed)
                    {
                        Changed();
                        return;
                    }
                }

            }

            rect.y += 20;
            var rectTopBorder = rect;
            rectTopBorder.yMax = rectTopBorder.yMin + 1;
            EditorGUI.DrawRect(rectTopBorder, new Color(0, 0, 0, 0.7f));

            rect.y += 1;

            var rectList = rect;
            rectList.yMin = rect.y;
            rectList.yMax = _borderHeight;
            rectList.xMax = _borderWidth;
            EditorGUI.DrawRect(rectList, new Color(0, 0, 0, 0.15f));
            var rectListView = rectList;
            rectListView.xMax = rectList.xMax;
            rectListView.yMax = rectList.yMin + _textureDataEditor.GetListHeight();
            if (rectListView.yMax > rectList.yMax) rectListView.xMax -= 13;
            using (var scrollScope = new GUI.ScrollViewScope(rectList, _currentScrollPosition, rectListView))
            {
                _currentScrollPosition = scrollScope.scrollPosition;
                _textureDataEditor.DoList(rectListView);
            }
            var rectContentBorder = rectList;
            rectContentBorder.xMin = _borderWidth;
            rectContentBorder.xMax = _borderWidth + 2;
            EditorGUI.DrawRect(rectContentBorder, new Color(0, 0, 0, 0.7f));
            rectContentBorder.xMin -= 2;
            rectContentBorder.xMax += 5;
            int sideid = EditorGUIUtility.GetControlID(FocusType.Passive);

            if (ev.button == 0)
            {
                switch (ev.type)
                {
                    case EventType.MouseDown:
                        if (rectContentBorder.Contains(ev.mousePosition))
                        {
                            EditorGUIUtility.hotControl = sideid;
                        }
                        break;

                    case EventType.MouseDrag:
                        if (EditorGUIUtility.hotControl == sideid)
                        {
                            _borderWidth = Mathf.Clamp(ev.mousePosition.x, 130, position.width - 150f);
                            Repaint();
                        }
                        break;
                    case EventType.MouseUp:
                        if (EditorGUIUtility.hotControl == sideid)
                        {
                            EditorGUIUtility.hotControl = 0;
                            Repaint();
                        }
                        break;
                }
            }
            EditorGUIUtility.AddCursorRect(rectContentBorder, MouseCursor.SplitResizeLeftRight);

            var rectLayer = rectList;
            rectLayer.xMin = _borderWidth + 2;
            rectLayer.xMax = position.width;
            EditorGUI.DrawRect(rectLayer, new Color(0, 0, 0, 0.25f));
            rectLayer.xMin += 5;
            rectLayer.xMax -= 5;
            var rectLayerView = rectLayer;
            rectLayerView.yMax = rectLayerView.yMin + _textureDataEditor.GetLayerHeight();
            if (rectLayerView.yMax > rectLayer.yMax) rectLayerView.xMax -= 13;
            using (var scrollScope = new GUI.ScrollViewScope(rectLayer, _currentScrollPosition2, rectLayerView))
            {
                _currentScrollPosition2 = scrollScope.scrollPosition;
                _textureDataEditor.DrawLayerProperty(rectLayerView);
            }

            var rectBorder = rect;
            rectBorder.y = _borderHeight;
            rectBorder.height = 3;
            EditorGUI.DrawRect(rectBorder, new Color(0, 0, 0, 0.7f));
            rectBorder.yMin -= 5;
            rectBorder.yMax += 2;
            int topid = EditorGUIUtility.GetControlID(FocusType.Passive);

            if (ev.button == 0)
            {
                switch (ev.type)
                {
                    case EventType.MouseDown:
                        if (rectBorder.Contains(ev.mousePosition))
                        {
                            EditorGUIUtility.hotControl = topid;
                        }
                        break;

                    case EventType.MouseDrag:
                        if (EditorGUIUtility.hotControl == topid)
                        {
                            _borderHeight = Mathf.Clamp(ev.mousePosition.y, position.height * 0.2f, position.height - 50f);
                            Repaint();
                        }
                        break;
                    case EventType.MouseUp:
                        if (EditorGUIUtility.hotControl == topid)
                        {
                            EditorGUIUtility.hotControl = 0;
                            Repaint();
                        }
                        break;
                }
            }
            EditorGUIUtility.AddCursorRect(rectBorder, MouseCursor.SplitResizeUpDown);

            if (_textureDataEditor.ListCount == 0) return;

            var rectPreviewTexType = rect;
            rectPreviewTexType.yMin = _borderHeight + 3;
            rectPreviewTexType.yMax = _borderHeight + 23;
            rectPreviewTexType.xMax = 90;
            rectPreviewTexType.xMin = 0;

            var rectPreviewChannel = rectPreviewTexType;
            rectPreviewChannel.xMax = position.width;
            rectPreviewChannel.xMin = position.width - 60;

            _previewTarget = EditorGUI.Popup(rectPreviewTexType, "", _previewTarget, PreviewTarget, EditorStyles.toolbarButton);
            _previewChannel = EditorGUI.Popup(rectPreviewChannel, "", _previewChannel, PreviewChannel, EditorStyles.toolbarButton);

            var rectPreviewTexture = rect;
            rectPreviewTexture.yMin = _borderHeight + 23;
            rectPreviewTexture.yMax = position.height;
            rectPreviewTexture.xMin = position.width / 2 - rectPreviewTexture.height / 2;
            rectPreviewTexture.xMax = position.width / 2 + rectPreviewTexture.height / 2;
            if (rectPreviewTexture.width > position.width)
            {
                var center = rectPreviewTexture.center;
                var size = position.width;
                rectPreviewTexture.yMin = center.y - size / 2;
                rectPreviewTexture.yMax = center.y + size / 2;
                rectPreviewTexture.xMin = center.x - size / 2;
                rectPreviewTexture.xMax = center.x + size / 2;
            }
            EditorGUI.DrawPreviewTexture(rectPreviewTexture, _previewBg, _previewMaterial, ScaleMode.StretchToFill, 0, -1, ColorWriteMask.All, 0);
            Texture previewTex;
            switch (_previewTarget)
            {
                case 0:
                    previewTex = TextureDataEditor.TextureData.PreviewTexture;
                    break;
                case 1:
                    previewTex = _textureDataEditor.SingleTexture;
                    break;
                default:
                    previewTex = _textureDataEditor.SingleTexture;
                    break;
            }
            if (!previewTex) previewTex = Texture2D.blackTexture;

            switch (_previewChannel)
            {
                case 0:
                    _previewMat.SetVector("_DrawChannel", new Vector4(1, 1, 1, 1));
                    EditorGUI.DrawPreviewTexture(rectPreviewTexture, previewTex, _previewMat, ScaleMode.StretchToFill, 0, -1, ColorWriteMask.All, 0);
                    break;
                case 1:
                    _previewMat.SetVector("_DrawChannel", new Vector4(1, 0, 0, 0));
                    EditorGUI.DrawPreviewTexture(rectPreviewTexture, previewTex, _previewMat, ScaleMode.StretchToFill, 0, -1, ColorWriteMask.All, 0);
                    break;
                case 2:
                    _previewMat.SetVector("_DrawChannel", new Vector4(0, 1, 0, 0));
                    EditorGUI.DrawPreviewTexture(rectPreviewTexture, previewTex, _previewMat, ScaleMode.StretchToFill, 0, -1, ColorWriteMask.All, 0);
                    break;
                case 3:
                    _previewMat.SetVector("_DrawChannel", new Vector4(0, 0, 1, 0));
                    EditorGUI.DrawPreviewTexture(rectPreviewTexture, previewTex, _previewMat, ScaleMode.StretchToFill, 0, -1, ColorWriteMask.All, 0);
                    break;
                case 4:
                    EditorGUI.DrawTextureAlpha(rectPreviewTexture, previewTex, ScaleMode.StretchToFill, 0, -1);
                    break;
            }
        }
    }
}