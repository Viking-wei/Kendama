
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace HitBlockLevels.Editor
{
    [CustomPropertyDrawer(typeof(LevelBlockData))]
    public class LevelBlockData_Property : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var data = property.FindPropertyRelative("_block");
            var tree = new VisualElement
            {
                name = "LevelBlock"
            };
            tree.style.flexDirection = FlexDirection.Row;
            tree.style.borderBottomWidth = 1;
            tree.style.borderBottomColor = new Color(0.1f, 0.1f, 0.1f, 1);
            tree.style.borderBottomLeftRadius = tree.style.borderBottomRightRadius = 3;

            var originalValue = new PropertyField(property) { name = "BlockValue", visible = false };
            originalValue.style.width = 0;
            tree.Add(originalValue);

            var blockPanel = new VisualElement();
            blockPanel.style.flexDirection = FlexDirection.Column;
            blockPanel.style.width = 100;
            blockPanel.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1);
            blockPanel.style.borderTopLeftRadius = blockPanel.style.borderTopRightRadius
                    = blockPanel.style.borderBottomLeftRadius = blockPanel.style.borderBottomRightRadius = 3;

            var block = new ObjectField();
            block.BindProperty(data);
            block.style.maxHeight = 20;

            #region Thumbnail

            var thumbnail = new VisualElement() { name = "Thumbnail" };
            thumbnail.style.width = 64;
            thumbnail.style.height = 64;
            thumbnail.style.marginLeft = 18f;
            thumbnail.style.marginTop = 20f;
            thumbnail.style.borderBottomColor = thumbnail.style.borderRightColor
                    = new Color(0.3f, 0.3f, 0.3f, 1f);
            thumbnail.style.borderBottomLeftRadius = thumbnail.style.borderBottomRightRadius = thumbnail.style.borderTopLeftRadius
                                                    = thumbnail.style.borderTopRightRadius = 3;

            thumbnail.style.backgroundImage = AssetPreview.GetAssetPreview((property.boxedValue as LevelBlockData)._block);

            #endregion

            #region Block Data Fields

            var group = new VisualElement() { name = "Field Group" };
            group.style.flexDirection = FlexDirection.Column;
            group.style.width = StyleKeyword.Auto;

            var positionField = new Vector3Field("Position");
            positionField.name = "Position";
            positionField.BindProperty(property.FindPropertyRelative("Position"));
            positionField.RegisterValueChangedCallback<Vector3>((evt) => property.FindPropertyRelative("Position").vector3Value = evt.newValue);

            positionField.ElementAt(1).style.flexDirection = FlexDirection.Column;
            positionField.style.width = 250;
            var floatField = positionField.ElementAt(1).ElementAt(0);
            floatField.style.borderTopLeftRadius = floatField.style.borderBottomLeftRadius = 3;
            floatField.style.borderTopRightRadius = floatField.style.borderBottomRightRadius = 6;
            floatField.style.borderLeftColor = new Color(0.8f, 0.3f, 0.3f, 1f);
            floatField.style.borderLeftWidth = 3;
            floatField.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            floatField.style.marginBottom = 1.5f;

            floatField.ElementAt(0).style.marginLeft = 3;
            floatField = positionField.ElementAt(1).ElementAt(1);
            floatField.style.borderTopLeftRadius = floatField.style.borderBottomLeftRadius = 3;
            floatField.style.borderTopRightRadius = floatField.style.borderBottomRightRadius = 6;
            floatField.style.borderTopLeftRadius = floatField.style.borderBottomLeftRadius = 3;
            floatField.style.borderLeftColor = new Color(0.3f, 0.8f, 0.3f, 1f);
            floatField.style.borderLeftWidth = 3;
            floatField.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            floatField.style.marginBottom = 1.5f;

            floatField = positionField.ElementAt(1).ElementAt(2);
            floatField.style.borderTopLeftRadius = floatField.style.borderBottomLeftRadius = 3;
            floatField.style.borderTopRightRadius = floatField.style.borderBottomRightRadius = 6;
            floatField.style.borderLeftColor = new Color(0.3f, 0.3f, 0.9f, 1f);
            floatField.style.borderLeftWidth = 3;
            floatField.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);


            var rotationField = new FloatField("Rotation");
            rotationField.name = "Rotation";
            rotationField.BindProperty(property.FindPropertyRelative("Rotation"));

            rotationField.style.borderBottomColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            rotationField.style.borderBottomWidth = 1;

            var easeInField = new IntegerField("In Time");
            easeInField.name = "InTime";
            easeInField.BindProperty(property.FindPropertyRelative("EaseInTime"));


            var easeOutField = new IntegerField("Out Time");
            easeOutField.name = "OutTime";
            easeOutField.BindProperty(property.FindPropertyRelative("EaseOutTime"));


            group.Add(positionField);
            group.Add(rotationField);
            group.Add(easeInField);
            group.Add(easeOutField);

            #endregion

            blockPanel.Add(block);
            blockPanel.Add(thumbnail);
            blockPanel.style.paddingRight = 15;

            tree.Add(blockPanel);
            tree.Add(group);

            return tree;
        }
    }
}
