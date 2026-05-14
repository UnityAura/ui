// Copyright (c) Supernova Technologies LLC
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Nova.Internal
{
    /// <summary>
    /// Merges Nova-configured TMP font fallbacks into <see cref="TMP_Settings.fallbackFontAssets"/> so missing
    /// glyphs resolve after each primary font's own fallback table, without requiring code in consuming projects.
    /// Does not call <see cref="TMP_Settings.LoadDefaultSettings"/>; touching TMP before its Resources bootstrap
    /// finishes (e.g. while the TMP Essential Resources importer runs) can contribute to confusing editor state.
    /// </summary>
    internal static class TmpGlobalFallbackSynchronizer
    {
        private static readonly List<TMP_FontAsset> LastInjectedNovaFonts = new List<TMP_FontAsset>();

        /// <summary>
        /// Reconciles TMP's global fallback list: removes fonts previously injected from the last Nova snapshot,
        /// then appends the current Nova list (deduplicated against the remaining list).
        /// </summary>
        public static void Sync(TMP_FontAsset[] novaGlobalFallbacks)
        {
            try
            {
                // Do not call TMP_Settings.LoadDefaultSettings() here. During TMP Essential Resources import,
                // forcing settings load can race Unity's TMP_PackageResourceImporter (known spurious
                // "Essential Resources are missing" on window close despite a successful import).
                if (TMP_Settings.instance == null)
                {
                    return;
                }

                List<TMP_FontAsset> list = TMP_Settings.fallbackFontAssets;
                if (list == null)
                {
                    list = new List<TMP_FontAsset>();
                    TMP_Settings.fallbackFontAssets = list;
                }

                for (int i = 0; i < LastInjectedNovaFonts.Count; i++)
                {
                    list.Remove(LastInjectedNovaFonts[i]);
                }

                LastInjectedNovaFonts.Clear();

                if (novaGlobalFallbacks == null)
                {
                    return;
                }

                for (int i = 0; i < novaGlobalFallbacks.Length; i++)
                {
                    TMP_FontAsset font = novaGlobalFallbacks[i];
                    if (font == null)
                    {
                        continue;
                    }

                    if (!list.Contains(font))
                    {
                        list.Add(font);
                        LastInjectedNovaFonts.Add(font);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Nova was unable to sync TextMesh Pro global fallback fonts: {ex.Message}");
            }
        }
    }
}
