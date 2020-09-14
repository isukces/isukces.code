using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmySystemColorDynamicResource : IAmmyCodePieceConvertible
    {
        public AmmySystemColorDynamicResource(SystemColorsKeys key) => Key = key;

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            // resource dyn System.Windows.SystemColors.WindowBrushKey
            const string systemWindows = "System.Windows";
            var          name          = $"SystemColors.{Key}Key";
            if (ctx.FullNamespaces || !ctx.NamespaceProvider.Namespaces.Contains(systemWindows))
                name = systemWindows + "." + name;
            return new SimpleAmmyCodePiece($"resource dyn {name}");
        }

        public SystemColorsKeys Key { get; set; }
    }

    public enum SystemColorsKeys
    {
        ActiveBorderColor,
        ActiveCaptionColor,
        ActiveCaptionTextColor,
        AppWorkspaceColor,
        ControlColor,
        ControlDarkColor,
        ControlDarkDarkColor,
        ControlLightColor,
        ControlLightLightColor,
        ControlTextColor,
        DesktopColor,
        GradientActiveCaptionColor,
        GradientInactiveCaptionColor,
        GrayTextColor,
        HighlightColor,
        HighlightTextColor,
        HotTrackColor,
        InactiveBorderColor,
        InactiveCaptionColor,
        InactiveCaptionTextColor,
        InfoColor,
        InfoTextColor,
        MenuColor,
        MenuBarColor,
        MenuHighlightColor,
        MenuTextColor,
        ScrollBarColor,
        WindowColor,
        WindowFrameColor,
        WindowTextColor,
        ActiveBorderBrush,
        ActiveCaptionBrush,
        ActiveCaptionTextBrush,
        AppWorkspaceBrush,
        ControlBrush,
        ControlDarkBrush,
        ControlDarkDarkBrush,
        ControlLightBrush,
        ControlLightLightBrush,
        ControlTextBrush,
        DesktopBrush,
        GradientActiveCaptionBrush,
        GradientInactiveCaptionBrush,
        GrayTextBrush,
        HighlightBrush,
        HighlightTextBrush,
        HotTrackBrush,
        InactiveBorderBrush,
        InactiveCaptionBrush,
        InactiveCaptionTextBrush,
        InfoBrush,
        InfoTextBrush,
        MenuBrush,
        MenuBarBrush,
        MenuHighlightBrush,
        MenuTextBrush,
        ScrollBarBrush,
        WindowBrush,
        WindowFrameBrush,
        WindowTextBrush,
        InactiveSelectionHighlightBrush,
        InactiveSelectionHighlightTextBrush
    }
}