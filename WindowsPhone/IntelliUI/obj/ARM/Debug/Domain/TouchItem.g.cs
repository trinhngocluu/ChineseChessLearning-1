﻿#pragma checksum "C:\Users\BT\Documents\GitHub\ChineseChessLearning\WindowsPhone\IntelliUI\Domain\TouchItem.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2DAE3AAE8365B7661FA09F969AA8BE87"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace IntelliUI.Domain {
    
    
    public partial class TouchItem : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid LayoutChildRoot;
        
        internal System.Windows.Controls.Image imgPiece;
        
        internal System.Windows.Controls.Image imgMask;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/IntelliUI;component/Domain/TouchItem.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LayoutChildRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutChildRoot")));
            this.imgPiece = ((System.Windows.Controls.Image)(this.FindName("imgPiece")));
            this.imgMask = ((System.Windows.Controls.Image)(this.FindName("imgMask")));
        }
    }
}

