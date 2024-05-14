using UnityEngine;
using UnityEditor;

namespace VFXTextureMaker
{
    public enum DragControll
    {
        Drag,
        Complete,
        Non
    }
    public static class CustomGUIUtility
    {
        public static DragControll dragControll = DragControll.Non;

        public static readonly float FoldoutHeight = 22;
        public static bool Foldout(Rect rect, bool display, GUIContent title)
        {
            rect.height = FoldoutHeight;
            var style = GUI.skin.FindStyle("Layer Foldout");
            if (style == null) style = new GUIStyle("ShurikenModuleTitle");

            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                display = !display;
                e.Use();
            }

            return display;
        }

        public static readonly float LayerSpaceHeight = 4;
        public static readonly float PropertyHeight = EditorGUIUtility.singleLineHeight * 2;
        public static void PropertyField(Rect rect, SerializedProperty property, GUIContent label)
        {
            var currentSkin = GUI.skin;
            GUI.skin = null;
            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            rect.y += EditorGUIUtility.singleLineHeight;
            rect.xMin += 10;

            var rectProp = rect;
            EditorGUI.PropertyField(rectProp, property, new GUIContent(""));
            GUI.skin = currentSkin;
        }

        public static void MaterialPropertyField(Rect rect, MaterialProperty property)
        {
            var currentSkin = GUI.skin;
            GUI.skin = null;
            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, property.displayName, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            rect.y += EditorGUIUtility.singleLineHeight;
            rect.xMin += 10;

            var rectProp = rect;
            switch (property.type)
            {
                case MaterialProperty.PropType.Color:
                    property.colorValue = EditorGUI.ColorField(rect, "", property.colorValue);
                    break;
                case MaterialProperty.PropType.Vector:
                    property.vectorValue = EditorGUI.Vector4Field(rect, "", property.vectorValue);
                    break;
                case MaterialProperty.PropType.Float:
                    var rectLabelX = rect;
                    rectLabelX.width = 16;
                    EditorGUI.LabelField(rectLabelX, "X");

                    var ev = Event.current;
                    Vector2 mous_pos = ev.mousePosition;
                    bool flg_on_rect = rectLabelX.Contains(mous_pos);

                    var rectValueX = rect;
                    rectValueX.width -= 16;
                    rectValueX.x += 16;
                    property.floatValue = EditorGUI.FloatField(rectValueX, property.floatValue);

                    int id = GUIUtility.GetControlID(FocusType.Passive);

                    if (ev.button == 0)
                    {
                        switch (ev.type)
                        {
                            case EventType.MouseDown:
                                if (flg_on_rect)
                                {
                                    GUIUtility.hotControl = id;
                                    ev.Use();
                                }
                                break;

                            case EventType.MouseDrag:
                                if (GUIUtility.hotControl == id)
                                {
                                    float dis = ev.delta.x;
                                    property.floatValue = property.floatValue * 100.0f + dis * 10.0f;
                                    property.floatValue = Mathf.Floor(Mathf.Abs(property.floatValue)) / 100f * Mathf.Sign(property.floatValue);
                                    dragControll = DragControll.Drag;
                                }
                                break;

                            case EventType.MouseUp:
                                if (GUIUtility.hotControl == id)
                                {
                                    GUIUtility.hotControl = 0;
                                    dragControll = DragControll.Complete;
                                    ev.Use();
                                }
                                break;

                            case EventType.Ignore:
                                if (GUIUtility.hotControl == id)
                                {
                                    GUIUtility.hotControl = 0;
                                    dragControll = DragControll.Complete;
                                    ev.Use();
                                }
                                break;
                        }
                    }
                    EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);

                    break;
                case MaterialProperty.PropType.Range:
                    property.floatValue = EditorGUI.Slider(rect, property.floatValue, property.rangeLimits.x, property.rangeLimits.y);
                    break;
                case MaterialProperty.PropType.Texture:
                    property.textureValue = (Texture)EditorGUI.ObjectField(rect, property.textureValue, typeof(Texture), false);
                    break;
            }
            GUI.skin = currentSkin;
        }
        public static float PropertyValueField(Rect rect, SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            rect.y += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight;
            rect.xMin += 10;

            var rectProp = rect;
            var value = property.FindPropertyRelative("_value");
            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));
            EditorGUI.PropertyField(rectProp, value, new GUIContent(""));

            return height;
        }
        public static void FloatField(Rect rect, SerializedProperty property, GUIContent label)
        {
            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            rect.xMin += 10;
            rect.y += EditorGUIUtility.singleLineHeight;


            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

            var rectLabelX = rect;
            rectLabelX.width = 16;
            EditorGUI.LabelField(rectLabelX, "X");


            var ev = Event.current;
            Vector2 mous_pos = ev.mousePosition;
            bool flg_on_rect = rectLabelX.Contains(mous_pos);

            var rectValueX = rect;
            rectValueX.width -= 16;
            rectValueX.x += 16;
            var value = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(rectValueX, value, new GUIContent(""));

            int id = GUIUtility.GetControlID(FocusType.Passive);

            if (ev.button == 0)
            {
                switch (ev.type)
                {
                    case EventType.MouseDown:
                        if (flg_on_rect)
                        {
                            GUIUtility.hotControl = id;
                            ev.Use();
                        }
                        break;

                    case EventType.MouseDrag:
                        if (GUIUtility.hotControl == id)
                        {
                            float dis = ev.delta.x;
                            value.floatValue = value.floatValue * 100.0f + dis * 10.0f;
                            value.floatValue = Mathf.Floor(Mathf.Abs(value.floatValue)) / 100f * Mathf.Sign(value.floatValue);
                            dragControll = DragControll.Drag;
                        }
                        break;

                    case EventType.MouseUp:
                        if (GUIUtility.hotControl == id)
                        {
                            GUIUtility.hotControl = 0;
                            dragControll = DragControll.Complete;
                            ev.Use();
                        }
                        break;

                    case EventType.Ignore:
                        if (GUIUtility.hotControl == id)
                        {
                            GUIUtility.hotControl = 0;
                            dragControll = DragControll.Complete;
                            ev.Use();
                        }
                        break;
                }
            }
            EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);

        }

        public static void IntField(Rect rect, SerializedProperty property, GUIContent label)
        {
            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            rect.y += EditorGUIUtility.singleLineHeight;
            rect.xMin += 10;


            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

            var rectLabelX = rect;
            rectLabelX.width = 16;
            EditorGUI.LabelField(rectLabelX, "X");


            var ev = Event.current;
            Vector2 mous_pos = ev.mousePosition;
            bool flg_on_rect = rectLabelX.Contains(mous_pos);

            var rectValueX = rect;
            rectValueX.width -= 16;
            rectValueX.x += 16;
            var value = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(rectValueX, value, new GUIContent(""));

            int id = GUIUtility.GetControlID(FocusType.Passive);

            if (ev.button == 0)
            {
                switch (ev.type)
                {
                    case EventType.MouseDown:
                        if (flg_on_rect)
                        {
                            GUIUtility.hotControl = id;
                            ev.Use();
                        }
                        break;

                    case EventType.MouseDrag:
                        if (GUIUtility.hotControl == id)
                        {
                            float dis = ev.delta.x;
                            value.intValue = value.intValue + (int)dis;
                            dragControll = DragControll.Drag;
                        }
                        break;

                    case EventType.MouseUp:
                        if (GUIUtility.hotControl == id)
                        {
                            GUIUtility.hotControl = 0;
                            dragControll = DragControll.Complete;
                            ev.Use();
                        }
                        break;

                    case EventType.Ignore:
                        if (GUIUtility.hotControl == id)
                        {
                            GUIUtility.hotControl = 0;
                            dragControll = DragControll.Complete;
                            ev.Use();
                        }
                        break;
                }
            }
            EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);

        }
        public static float Vector4Field(Rect rect, SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            rect.y += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight;
            rect.xMin += 10;

            var rectProp = rect;
            var value = property.FindPropertyRelative("_value");
            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));
            value.vector4Value = EditorGUI.Vector4Field(rectProp, new GUIContent(""), value.vector4Value);

            return height;
        }
        public static void FloatAnimField(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            var value = property.FindPropertyRelative("_value");
            var curve = property.FindPropertyRelative("_curve");
            var isAnim = property.FindPropertyRelative("_isAnim");
            var isCurve = property.FindPropertyRelative("_isCurve");

            var isAnimStyle = GUI.skin.FindStyle("Anim Toggle");
            if (isAnimStyle == null) isAnimStyle = new GUIStyle("Toggle");

            var isCurveStyle = GUI.skin.FindStyle("Curve Toggle");
            if (isCurveStyle == null) isCurveStyle = new GUIStyle("Toggle");

            var isKeyStyle = GUI.skin.FindStyle("Key Toggle");
            if (isKeyStyle == null) isKeyStyle = new GUIStyle("Toggle");

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            var rectIsAnim = rect;
            rectIsAnim.x = lineRect.x + 5;
            rectIsAnim.width = 16;

            isAnim.boolValue = EditorGUI.Toggle(rectIsAnim, new GUIContent(""), isAnim.boolValue, isAnimStyle);

            if (isAnim.boolValue)
            {
                var rectIsCurve = rect;
                rectIsCurve.x = lineRect.x + 21;
                rectIsCurve.width = 16;
                isCurve.boolValue = EditorGUI.Toggle(rectIsCurve, new GUIContent(""), isCurve.boolValue, isCurveStyle);

                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X");

                var curveX = curve.animationCurveValue;
                float valueX = 0;
                int indexX = -1;
                bool isKeyX = false;
                for (int i = 0; i < curveX.length; i++)
                {
                    if (curveX[i].time == (float)currentFrame)
                    {
                        valueX = curveX[i].value;
                        indexX = i;
                        isKeyX = true;
                    }
                }
                var currentKeyX = isKeyX;

                var rectKeyX = rect;
                rectKeyX.width = 16;
                rectKeyX.x += 12;
                rectKeyX.y += 1;
                isKeyX = EditorGUI.Toggle(rectKeyX, new GUIContent(""), isKeyX, isKeyStyle);

                var rectValueX = rect;
                rectValueX.width -= 32;
                rectValueX.x += 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueX, curve, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyX))
                    {
                        if (currentKeyX && !isKeyX)
                        {
                            EditorGUI.FloatField(rectValueX, valueX);
                            curveX.RemoveKey(indexX);
                        }
                        else if (!currentKeyX && isKeyX)
                        {
                            EditorGUI.FloatField(rectValueX, curveX.Evaluate(currentFrame));
                            var num = curveX.AddKey((float)currentFrame, curveX.Evaluate(currentFrame));
                            curveX.SmoothTangents(num, 0);
                        }
                        else if (currentKeyX && isKeyX)
                        {
                            var ev = Event.current;
                            Vector2 mous_pos = ev.mousePosition;
                            bool flg_on_rect = rectLabelX.Contains(mous_pos);

                            valueX = EditorGUI.FloatField(rectValueX, valueX);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (flg_on_rect)
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            float dis = ev.delta.x;
                                            valueX = valueX * 100.0f + dis * 10.0f;
                                            valueX = Mathf.Floor(Mathf.Abs(valueX)) / 100f * Mathf.Sign(valueX);
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);
                            curveX.MoveKey(indexX, new Keyframe((float)currentFrame, valueX));
                            curveX.SmoothTangents(indexX, 0);

                        }
                        else
                        {

                            EditorGUI.FloatField(rectValueX, curveX.Evaluate(currentFrame));
                        }
                    }
                    curve.animationCurveValue = curveX;
                }
            }
            else
            {
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X");

                var ev = Event.current;
                Vector2 mous_pos = ev.mousePosition;
                bool flg_on_rect = rectLabelX.Contains(mous_pos);

                var rectValueX = rect;
                rectValueX.width -= 16;
                rectValueX.x += 16;
                EditorGUI.PropertyField(rectValueX, value, new GUIContent(""));

                int id = GUIUtility.GetControlID(FocusType.Passive);

                if (ev.button == 0)
                {
                    switch (ev.type)
                    {
                        case EventType.MouseDown:
                            if (flg_on_rect)
                            {
                                GUIUtility.hotControl = id;
                                ev.Use();
                            }
                            break;

                        case EventType.MouseDrag:
                            if (GUIUtility.hotControl == id)
                            {
                                float dis = ev.delta.x;
                                value.floatValue = value.floatValue * 100.0f + dis * 10.0f;
                                value.floatValue = Mathf.Floor(Mathf.Abs(value.floatValue)) / 100f * Mathf.Sign(value.floatValue);
                                dragControll = DragControll.Drag;
                            }
                            break;

                        case EventType.MouseUp:
                            if (GUIUtility.hotControl == id)
                            {
                                GUIUtility.hotControl = 0;
                                dragControll = DragControll.Complete;
                                ev.Use();
                            }
                            break;

                        case EventType.Ignore:
                            if (GUIUtility.hotControl == id)
                            {
                                GUIUtility.hotControl = 0;
                                dragControll = DragControll.Complete;
                                ev.Use();
                            }
                            break;
                    }
                }
                EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);
            }
        }
        public static void BoolAnimField(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            var value = property.FindPropertyRelative("_value");
            var curve = property.FindPropertyRelative("_curve");
            var isAnim = property.FindPropertyRelative("_isAnim");
            var isCurve = property.FindPropertyRelative("_isCurve");

            var isAnimStyle = GUI.skin.FindStyle("Anim Toggle");
            if (isAnimStyle == null) isAnimStyle = new GUIStyle("Toggle");

            var isCurveStyle = GUI.skin.FindStyle("Curve Toggle");
            if (isCurveStyle == null) isCurveStyle = new GUIStyle("Toggle");

            var isKeyStyle = GUI.skin.FindStyle("Key Toggle");
            if (isKeyStyle == null) isKeyStyle = new GUIStyle("Toggle");

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            var rectIsAnim = rect;
            rectIsAnim.x = lineRect.x + 5;
            rectIsAnim.width = 16;

            isAnim.boolValue = EditorGUI.Toggle(rectIsAnim, new GUIContent(""), isAnim.boolValue, isAnimStyle);

            if (isAnim.boolValue)
            {
                var rectIsCurve = rect;
                rectIsCurve.x = lineRect.x + 21;
                rectIsCurve.width = 16;
                isCurve.boolValue = EditorGUI.Toggle(rectIsCurve, new GUIContent(""), isCurve.boolValue, isCurveStyle);

                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X");

                var curveX = curve.animationCurveValue;
                bool valueX = false;
                int indexX = -1;
                bool isKeyX = false;
                for (int i = 0; i < curveX.length; i++)
                {
                    if (curveX[i].time == (float)currentFrame)
                    {
                        valueX = curveX[i].value >= 1;
                        indexX = i;
                        isKeyX = true;
                    }
                }
                var currentKeyX = isKeyX;

                var rectKeyX = rect;
                rectKeyX.width = 16;
                rectKeyX.x += 12;
                rectKeyX.y += 1;
                isKeyX = EditorGUI.Toggle(rectKeyX, new GUIContent(""), isKeyX, isKeyStyle);

                var rectValueX = rect;
                rectValueX.width -= 32;
                rectValueX.x += 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueX, curve, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyX))
                    {
                        if (currentKeyX && !isKeyX)
                        {
                            EditorGUI.Toggle(rectValueX, valueX);
                            curveX.RemoveKey(indexX);
                        }
                        else if (!currentKeyX && isKeyX)
                        {
                            EditorGUI.Toggle(rectValueX, curveX.Evaluate(currentFrame) >= 1);
                            var num = curveX.AddKey(new Keyframe(currentFrame, curveX.Evaluate(currentFrame), float.PositiveInfinity, float.PositiveInfinity));
                        }
                        else if (currentKeyX && isKeyX)
                        {
                            valueX = EditorGUI.Toggle(rectValueX, valueX);
                            curveX.MoveKey(indexX, new Keyframe((float)currentFrame, valueX ? 0 : 1, float.PositiveInfinity, float.PositiveInfinity));
                        }
                        else
                        {
                            EditorGUI.Toggle(rectValueX, curveX.Evaluate(currentFrame) >= 1);
                        }
                    }
                    curve.animationCurveValue = curveX;
                }
            }
            else
            {
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X");

                var rectValueX = rect;
                rectValueX.width -= 16;
                rectValueX.x += 16;
                EditorGUI.PropertyField(rectValueX, value, new GUIContent(""));

            }
        }
        public static void IntAnimField(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            var value = property.FindPropertyRelative("_value");
            var curve = property.FindPropertyRelative("_curve");
            var isAnim = property.FindPropertyRelative("_isAnim");
            var isCurve = property.FindPropertyRelative("_isCurve");

            var isAnimStyle = GUI.skin.FindStyle("Anim Toggle");
            if (isAnimStyle == null) isAnimStyle = new GUIStyle("Toggle");

            var isCurveStyle = GUI.skin.FindStyle("Curve Toggle");
            if (isCurveStyle == null) isCurveStyle = new GUIStyle("Toggle");

            var isKeyStyle = GUI.skin.FindStyle("Key Toggle");
            if (isKeyStyle == null) isKeyStyle = new GUIStyle("Toggle");

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            var rectIsAnim = rect;
            rectIsAnim.x = lineRect.x + 5;
            rectIsAnim.width = 16;
            isAnim.boolValue = EditorGUI.Toggle(rectIsAnim, new GUIContent(""), isAnim.boolValue, isAnimStyle);

            if (isAnim.boolValue)
            {
                var rectIsCurve = rect;
                rectIsCurve.x = lineRect.x + 21;
                rectIsCurve.width = 16;
                isCurve.boolValue = EditorGUI.Toggle(rectIsCurve, new GUIContent(""), isCurve.boolValue, isCurveStyle);

                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.15f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X");

                var curveX = curve.animationCurveValue;
                int valueX = 0;
                int indexX = -1;
                bool isKeyX = false;
                for (int i = 0; i < curveX.length; i++)
                {
                    if (curveX[i].time == (float)currentFrame)
                    {
                        valueX = (int)curveX[i].value;
                        indexX = i;
                        isKeyX = true;
                    }
                }
                var currentKeyX = isKeyX;

                var rectKeyX = rect;
                rectKeyX.width = 16;
                rectKeyX.x += 12;
                rectKeyX.y += 1;
                isKeyX = EditorGUI.Toggle(rectKeyX, new GUIContent(""), isKeyX, isKeyStyle);

                var rectValueX = rect;
                rectValueX.width -= 32;
                rectValueX.x += 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueX, curve, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyX))
                    {
                        if (currentKeyX && !isKeyX)
                        {
                            EditorGUI.IntField(rectValueX, valueX);
                            curveX.RemoveKey(indexX);
                        }
                        else if (!currentKeyX && isKeyX)
                        {
                            EditorGUI.IntField(rectValueX, (int)curveX.Evaluate(currentFrame));
                            var num = curveX.AddKey((float)currentFrame, curveX.Evaluate(currentFrame));
                            curveX.SmoothTangents(num, 0);
                        }
                        else if (currentKeyX && isKeyX)
                        {
                            var ev = Event.current;
                            Vector2 mous_pos = ev.mousePosition;
                            bool flg_on_rect = rectLabelX.Contains(mous_pos);

                            valueX = EditorGUI.IntField(rectValueX, valueX);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (flg_on_rect)
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            valueX = valueX + (int)ev.delta.x;
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);
                            curveX.MoveKey(indexX, new Keyframe((float)currentFrame, valueX));
                            curveX.SmoothTangents(indexX, 0);

                        }
                        else
                        {

                            EditorGUI.IntField(rectValueX, (int)curveX.Evaluate(currentFrame));
                        }
                    }
                    curve.animationCurveValue = curveX;
                }
            }
            else
            {
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.15f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X");

                var ev = Event.current;
                Vector2 mous_pos = ev.mousePosition;
                bool flg_on_rect = rectLabelX.Contains(mous_pos);

                var rectValueX = rect;
                rectValueX.width -= 16;
                rectValueX.x += 16;
                EditorGUI.PropertyField(rectValueX, value, new GUIContent(""));

                int id = GUIUtility.GetControlID(FocusType.Passive);

                if (ev.button == 0)
                {
                    switch (ev.type)
                    {
                        case EventType.MouseDown:
                            if (flg_on_rect)
                            {
                                GUIUtility.hotControl = id;
                                ev.Use();
                            }
                            break;

                        case EventType.MouseDrag:
                            if (GUIUtility.hotControl == id)
                            {
                                value.intValue = value.intValue + (int)ev.delta.x;
                                dragControll = DragControll.Drag;
                            }
                            break;

                        case EventType.MouseUp:
                            if (GUIUtility.hotControl == id)
                            {
                                GUIUtility.hotControl = 0;
                                dragControll = DragControll.Complete;
                                ev.Use();
                            }
                            break;

                        case EventType.Ignore:
                            if (GUIUtility.hotControl == id)
                            {
                                GUIUtility.hotControl = 0;
                                dragControll = DragControll.Complete;
                                ev.Use();
                            }
                            break;
                    }
                }
                EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);
            }
        }
        public static void Vector2AnimField(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            var value = property.FindPropertyRelative("_value");
            var curveX = property.FindPropertyRelative("_curveX");
            var curveY = property.FindPropertyRelative("_curveY");
            var isAnim = property.FindPropertyRelative("_isAnim");
            var isCurve = property.FindPropertyRelative("_isCurve");

            var isAnimStyle = GUI.skin.FindStyle("Anim Toggle");
            if (isAnimStyle == null) isAnimStyle = new GUIStyle("Toggle");

            var isCurveStyle = GUI.skin.FindStyle("Curve Toggle");
            if (isCurveStyle == null) isCurveStyle = new GUIStyle("Toggle");

            var isKeyStyle = GUI.skin.FindStyle("Key Toggle");
            if (isKeyStyle == null) isKeyStyle = new GUIStyle("Toggle");

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;
            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            var rectIsAnim = rect;
            rectIsAnim.x = lineRect.x + 5;
            rectIsAnim.width = 16;
            isAnim.boolValue = EditorGUI.Toggle(rectIsAnim, new GUIContent(""), isAnim.boolValue, isAnimStyle);

            if (isAnim.boolValue)
            {
                var rectIsCurve = rect;
                rectIsCurve.x = lineRect.x + 21;
                rectIsCurve.width = 16;
                isCurve.boolValue = EditorGUI.Toggle(rectIsCurve, new GUIContent(""), isCurve.boolValue, isCurveStyle);

                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X", EditorStyles.miniLabel);

                var curveXValue = curveX.animationCurveValue;
                float valueX = 0;
                int indexX = -1;
                bool isKeyX = false;
                for (int i = 0; i < curveXValue.length; i++)
                {
                    if (curveXValue[i].time == (float)currentFrame)
                    {
                        valueX = curveXValue[i].value;
                        indexX = i;
                        isKeyX = true;
                    }
                }
                var currentKeyX = isKeyX;

                var rectKeyX = rect;
                rectKeyX.width = 16;
                rectKeyX.x += 12;
                rectKeyX.y += 1;
                isKeyX = EditorGUI.Toggle(rectKeyX, new GUIContent(""), isKeyX, isKeyStyle);

                var rectValueX = rect;
                rectValueX.width *= 0.5f;
                rectValueX.width -= 32;
                rectValueX.x += 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueX, curveX, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyX))
                    {
                        if (currentKeyX && !isKeyX)
                        {
                            EditorGUI.FloatField(rectValueX, valueX);
                            curveXValue.RemoveKey(indexX);
                        }
                        else if (!currentKeyX && isKeyX)
                        {
                            EditorGUI.FloatField(rectValueX, curveXValue.Evaluate(currentFrame));
                            var num = curveXValue.AddKey((float)currentFrame, curveXValue.Evaluate(currentFrame));
                            curveXValue.SmoothTangents(num, 0);
                        }
                        else if (currentKeyX && isKeyX)
                        {
                            var ev = Event.current;
                            Vector2 mous_pos = ev.mousePosition;
                            bool flg_on_rect = rectLabelX.Contains(mous_pos);

                            valueX = EditorGUI.FloatField(rectValueX, valueX);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (flg_on_rect)
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            float dis = ev.delta.x;
                                            valueX = valueX * 100.0f + dis * 10.0f;
                                            valueX = Mathf.Floor(Mathf.Abs(valueX)) / 100f * Mathf.Sign(valueX);
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);
                            curveXValue.MoveKey(indexX, new Keyframe((float)currentFrame, valueX));
                            curveXValue.SmoothTangents(indexX, 0);

                        }
                        else
                        {

                            EditorGUI.FloatField(rectValueX, curveXValue.Evaluate(currentFrame));
                        }
                    }
                    curveX.animationCurveValue = curveXValue;
                }

                var rectLabelY = rect;
                rectLabelY.x += rect.width * 0.5f + 2;
                rectLabelY.width = 16;
                EditorGUI.LabelField(rectLabelY, "Y", EditorStyles.miniLabel);

                var curveYValue = curveY.animationCurveValue;
                float valueY = 0;
                int indexY = -1;
                bool isKeyY = false;
                for (int i = 0; i < curveYValue.length; i++)
                {
                    if (curveYValue[i].time == currentFrame)
                    {
                        valueY = curveYValue[i].value;
                        indexY = i;
                        isKeyY = true;
                    }
                }
                var currentKeyY = isKeyY;

                var rectKeyY = rect;
                rectKeyY.width = 16;
                rectKeyY.x += rect.width * 0.5f + 2;
                rectKeyY.x += 12;
                rectKeyY.y += 1;
                isKeyY = EditorGUI.Toggle(rectKeyY, new GUIContent(""), isKeyY, isKeyStyle);

                var rectValueY = rect;
                rectValueY.width *= 0.5f;
                rectValueY.width -= 32;
                rectValueY.x += rect.width * 0.5f + 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueY, curveY, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyY))
                    {
                        if (currentKeyY && !isKeyY)
                        {
                            EditorGUI.FloatField(rectValueY, valueY);
                            curveYValue.RemoveKey(indexY);
                        }
                        else if (!currentKeyY && isKeyY)
                        {
                            EditorGUI.FloatField(rectValueY, curveYValue.Evaluate(currentFrame));
                            var num = curveYValue.AddKey((float)currentFrame, curveYValue.Evaluate(currentFrame));
                            curveYValue.SmoothTangents(num, 0);
                        }
                        else if (currentKeyY && isKeyY)
                        {
                            var ev = Event.current;

                            valueY = EditorGUI.FloatField(rectValueY, valueY);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (rectLabelY.Contains(ev.mousePosition))
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            float dis = ev.delta.x;
                                            valueY = valueY * 100.0f + dis * 10.0f;
                                            valueY = Mathf.Floor(Mathf.Abs(valueY)) / 100f * Mathf.Sign(valueY);
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelY, MouseCursor.SlideArrow);
                            curveYValue.MoveKey(indexY, new Keyframe((float)currentFrame, valueY));
                            curveYValue.SmoothTangents(indexY, 1);
                        }
                        else
                        {
                            EditorGUI.FloatField(rectValueY, curveYValue.Evaluate(currentFrame));
                        }
                    }
                    curveY.animationCurveValue = curveYValue;
                }
            }
            else
            {
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;
                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));
                EditorGUI.PropertyField(rect, value, new GUIContent(""));
            }
        }
        public static void Vector4AnimField(Rect rect, SerializedProperty property, int currentFrame, GUIContent label)
        {
            var value = property.FindPropertyRelative("_value");
            var curveX = property.FindPropertyRelative("_curveX");
            var curveY = property.FindPropertyRelative("_curveY");
            var curveZ = property.FindPropertyRelative("_curveZ");
            var curveW = property.FindPropertyRelative("_curveW");
            var isAnim = property.FindPropertyRelative("_isAnim");
            var isCurve = property.FindPropertyRelative("_isCurve");

            var isAnimStyle = GUI.skin.FindStyle("Anim Toggle");
            if (isAnimStyle == null) isAnimStyle = new GUIStyle("Toggle");

            var isCurveStyle = GUI.skin.FindStyle("Curve Toggle");
            if (isCurveStyle == null) isCurveStyle = new GUIStyle("Toggle");

            var isKeyStyle = GUI.skin.FindStyle("Key Toggle");
            if (isKeyStyle == null) isKeyStyle = new GUIStyle("Toggle");

            var rectBG = rect;
            rectBG.width = 105;
            EditorGUI.DrawRect(rectBG, new Color(1, 1, 1, 0.1f));

            var rectLabel = rect;
            rectLabel.width = 100;

            EditorGUI.LabelField(rectLabel, label, EditorStyles.boldLabel);

            var lineRect = rect;
            lineRect.width = 2;
            lineRect.x = rect.x + rectLabel.width + 5;
            EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            var rectIsAnim = rect;
            rectIsAnim.x = lineRect.x + 5;
            rectIsAnim.width = 16;
            isAnim.boolValue = EditorGUI.Toggle(rectIsAnim, new GUIContent(""), isAnim.boolValue, isAnimStyle);

            if (isAnim.boolValue)
            {
                var rectIsCurve = rect;
                rectIsCurve.x = lineRect.x + 21;
                rectIsCurve.width = 16;
                isCurve.boolValue = EditorGUI.Toggle(rectIsCurve, new GUIContent(""), isCurve.boolValue, isCurveStyle);

                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;

                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));

                var rectLabelX = rect;
                rectLabelX.width = 16;
                EditorGUI.LabelField(rectLabelX, "X");

                var curveXValue = curveX.animationCurveValue;
                float valueX = 0;
                int indexX = -1;
                bool isKeyX = false;
                for (int i = 0; i < curveXValue.length; i++)
                {
                    if (curveXValue[i].time == (float)currentFrame)
                    {
                        valueX = curveXValue[i].value;
                        indexX = i;
                        isKeyX = true;
                    }
                }
                var currentKeyX = isKeyX;

                var rectKeyX = rect;
                rectKeyX.width = 16;
                rectKeyX.x += 12;
                rectKeyX.y += 1;
                isKeyX = EditorGUI.Toggle(rectKeyX, new GUIContent(""), isKeyX, isKeyStyle);

                var rectValueX = rect;
                rectValueX.width *= 0.25f;
                rectValueX.width -= 32;
                rectValueX.x += 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueX, curveX, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyX))
                    {
                        if (currentKeyX && !isKeyX)
                        {
                            EditorGUI.FloatField(rectValueX, valueX);
                            curveXValue.RemoveKey(indexX);
                        }
                        else if (!currentKeyX && isKeyX)
                        {
                            EditorGUI.FloatField(rectValueX, curveXValue.Evaluate(currentFrame));
                            var num = curveXValue.AddKey((float)currentFrame, curveXValue.Evaluate(currentFrame));
                            curveXValue.SmoothTangents(num, 0);
                        }
                        else if (currentKeyX && isKeyX)
                        {
                            var ev = Event.current;
                            Vector2 mous_pos = ev.mousePosition;
                            bool flg_on_rect = rectLabelX.Contains(mous_pos);

                            valueX = EditorGUI.FloatField(rectValueX, valueX);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (flg_on_rect)
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            float dis = ev.delta.x;
                                            valueX = valueX * 100.0f + dis * 10.0f;
                                            valueX = Mathf.Floor(Mathf.Abs(valueX)) / 100f * Mathf.Sign(valueX);
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelX, MouseCursor.SlideArrow);
                            curveXValue.MoveKey(indexX, new Keyframe((float)currentFrame, valueX));
                            curveXValue.SmoothTangents(indexX, 0);

                        }
                        else
                        {

                            EditorGUI.FloatField(rectValueX, curveXValue.Evaluate(currentFrame));
                        }
                    }
                    curveX.animationCurveValue = curveXValue;
                }

                var rectLabelY = rect;
                rectLabelY.x += rect.width * 0.25f + 2;
                rectLabelY.width = 16;
                EditorGUI.LabelField(rectLabelY, "Y");

                var curveYValue = curveY.animationCurveValue;
                float valueY = 0;
                int indexY = -1;
                bool isKeyY = false;
                for (int i = 0; i < curveYValue.length; i++)
                {
                    if (curveYValue[i].time == currentFrame)
                    {
                        valueY = curveYValue[i].value;
                        indexY = i;
                        isKeyY = true;
                    }
                }
                var currentKeyY = isKeyY;

                var rectKeyY = rect;
                rectKeyY.width = 16;
                rectKeyY.x += rect.width * 0.25f + 2;
                rectKeyY.x += 12;
                rectKeyY.y += 1;
                isKeyY = EditorGUI.Toggle(rectKeyY, new GUIContent(""), isKeyY, isKeyStyle);

                var rectValueY = rect;
                rectValueY.width *= 0.25f;
                rectValueY.width -= 32;
                rectValueY.x += rect.width * 0.25f + 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueY, curveY, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyY))
                    {
                        if (currentKeyY && !isKeyY)
                        {
                            EditorGUI.FloatField(rectValueY, valueY);
                            curveYValue.RemoveKey(indexY);
                        }
                        else if (!currentKeyY && isKeyY)
                        {
                            EditorGUI.FloatField(rectValueY, curveYValue.Evaluate(currentFrame));
                            var num = curveYValue.AddKey((float)currentFrame, curveYValue.Evaluate(currentFrame));
                            curveYValue.SmoothTangents(num, 0);
                        }
                        else if (currentKeyY && isKeyY)
                        {
                            var ev = Event.current;

                            valueY = EditorGUI.FloatField(rectValueY, valueY);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (rectLabelY.Contains(ev.mousePosition))
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            float dis = ev.delta.x;
                                            valueY = valueY * 100.0f + dis * 10.0f;
                                            valueY = Mathf.Floor(Mathf.Abs(valueY)) / 100f * Mathf.Sign(valueY);
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelY, MouseCursor.SlideArrow);
                            curveYValue.MoveKey(indexY, new Keyframe((float)currentFrame, valueY));
                            curveYValue.SmoothTangents(indexY, 1);
                        }
                        else
                        {
                            EditorGUI.FloatField(rectValueY, curveYValue.Evaluate(currentFrame));
                        }
                    }
                    curveY.animationCurveValue = curveYValue;
                }

                var rectLabelZ = rect;
                rectLabelZ.x += rect.width * 0.5f + 2;
                rectLabelZ.width = 16;
                EditorGUI.LabelField(rectLabelZ, "Z");

                var curveZValue = curveZ.animationCurveValue;
                float valueZ = 0;
                int indexZ = -1;
                bool isKeyZ = false;
                for (int i = 0; i < curveZValue.length; i++)
                {
                    if (curveZValue[i].time == currentFrame)
                    {
                        valueZ = curveZValue[i].value;
                        indexZ = i;
                        isKeyZ = true;
                    }
                }
                var currentKeyZ = isKeyZ;

                var rectKeyZ = rect;
                rectKeyZ.width = 16;
                rectKeyZ.x += rect.width * 0.5f + 2;
                rectKeyZ.x += 12;
                rectKeyZ.y += 1;
                isKeyZ = EditorGUI.Toggle(rectKeyZ, new GUIContent(""), isKeyZ, isKeyStyle);

                var rectValueZ = rect;
                rectValueZ.width *= 0.25f;
                rectValueZ.width -= 32;
                rectValueZ.x += rect.width * 0.5f + 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueZ, curveZ, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyZ))
                    {
                        if (currentKeyZ && !isKeyZ)
                        {
                            EditorGUI.FloatField(rectValueZ, valueZ);
                            curveZValue.RemoveKey(indexZ);
                        }
                        else if (!currentKeyZ && isKeyZ)
                        {
                            EditorGUI.FloatField(rectValueZ, curveZValue.Evaluate(currentFrame));
                            var num = curveZValue.AddKey((float)currentFrame, curveZValue.Evaluate(currentFrame));
                            curveZValue.SmoothTangents(num, 0);
                        }
                        else if (currentKeyZ && isKeyZ)
                        {
                            var ev = Event.current;

                            valueZ = EditorGUI.FloatField(rectValueZ, valueZ);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (rectLabelZ.Contains(ev.mousePosition))
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            float dis = ev.delta.x;
                                            valueZ = valueZ * 100.0f + dis * 10.0f;
                                            valueZ = Mathf.Floor(Mathf.Abs(valueZ)) / 100f * Mathf.Sign(valueZ);
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelZ, MouseCursor.SlideArrow);
                            curveZValue.MoveKey(indexZ, new Keyframe((float)currentFrame, valueZ));
                            curveZValue.SmoothTangents(indexZ, 1);
                        }
                        else
                        {
                            EditorGUI.FloatField(rectValueZ, curveZValue.Evaluate(currentFrame));
                        }
                    }
                    curveZ.animationCurveValue = curveZValue;
                }


                var rectLabelW = rect;
                rectLabelW.x += rect.width * 0.75f + 2;
                rectLabelW.width = 16;
                EditorGUI.LabelField(rectLabelW, "Z");

                var curveWValue = curveW.animationCurveValue;
                float valueW = 0;
                int indexW = -1;
                bool isKeyW = false;
                for (int i = 0; i < curveWValue.length; i++)
                {
                    if (curveWValue[i].time == currentFrame)
                    {
                        valueW = curveWValue[i].value;
                        indexW = i;
                        isKeyW = true;
                    }
                }
                var currentKeyW = isKeyW;

                var rectKeyW = rect;
                rectKeyW.width = 16;
                rectKeyW.x += rect.width * 0.75f + 2;
                rectKeyW.x += 12;
                rectKeyW.y += 1;
                isKeyW = EditorGUI.Toggle(rectKeyW, new GUIContent(""), isKeyW, isKeyStyle);

                var rectValueW = rect;
                rectValueW.width *= 0.25f;
                rectValueW.width -= 32;
                rectValueW.x += rect.width * 0.75f + 32;

                if (isCurve.boolValue)
                {
                    EditorGUI.PropertyField(rectValueW, curveW, new GUIContent(""));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(!isKeyW))
                    {
                        if (currentKeyW && !isKeyW)
                        {
                            EditorGUI.FloatField(rectValueW, valueW);
                            curveWValue.RemoveKey(indexW);
                        }
                        else if (!currentKeyW && isKeyW)
                        {
                            EditorGUI.FloatField(rectValueW, curveWValue.Evaluate(currentFrame));
                            var num = curveWValue.AddKey((float)currentFrame, curveWValue.Evaluate(currentFrame));
                            curveWValue.SmoothTangents(num, 0);
                        }
                        else if (currentKeyW && isKeyW)
                        {
                            var ev = Event.current;

                            valueW = EditorGUI.FloatField(rectValueW, valueW);
                            int id = GUIUtility.GetControlID(FocusType.Passive);

                            if (ev.button == 0)
                            {
                                switch (ev.type)
                                {
                                    case EventType.MouseDown:
                                        if (rectLabelW.Contains(ev.mousePosition))
                                        {
                                            GUIUtility.hotControl = id;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.MouseDrag:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            float dis = ev.delta.x;
                                            valueW = valueW * 100.0f + dis * 10.0f;
                                            valueW = Mathf.Floor(Mathf.Abs(valueW)) / 100f * Mathf.Sign(valueW);
                                            dragControll = DragControll.Drag;
                                        }
                                        break;

                                    case EventType.MouseUp:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;

                                    case EventType.Ignore:
                                        if (GUIUtility.hotControl == id)
                                        {
                                            GUIUtility.hotControl = 0;
                                            dragControll = DragControll.Complete;
                                            ev.Use();
                                        }
                                        break;
                                }
                            }
                            EditorGUIUtility.AddCursorRect(rectLabelZ, MouseCursor.SlideArrow);
                            curveWValue.MoveKey(indexW, new Keyframe((float)currentFrame, valueW));
                            curveWValue.SmoothTangents(indexW, 1);
                        }
                        else
                        {
                            EditorGUI.FloatField(rectValueW, curveWValue.Evaluate(currentFrame));
                        }
                    }
                    curveW.animationCurveValue = curveWValue;
                }
            }
            else
            {
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.xMin += 10;
                EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.02f));
                value.vector4Value = EditorGUI.Vector4Field(rect, new GUIContent(""), value.vector4Value);
            }
        }
    }
}