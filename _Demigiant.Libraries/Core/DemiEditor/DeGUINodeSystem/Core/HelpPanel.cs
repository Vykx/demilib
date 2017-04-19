﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2017/04/19 12:31
// License Copyright (c) Daniele Giardini

using System.Text;
using DG.DemiEditor.Internal;
using DG.DemiLib;
using UnityEngine;

namespace DG.DemiEditor.DeGUINodeSystem.Core
{
    internal static class HelpPanel
    {
        const int _OuterPadding = 6;
        const int _InnerPadding = 8;
        static readonly string _Content;
        static readonly Styles _Styles = new Styles();

        #region Constructor

        static HelpPanel()
        {
            StringBuilder strb = new StringBuilder();

            strb.Append("<size=20><color=#0e98f8>HELP PANEL (F1)</color></size>")
                // MOUSE
                .AppendHeader("MOUSE")
                .AppendKey("LMB").AppendTarget("background").AppendDescription("Deselect all")
                .AppendKey("LMB").AppendTarget("node draggable area").AppendDescription("Select node and deselect all others")
                .AppendKey("LMB + SHIFT").AppendTarget("node draggable area").AppendDescription("Add node to selection")
                .AppendKey("LMB-Drag").AppendTarget("background").AppendDescription("Draw new nodes selection rect - <i>clears previous selection</i>")
                .AppendKey("LMB-Drag + SHIFT").AppendTarget("background").AppendDescription("Draw new nodes selection rect - <i>will add nodes to current selection</i>")
                .AppendKey("LMB-Drag").AppendTarget("node draggable area").AppendDescription("Drag selected nodes")
                .AppendKey("LMB-Drag + CTRL").AppendTarget("node draggable area").AppendDescription("Drag new connection from node - <i>if node allows it</i>")
                .AppendKey("RMB").AppendDescription("Context menu")
                .AppendKey("MMB-Drag").AppendDescription("Area panning")
                .AppendKey("Scrollwheel").AppendDescription("Zoom in/out")
                // KEYBOARD
                .AppendHeader("KEYBOARD")
                .AppendKey("F1").AppendDescription("Open/close this panel")
                .AppendKey("CTRL + A").AppendDescription("Select all nodes")
                ;

            _Content = strb.ToString();
        }

        #endregion

        #region GUI Methods

        // Only called during Repaint
        public static void Draw(NodeProcess process)
        {
            _Styles.Init();
            Matrix4x4 prevGuiMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(process.position.min - process.position.min / process.guiScale, Quaternion.identity, Vector3.one);

            // Background (adapt area to counter GUI scale)
            Rect area = new Rect(
                process.relativeArea.x, process.relativeArea.y,
                process.relativeArea.width * process.guiScale, process.relativeArea.height * process.guiScale
            );
            area = area.Shift(_OuterPadding, _OuterPadding, -_OuterPadding * 2, -_OuterPadding * 2);
            GUI.DrawTexture(area, DeStylePalette.blackSquareAlpha80);
            using (new DeGUI.ColorScope(new DeSkinColor(0.3f))) GUI.Box(area, "", DeGUI.styles.box.outline01);

            // Content
            Rect contentR = area.Shift(_InnerPadding, _InnerPadding, -_InnerPadding * 2, -_InnerPadding * 2);
            GUI.Label(contentR, _Content, _Styles.contentLabel);

            GUI.matrix = prevGuiMatrix;
        }

        #endregion

        #region Helpers

        static StringBuilder AppendHeader(this StringBuilder strb, string value)
        {
            return strb.Append("\n\n<size=12><color=#0e98f8><b>").Append(value).Append("</b></color></size>");
        }

        static StringBuilder AppendKey(this StringBuilder strb, string value)
        {
            return strb.Append("\n<color=#ff6540>").Append(value).Append("</color>");
        }

        static StringBuilder AppendTarget(this StringBuilder strb, string value)
        {
            return strb.Append("<color=#0e98f8> ▶ </color><color=#ffb407>").Append(value).Append("</color>");
        }

        static StringBuilder AppendDescription(this StringBuilder strb, string value)
        {
            return strb.Append("<color=#0e98f8> ▶ </color>").Append(value);
        }

        #endregion

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        class Styles
        {
            public GUIStyle boxOutline, contentLabel;
            bool _initialized;

            public void Init()
            {
                if (_initialized) return;

                _initialized = true;
                contentLabel = new GUIStyle(GUI.skin.label).Add(Color.white, Format.RichText, Format.WordWrap, TextAnchor.UpperLeft);
            }
        }
    }
}