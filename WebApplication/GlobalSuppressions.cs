using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member",
        Target = "WebApplication.ApiManager.BeyondVerbal.#authRequest(System.String,System.Byte[])")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member",
        Target = "WebApplication.ApiManager.BeyondVerbal.#CreateWebRequest(System.String,System.Byte[],System.String)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type",
        Target = "WebApplication.ApiManager.MicrosoftSpeechToText")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type",
        Target = "WebApplication.ApiManager.YoutubeSoundAnalisys")]
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.