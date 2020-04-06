﻿using NUnit.Framework;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Maimai.UI.Components;
using osu.Game.Tests.Visual;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Maimai.Tests.UI
{
    [TestFixture]
    public class TestSceneMaimaiRing : OsuTestScene
    {
        private MaimaiRing ring;
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(MaimaiRing)
        };

        public TestSceneMaimaiRing()
        {
            AddStep("Clear test", () =>
            {
                Clear();
                Add(new Box
                {
                    RelativeSizeAxes = Framework.Graphics.Axes.Both
                });
            });

            AddStep("Create Ring", () => Add(ring = new MaimaiRing()));
            AddUntilStep("Ring loaded", () => ring.IsLoaded && ring.Alpha == 1);
            AddToggleStep("Toggle notestart Indicators", b => ring.noteStartIndicators.Value = b);
            AddSliderStep<float>("Test opacity", 0, 1, 1, f => { if (ring != null) ring.ringOpacity.Value = f; });
        }
    }
}
