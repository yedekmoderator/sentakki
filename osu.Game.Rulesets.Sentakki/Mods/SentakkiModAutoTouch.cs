using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Sentakki.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using System;

namespace osu.Game.Rulesets.Sentakki.Mods
{
    public class SentakkiModAutoTouch : Mod, IApplicableToDrawableHitObjects
    {
        public override string Name => "Auto Touch";
        public override string Acronym => "AT";
        public override IconUsage? Icon => OsuIcon.PlaystyleTouch;
        public override ModType Type => ModType.Automation;
        public override string Description => @"Focus on the laned notes. Touch screen notes will be completed automatically.";
        public override double ScoreMultiplier => .5f;
        public override bool Ranked => false;
        public override Type[] IncompatibleMods => base.IncompatibleMods.Append(typeof(ModAutoplay)).ToArray();

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            foreach (var d in drawables.OfType<DrawableSentakkiTouchHitObject>())
                d.AutoTouch = true;
        }
    }
}