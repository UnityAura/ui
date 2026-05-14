// Copyright (c) Supernova Technologies LLC
using Nova;
using UnityEditor;

namespace Nova.Editor
{
    /// <summary>
    /// Ensures Nova-managed TMP global fallbacks are applied in the editor without entering play mode
    /// (after domain reload and once Nova settings are loadable). Runs after a short delay so we do not run
    /// while TMP's own Essential Resources importer is tearing down (Unity can log a false "resources missing"
    /// error on that window's close even when import succeeded).
    /// </summary>
    [InitializeOnLoad]
    internal static class TmpGlobalFallbackEditorBootstrap
    {
        static TmpGlobalFallbackEditorBootstrap()
        {
            EditorApplication.delayCall += SyncOnDelayCall;
        }

        private static void SyncOnDelayCall()
        {
            EditorApplication.delayCall -= SyncOnDelayCall;
            EditorApplication.delayCall += SyncAfterTmpBootstrapFrame;
        }

        private static void SyncAfterTmpBootstrapFrame()
        {
            EditorApplication.delayCall -= SyncAfterTmpBootstrapFrame;
            NovaSettings.ApplyTmpGlobalFontFallbacks();
        }
    }
}
