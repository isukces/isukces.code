using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmySystemColorResource : IAmmyCodePieceConvertible
    {
        public AmmySystemColorResource(SystemColorsKeys key) => Key = key;

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            // resource dyn System.Windows.SystemColors.WindowBrushKey
            const string systemWindows = "System.Windows";
            var          name          = "SystemColors." + Key;
            if (!ctx.NamespaceProvider.Namespaces.Contains(systemWindows))
                name = systemWindows + "." + name;
            return new SimpleAmmyCodePiece($"resource dyn {name}");
        }

        public SystemColorsKeys Key { get; set; }
    }

    public enum SystemColorsKeys
    {
        ActiveBorderColorKey,
        ActiveCaptionColorKey,
        ActiveCaptionTextColorKey,
        AppWorkspaceColorKey,
        ControlColorKey,
        ControlDarkColorKey,
        ControlDarkDarkColorKey,
        ControlLightColorKey,
        ControlLightLightColorKey,
        ControlTextColorKey,
        DesktopColorKey,
        GradientActiveCaptionColorKey,
        GradientInactiveCaptionColorKey,
        GrayTextColorKey,
        HighlightColorKey,
        HighlightTextColorKey,
        HotTrackColorKey,
        InactiveBorderColorKey,
        InactiveCaptionColorKey,
        InactiveCaptionTextColorKey,
        InfoColorKey,
        InfoTextColorKey,
        MenuColorKey,
        MenuBarColorKey,
        MenuHighlightColorKey,
        MenuTextColorKey,
        ScrollBarColorKey,
        WindowColorKey,
        WindowFrameColorKey,
        WindowTextColorKey,
        ActiveBorderBrushKey,
        ActiveCaptionBrushKey,
        ActiveCaptionTextBrushKey,
        AppWorkspaceBrushKey,
        ControlBrushKey,
        ControlDarkBrushKey,
        ControlDarkDarkBrushKey,
        ControlLightBrushKey,
        ControlLightLightBrushKey,
        ControlTextBrushKey,
        DesktopBrushKey,
        GradientActiveCaptionBrushKey,
        GradientInactiveCaptionBrushKey,
        GrayTextBrushKey,
        HighlightBrushKey,
        HighlightTextBrushKey,
        HotTrackBrushKey,
        InactiveBorderBrushKey,
        InactiveCaptionBrushKey,
        InactiveCaptionTextBrushKey,
        InfoBrushKey,
        InfoTextBrushKey,
        MenuBrushKey,
        MenuBarBrushKey,
        MenuHighlightBrushKey,
        MenuTextBrushKey,
        ScrollBarBrushKey,
        WindowBrushKey,
        WindowFrameBrushKey,
        WindowTextBrushKey,
        InactiveSelectionHighlightBrushKey,
        InactiveSelectionHighlightTextBrushKey
    }
}