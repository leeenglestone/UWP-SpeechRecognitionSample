﻿#pragma checksum "C:\Development\UWP-SpeechRecognitionSample\UwpSpeechRecognitionSampleSolution\UwpSpeechRecognitionSample.UwpApplication\UserControls\SpeechRecognitionControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "206B4CD7DCC858B143E961C4C9AE8BCF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UwpSpeechRecognitionSample.UwpApplication.UserControls
{
    partial class SpeechRecognitionControl : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.ListeningStateToTextConverter = (global::UwpSpeechRecognitionSample.UwpApplication.Converters.ListeningStateToTextConverter)(target);
                }
                break;
            case 2:
                {
                    this.BtnStopListening = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 34 "..\..\..\UserControls\SpeechRecognitionControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.BtnStopListening).Click += this.BtnStopListening_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.BtnStartListening = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 35 "..\..\..\UserControls\SpeechRecognitionControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.BtnStartListening).Click += this.BtnStartListening_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

