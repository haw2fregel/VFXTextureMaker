using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace VFXTextureMaker
{
    public class TextureDataEditor : Editor
    {
        static TextureData _textureData;
        public static TextureData TextureData => _textureData;
        public static string TextureDataPath => AssetDatabase.GetAssetPath(_textureData);
        
        [SerializeField] GUISkin _guiSkin;
        [SerializeField] Texture2D _underArrow;

        public event Action OnChanged = delegate { };
        
        ReorderableList _reorderableList;
        SerializedObject _so;
        SerializedProperty _layerListProp;
        int _selectedIndex;
        public int ListCount => _layerListProp.arraySize;
        public int SelectedIndex
        {
            get
            {
                if (_layerListProp.arraySize == 0) return -1;
                if (_layerListProp.arraySize <= _selectedIndex)
                {
                    _selectedIndex = _layerListProp.arraySize - 1;
                }
                return _selectedIndex;
            }
        }

        public string Name => _textureData.name;
        public Texture2D TargetTexture { get => _textureData.TargetTexture; set => _textureData.TargetTexture = value; }

        public bool GetVisibleAtIndex(int index)
        {
            var layerProp = _layerListProp.GetArrayElementAtIndex(index);
            return layerProp.FindPropertyRelative("_visible").boolValue;
        }

        public bool GetMaskedAtIndex(int index)
        {
            var layerProp = _layerListProp.GetArrayElementAtIndex(index);
            return layerProp.FindPropertyRelative("_masked").boolValue;
        }

        public int GetMaskedLayerAtIndex(int index)
        {
            var layerProp = _layerListProp.GetArrayElementAtIndex(index);
            return layerProp.FindPropertyRelative("_maskedLayer").intValue;
        }

        public bool Animated
        {
            get
            {
                var animated = _so.FindProperty("_animated");
                return animated.boolValue;
            }
            set
            {
                var animated = _so.FindProperty("_animated");
                var currentValue = animated.boolValue;
                animated.boolValue = value;
                for (int i = 0; i < _layerListProp.arraySize; i++)
                {
                    var layer = _layerListProp.GetArrayElementAtIndex(i);
                    var layerAnimated = layer.FindPropertyRelative("_animated");
                    layerAnimated.boolValue = value;
                }
                _so.ApplyModifiedProperties();
                if (currentValue != value)
                {
                    OnChanged();
                }
            }
        }

        public int AnimLength
        {
            get
            {
                var animLength = _so.FindProperty("_animLength");
                return animLength.intValue;
            }
            set
            {
                var animLength = _so.FindProperty("_animLength");
                var currentValue = animLength.intValue;
                animLength.intValue = value;
                _so.ApplyModifiedProperties();
                if (currentValue != value)
                {
                    OnChanged();
                }
            }
        }

        public int CurrentFrame
        {
            get
            {
                var currentFrame = _so.FindProperty("_currentFrame");
                return currentFrame.intValue;
            }
            set
            {
                var currentFrame = _so.FindProperty("_currentFrame");
                var currentValue = currentFrame.intValue;
                currentFrame.intValue = value;
                for (int i = 0; i < _layerListProp.arraySize; i++)
                {
                    var layer = _layerListProp.GetArrayElementAtIndex(i);
                    var layerCurrentFrame = layer.FindPropertyRelative("_currentFrame");
                    layerCurrentFrame.intValue = value;
                }
                _so.ApplyModifiedProperties();
                if (currentValue != value)
                {
                    OnChanged();
                }
            }
        }

        public Vector2Int TextureSize
        {
            get
            {
                var textureSize = _so.FindProperty("_textureSize");
                return textureSize.vector2IntValue;
            }
            set
            {
                var textureSize = _so.FindProperty("_textureSize");
                var setValue = value;
                setValue.x = setValue.x <= 1 ? 2 : setValue.x;
                setValue.y = setValue.y <= 1 ? 2 : setValue.y;
                setValue.x = setValue.x >= 4098 ? 4098 : setValue.x;
                setValue.y = setValue.y >= 4098 ? 4098 : setValue.y;
                textureSize.vector2IntValue = setValue;
                _so.ApplyModifiedProperties();
            }
        }
        public Texture SingleTexture => _textureData.GetSingleTexture(SelectedIndex);


        public void OnDisable()
        {
            if (_textureData == null) return;
            _textureData.OnDisable();
            _textureData = null;
        }

        public void InitLayerList()
        {
            if (_textureData == null) return;
            InitReorderableList();
        }

        public void InitLayerList(TextureData textureData)
        {
            if (_textureData != null)
            {
                _textureData.OnDisable();
            }
            _textureData = textureData;
            _textureData.OnEnable();
            InitReorderableList();
        }

        public void Blit(ComputeShader cs, bool convert = false)
        {
            _textureData.Blit(cs, convert);
        }

        public void AnimBlit(ComputeShader cs, ComputeShader textureSeetCs, bool convert = false)
        {
            _textureData.AnimBlit(cs, textureSeetCs, convert);
        }

        void InitReorderableList()
        {
            _so = new SerializedObject(_textureData);
            _so.Update();
            _layerListProp = _so.FindProperty("_layerList");

            _reorderableList = new ReorderableList(_so, _layerListProp);
            _reorderableList.drawElementCallback += DrawElement;
            _reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Layer List");
            _reorderableList.elementHeightCallback = (index) => 36;
            _reorderableList.onSelectCallback += OnSelected;
            _reorderableList.onReorderCallback += OnReorder;
            _reorderableList.onAddDropdownCallback += AddDropdown;
            _reorderableList.onRemoveCallback += RemoveElement;
            _reorderableList.drawElementBackgroundCallback += DrawElementBackground;
        }

        public void DoList(Rect rect)
        {
            _so.Update();

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                CustomGUIUtility.dragControll = DragControll.Non;

                _reorderableList.DoList(rect);

                bool chenged = false;
                if (Event.current.type == EventType.Used && check.changed) chenged = true;
                if (CustomGUIUtility.dragControll == DragControll.Complete) chenged = true;
                if (CustomGUIUtility.dragControll == DragControll.Drag) chenged = true;

                if (chenged)
                {
                    OnChanged();
                }
            }
        }

        public float GetListHeight()
        {
            return _reorderableList.GetHeight();
        }

        public void DrawLayerProperty(Rect rect)
        {
            if (SelectedIndex == -1) return;
            GUI.skin = _guiSkin;

            var layerProp = _layerListProp.GetArrayElementAtIndex(SelectedIndex);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                CustomGUIUtility.dragControll = DragControll.Non;
                EditorGUI.PropertyField(rect, layerProp);
                _so.ApplyModifiedProperties();

                bool chenged = false;
                if (Event.current.type == EventType.Used && check.changed) chenged = true;
                if (Event.current.commandName == "ColorPickerChanged") chenged = true;
                if (Event.current.commandName == "CurveChangeCompleted") chenged = true;
                if (Event.current.commandName == "GradientPickerChanged") chenged = true;
                if (CustomGUIUtility.dragControll == DragControll.Complete) chenged = true;
                if (CustomGUIUtility.dragControll == DragControll.Drag) chenged = true;

                if (chenged)
                {
                    OnChanged();
                }
            }

            GUI.skin = null;
        }

        public float GetLayerHeight()
        {
            if (SelectedIndex == -1) return 0;
            var layerProp = _layerListProp.GetArrayElementAtIndex(SelectedIndex);
            return EditorGUI.GetPropertyHeight(layerProp);
        }

        void DrawElementBackground(Rect rect, int index, bool active, bool focused)
        {
            if (SelectedIndex == -1) return;

            var layerProp = _layerListProp.GetArrayElementAtIndex(index);
            var visible = layerProp.FindPropertyRelative("_visible");
            var bgColor = new Color(1, 1, 1, 1);
            if (index == SelectedIndex)
            {
                if (visible.boolValue)
                {
                    bgColor = new Color(0.3f, 0.6f, 0.75f, 0.5f);
                }
                else
                {
                    bgColor = new Color(0.15f, 0.3f, 0.4f, 0.5f);
                }
            }
            else if (visible.boolValue)
            {
                bgColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            }
            else
            {
                bgColor = new Color(0.15f, 0.15f, 0.15f, 0.5f);
            }
            EditorGUI.DrawRect(rect, bgColor);

        }


        void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            if (SelectedIndex == -1) return;

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                var layerProp = _layerListProp.GetArrayElementAtIndex(index);

                var shaderPassProp = layerProp.FindPropertyRelative("_shaderPass");
                var shaderPassNames = shaderPassProp.enumNames;
                var shaderPassIndex = shaderPassProp.enumValueIndex;

                var passRect = rect;
                passRect.height *= 0.5f;
                var passContent = new GUIContent(" " + index + " : " + shaderPassNames[shaderPassIndex]);
                passContent.image = _underArrow;
                if (GUI.Button(passRect, passContent, _guiSkin.button))
                {
                    ChangeDropdown(passRect, index);
                    return;
                }
                EditorGUI.LabelField(passRect, passContent, _guiSkin.GetStyle("Contant Right Image"));

                var visible = layerProp.FindPropertyRelative("_visible");
                var visibleRect = rect;
                visibleRect.height = 12;
                visibleRect.width = 12;
                visibleRect.y += (visibleRect.height * 1.5f) + 3;
                visible.boolValue = EditorGUI.Toggle(visibleRect, visible.boolValue, _guiSkin.GetStyle("Visible Toggle"));

                var visibleLineRect = visibleRect;
                visibleLineRect.width = 2;
                visibleLineRect.x += 18;
                EditorGUI.DrawRect(visibleLineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

                var rocked = layerProp.FindPropertyRelative("_rocked");
                var rockedRect = visibleRect;
                rockedRect.x += 25;
                rocked.boolValue = EditorGUI.Toggle(rockedRect, rocked.boolValue, _guiSkin.GetStyle("Rock Toggle"));

                var rockedLineRect = rockedRect;
                rockedLineRect.width = 2;
                rockedLineRect.x += 18;
                EditorGUI.DrawRect(rockedLineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

                var _masked = layerProp.FindPropertyRelative("_masked");
                var maskRect = rockedRect;
                maskRect.x += 25;
                _masked.boolValue = EditorGUI.Toggle(maskRect, _masked.boolValue, _guiSkin.GetStyle("Mask Toggle"));

                using (new EditorGUI.DisabledScope(!_masked.boolValue))
                {
                    var maskedLayer = layerProp.FindPropertyRelative("_maskedLayer");
                    var maskLayerRect = maskRect;
                    maskLayerRect.width = 42;
                    maskLayerRect.height = 18;
                    maskLayerRect.x += 16;
                    maskLayerRect.y -= 3;
                    var maskLayerContent = new GUIContent();
                    if (maskedLayer.intValue == -1)
                    {
                        maskLayerContent.text = "None";
                    }
                    else
                    {
                        maskLayerContent.text = "" + maskedLayer.intValue;
                    }

                    if (GUI.Button(maskLayerRect, maskLayerContent, _guiSkin.GetStyle("Sub Button")))
                    {
                        MaskLayerDropDown(maskLayerRect, index);
                        return;
                    }
                }
                _so.ApplyModifiedProperties();

                if (check.changed)
                {
                    OnChanged();
                }
            }
        }

        void OnReorder(ReorderableList list)
        {
            UpdateIndex();

            for (int i = 0; i < _layerListProp.arraySize; i++)
            {
                var prop = _layerListProp.GetArrayElementAtIndex(i);
                var maskedLayer = prop.FindPropertyRelative("_maskedLayer");
                var index = prop.FindPropertyRelative("_index");
                if(maskedLayer.intValue >= index.intValue) maskedLayer.intValue = -1;
            }
            _so.ApplyModifiedProperties();
            OnChanged();
        }

        void OnSelected(ReorderableList list)
        {
            _selectedIndex = list.index;
            _so.ApplyModifiedProperties();
            OnChanged();
        }

        void RemoveElement(ReorderableList list)
        {
            for (int i = 0; i < _layerListProp.arraySize; i++)
            {
                var prop = _layerListProp.GetArrayElementAtIndex(i);
                var maskedLayer = prop.FindPropertyRelative("_maskedLayer");
                if(maskedLayer.intValue == _selectedIndex) maskedLayer.intValue = -1;
            }

            _textureData.DestroyLayer(_selectedIndex);

            _layerListProp.DeleteArrayElementAtIndex(_selectedIndex);
            if (_selectedIndex >= _layerListProp.arraySize)
            {
                _selectedIndex--;
            }
            UpdateIndex();

            _so.ApplyModifiedProperties();
            OnChanged();
        }

        void UpdateIndex()
        {
            for (int i = 0; i < _layerListProp.arraySize; i++)
            {
                var prop = _layerListProp.GetArrayElementAtIndex(i);
                prop.FindPropertyRelative("_index").intValue = i;
            }
        }

        void MaskLayerDropDown(Rect buttonRect, int index)
        {
            GenericMenu menu = new GenericMenu();

            var layerProp = _layerListProp.GetArrayElementAtIndex(index);
            var maskedLayer = layerProp.FindPropertyRelative("_maskedLayer");

            menu.AddItem(new GUIContent("None"), on: false, func: () =>
            {
                maskedLayer.intValue = -1;
                _so.ApplyModifiedProperties();
                OnChanged();
            });


            for (int i = 0; i < _layerListProp.arraySize; i++)
            {
                if (index == i) break;
                var value = i;
                menu.AddItem(new GUIContent("" + i), on: false, func: () =>
                {
                    maskedLayer.intValue = value;
                    _so.ApplyModifiedProperties();
                    OnChanged();
                });
            }

            menu.DropDown(buttonRect);
        }

        void ChangeDropdown(Rect buttonRect, int index)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Clear"), on: false, func: () => ChangeLayer(ShaderPass.Clear, index));
            menu.AddItem(new GUIContent("Draw/Draw"), on: false, func: () => ChangeLayer(ShaderPass.Draw_Draw, index));
            menu.AddItem(new GUIContent("Draw/Texture"), on: false, func: () => ChangeLayer(ShaderPass.Draw_Texture, index));
            menu.AddItem(new GUIContent("Draw/Noise"), on: false, func: () => ChangeLayer(ShaderPass.Draw_Noise, index));
            menu.AddItem(new GUIContent("Draw/Shape"), on: false, func: () => ChangeLayer(ShaderPass.Draw_Shape, index));
            menu.AddItem(new GUIContent("Displacement/Displacement"), on: false, func: () => ChangeLayer(ShaderPass.Displacement_Displacement, index));
            menu.AddItem(new GUIContent("Displacement/Texture"), on: false, func: () => ChangeLayer(ShaderPass.Displacement_Texture, index));
            menu.AddItem(new GUIContent("Displacement/NormalMap"), on: false, func: () => ChangeLayer(ShaderPass.Displacement_NormalMap, index));
            menu.AddItem(new GUIContent("Displacement/Noise"), on: false, func: () => ChangeLayer(ShaderPass.Displacement_Noise, index));
            menu.AddItem(new GUIContent("Filter/Blur"), on: false, func: () => ChangeLayer(ShaderPass.Filter_Blur, index));
            menu.AddItem(new GUIContent("Filter/GradationSample"), on: false, func: () => ChangeLayer(ShaderPass.Filter_GradationSample, index));
            menu.AddItem(new GUIContent("Filter/Glow"), on: false, func: () => ChangeLayer(ShaderPass.Filter_Glow, index));
            menu.AddItem(new GUIContent("Filter/ColorBalance"), on: false, func: () => ChangeLayer(ShaderPass.Filter_ColorBalance, index));
            menu.AddItem(new GUIContent("CustomShader"), on: false, func: () => ChangeLayer(ShaderPass.CustomShader, index));

            menu.DropDown(buttonRect);
        }
        void ChangeLayer(ShaderPass pass, int index)
        {
            _textureData.LayerList[index].SetPass(pass);
            EditorUtility.SetDirty(_textureData);
            InitReorderableList();
            OnChanged();
        }

        void AddDropdown(Rect buttonRect, ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Clear"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Clear)));
            menu.AddItem(new GUIContent("Draw/Draw"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Draw_Draw)));
            menu.AddItem(new GUIContent("Draw/Texture"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Draw_Texture)));
            menu.AddItem(new GUIContent("Draw/Noise"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Draw_Noise)));
            menu.AddItem(new GUIContent("Draw/Shape"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Draw_Shape)));
            menu.AddItem(new GUIContent("Displacement/Displacement"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Displacement_Displacement)));
            menu.AddItem(new GUIContent("Displacement/Texture"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Displacement_Texture)));
            menu.AddItem(new GUIContent("Displacement/NormalMap"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Displacement_NormalMap)));
            menu.AddItem(new GUIContent("Displacement/Noise"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Displacement_Noise)));
            menu.AddItem(new GUIContent("Filter/Blur"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Filter_Blur)));
            menu.AddItem(new GUIContent("Filter/GradationSample"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Filter_GradationSample)));
            menu.AddItem(new GUIContent("Filter/Glow"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Filter_Glow)));
            menu.AddItem(new GUIContent("Filter/ColorBalance"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.Filter_ColorBalance)));
            menu.AddItem(new GUIContent("CustomShader"), on: false, func: () => AddLayer(new Layer(list.count, ShaderPass.CustomShader)));

            menu.DropDown(buttonRect);
        }

        void AddLayer(Layer layer)
        {
            _textureData.LayerList.Add(layer);
            EditorUtility.SetDirty(_textureData);
            InitReorderableList();

            _selectedIndex = _layerListProp.arraySize - 1;
            _so.ApplyModifiedProperties();
            OnChanged();
        }

    }
}