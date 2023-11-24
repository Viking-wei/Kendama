using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

namespace HitBlockLevels.Editor
{
    [CustomEditor(typeof(LevelRunner))]
    [CanEditMultipleObjects]
    public class LevelRunner_Editor : UnityEditor.Editor
    {
        public Texture2D ButtonTexture;
        private SliderInt _progressBar;
        private Vector3 _handlePosition = Vector3.zero;
        private Quaternion _handleRotation = Quaternion.identity;
        private GameObject _goInEdit;
        private List<GameObject> _goAtCurrentBeat = new List<GameObject>();
        private Vector3Field _positionProperty;
        private FloatField _rotationProperty;
        private ObjectField _objectProperty;
        private IntegerField _inTimeProperty;
        private int _blockIndex = -1;
        private VisualElement _root;

        private int _currentEditingBeat = 0;
        private IntegerField _beatLabel;
        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();

            _root.Add(new PropertyField(serializedObject.FindProperty("BPM")));
            var levelBeatsProperty = new IntegerField("Level Beats");
            levelBeatsProperty.BindProperty(serializedObject.FindProperty("LevelBeats"));
            levelBeatsProperty.tooltip = "Total beats count for a the whole level.";

            #region Beat Group

            var beatGroup = new VisualElement();
            beatGroup.style.backgroundColor = new Color(0.2f, 0.2f, 0.3f, 1f);
            beatGroup.style.paddingRight = 5;
            beatGroup.style.marginTop = 5;
            beatGroup.style.marginBottom = 3;
            beatGroup.style.borderBottomLeftRadius = beatGroup.style.borderBottomRightRadius =
                    beatGroup.style.borderTopLeftRadius = beatGroup.style.borderTopRightRadius = 4;
            beatGroup.style.borderBottomColor = beatGroup.style.borderLeftColor =
                    beatGroup.style.borderRightColor = beatGroup.style.borderTopColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            beatGroup.style.borderBottomWidth = beatGroup.style.borderTopWidth =
                    beatGroup.style.borderLeftWidth = beatGroup.style.borderRightWidth = 1.5f;

            _beatLabel = new IntegerField("Current Beat")
            {
                label = "Current Beat 0"
            };
            _beatLabel.RegisterValueChangedCallback<int>(
                (evt) =>
                {
                    _currentEditingBeat = evt.newValue;
                    _progressBar.SetValueWithoutNotify(evt.newValue);
                    OnCurrentBeatChanged();
                }
            );
            _progressBar = new SliderInt("Time Line Progress", 0, 1);
            _progressBar.RegisterValueChangedCallback<int>(
                (evt) =>
                {
                    _currentEditingBeat = evt.newValue;
                    _beatLabel.SetValueWithoutNotify(evt.newValue);
                    OnCurrentBeatChanged();
                });

            var noticeLabel = new Label("* this section is only for editing *");
            noticeLabel.style.marginTop = 2;
            noticeLabel.style.unityFontStyleAndWeight = FontStyle.BoldAndItalic;

            var addButton = new Button() { name = "AddNewButton", text = "Add Block At Current Beat" };
            addButton.clicked += () => AddBlockAtCurrentBeat();
            beatGroup.Add(noticeLabel);
            beatGroup.Add(_progressBar);
            beatGroup.Add(_beatLabel);
            beatGroup.Add(addButton);
            #endregion

            levelBeatsProperty.RegisterValueChangedCallback<int>(
                (evt) => _progressBar.highValue = evt.newValue);
            _root.Add(levelBeatsProperty);
            _root.Add(beatGroup);

            var blockData = new PropertyField(serializedObject.FindProperty("LevelBlocks"));
            _root.Add(blockData);
            _root.schedule.Execute(AddButtons).StartingIn(0);
            return _root;
        }

        private void AddButtons()
        {
            var blocks = _root.Query<VisualElement>("LevelBlock").ToList();
            foreach (var block in blocks)
            {
                var button = new Button() { text = "Edit" };
                button.style.backgroundImage = ButtonTexture;
                button.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
                button.style.unityTextAlign = TextAnchor.LowerCenter;
                button.style.marginLeft = 3;

                button.clicked += () =>
                {
                    _objectProperty = block.Query<ObjectField>().First();
                    _positionProperty = block.Query<Vector3Field>("Position").First();
                    _rotationProperty = block.Query<FloatField>("Rotation").First();
                    _inTimeProperty = block.Query<IntegerField>("InTime").First();
                    _blockIndex = blocks.IndexOf(block);
                    SwitchEditingObj();
                };

                block.Add(button);
            }
        }

        private void SwitchEditingObj()
        {
            _beatLabel.value = _inTimeProperty.value;
            if (_goInEdit != null) DestroyImmediate(_goInEdit);
            _goInEdit = Instantiate(_objectProperty.value as GameObject, (target as LevelRunner).transform);
            _goInEdit.name = $"<In Level Editing Mode> {_goInEdit.name}";

            _handlePosition = _positionProperty.value;
            _goInEdit.transform.position = _handlePosition;
            _handleRotation = Quaternion.Euler(0, 0, _rotationProperty.value);
            _goInEdit.transform.rotation = _handleRotation;
        }

        private void OnCurrentBeatChanged()
        {
            foreach (var go in _goAtCurrentBeat)
            {
                DestroyImmediate(go);
            }
            _goAtCurrentBeat.Clear();
            var lr = target as LevelRunner;
            for (int i = 0; i < lr.LevelBlocks.Count; i++)
            {
                if (_blockIndex == i) continue;
                if (lr.LevelBlocks[i].EaseInTime <= _currentEditingBeat && lr.LevelBlocks[i].EaseOutTime >= _currentEditingBeat)
                {
                    var go = Instantiate(lr.LevelBlocks[i]._block, (target as LevelRunner).transform);
                    go.name = $"<In Level Editing Mode> {go.name}";
                    go.transform.position = lr.LevelBlocks[i].Position;
                    go.transform.rotation = Quaternion.Euler(0, 0, lr.LevelBlocks[i].Rotation);
                    _goAtCurrentBeat.Add(go);
                }
            }
        }

        private void AddBlockAtCurrentBeat()
        {
            var newBlock = new LevelBlockData() { EaseInTime = _currentEditingBeat, EaseOutTime = _currentEditingBeat + 1 };
            (target as LevelRunner).LevelBlocks.Add(newBlock);
        }

        private void OnSceneGUI()
        {
            if (_goInEdit == null) return;

            var tempHandlePosition = Handles.PositionHandle(_handlePosition, Quaternion.identity);
            var tempHandleRotation = Quaternion.Euler(0, 0, Handles.RotationHandle(_handleRotation, _handlePosition).eulerAngles.z);
            _goInEdit.transform.position = _handlePosition;
            _goInEdit.transform.rotation = _handleRotation;

            if (Vector3.Distance(tempHandlePosition, _handlePosition) > 0)
                _positionProperty.value = tempHandlePosition;
            else
                if (Vector3.Distance(_positionProperty.value, tempHandlePosition) > 0)
                tempHandlePosition = _positionProperty.value;

            if (Mathf.Abs(tempHandleRotation.eulerAngles.z - _handleRotation.eulerAngles.z) > 0)
                _rotationProperty.value = tempHandleRotation.eulerAngles.z;
            else
                if (Mathf.Abs(tempHandleRotation.eulerAngles.z - _rotationProperty.value) > 0)
                tempHandleRotation = Quaternion.Euler(0, 0, _rotationProperty.value);

            _handlePosition = tempHandlePosition;
            _handleRotation = tempHandleRotation;
        }

        private void OnDisable()
        {
            DestroyImmediate(_goInEdit);
            foreach (var go in _goAtCurrentBeat)
            {
                DestroyImmediate(go);
            }
            _goAtCurrentBeat.Clear();
        }
    }
}
