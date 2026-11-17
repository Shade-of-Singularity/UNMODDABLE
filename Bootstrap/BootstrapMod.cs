namespace Unmoddable;

public sealed class BootstrapMod : Mod<BootstrapMod>
{
    public override string Identifier => "sog.unbeatable.bootstrap";
    public override string[] Dependencies => [];
    public override string Name => "BOOTSTRAP";
    public override string Prefix => "[BOT]";
    public override string Description => "UNMODDABLE's mod loader";
    public override bool AlwaysActive => true;
}
