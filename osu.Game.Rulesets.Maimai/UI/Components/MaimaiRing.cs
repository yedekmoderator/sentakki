﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Maimai.Configuration;
using osuTK;
using osuTK.Graphics;
using System;
using osu.Game.Beatmaps;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Maimai.UI.Components
{
    public class MaimaiRing : CompositeDrawable
    {
        private readonly Container ring;
        private readonly Container spawnIndicator;

        private DifficultyRating? difficultyRating;

        public MaimaiRing(DifficultyRating? rating = null)
        {
            difficultyRating = rating;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChildren = new Drawable[]
            {
                ring = new Container
                {
                    Scale = Vector2.Zero,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(MaimaiPlayfield.RingSize),
                    FillAspectRatio = 1,
                    FillMode = FillMode.Fit,
                    Alpha = 0,
                    Children = new Drawable[]{
                        new CircularContainer{
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Masking = true,
                            BorderThickness = 8.35f,
                            BorderColour = Color4.Gray,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Alpha = 0,
                                    AlwaysPresent = true,
                                },
                            }
                        },
                        new Container{
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(1),
                            Child = new CircularContainer{
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Masking = true,
                                BorderThickness = 6,
                                BorderColour = Color4.White,
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Alpha = 0,
                                    AlwaysPresent = true,
                                },
                            },
                        },
                        new CircularContainer{
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Masking = true,
                            BorderThickness = 2,
                            BorderColour = Color4.Gray,
                            Child = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Alpha = 0,
                                AlwaysPresent = true,
                            },
                        },
                    }
                },
                spawnIndicator = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0
                }
            };

            // Add dots to the actual ring
            foreach (float pathAngle in MaimaiPlayfield.PathAngles)
            {
                ring.Add(new CircularContainer
                {
                    Size = new Vector2(MaimaiPlayfield.DotSize),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.None,
                    Masking = true,
                    BorderColour = Color4.Gray,
                    BorderThickness = 2,
                    Position = new Vector2(-(MaimaiPlayfield.IntersectDistance * (float)Math.Cos((pathAngle + 90f) * (float)(Math.PI / 180))), -(MaimaiPlayfield.IntersectDistance * (float)Math.Sin((pathAngle + 90f) * (float)(Math.PI / 180)))),
                    Child = new Box
                    {
                        AlwaysPresent = true,
                        RelativeSizeAxes = Axes.Both,
                    }
                });

                spawnIndicator.Add(new CircularContainer
                {
                    Size = new Vector2(16, 8),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,
                    BorderColour = Color4.Gray,
                    BorderThickness = 2f,
                    Rotation = pathAngle,
                    Position = new Vector2(-(MaimaiPlayfield.NoteStartDistance * (float)Math.Cos((pathAngle + 90f) * (float)(Math.PI / 180))), -(MaimaiPlayfield.NoteStartDistance * (float)Math.Sin((pathAngle + 90f) * (float)(Math.PI / 180)))),
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                });
            }
        }

        public Bindable<float> ringOpacity = new Bindable<float>(1);
        public Bindable<bool> noteStartIndicators = new Bindable<bool>(false);
        Bindable<bool> diffBasedColor = new Bindable<bool>(false);

        [BackgroundDependencyLoader(true)]
        private void load(MaimaiRulesetConfigManager settings, OsuColour colours)
        {
            settings?.BindWith(MaimaiRulesetSettings.RingOpacity, ringOpacity);
            ringOpacity.BindValueChanged(opacity => this.Alpha = opacity.NewValue, true);

            settings?.BindWith(MaimaiRulesetSettings.ShowNoteStartIndicators, noteStartIndicators);
            noteStartIndicators.BindValueChanged(opacity => spawnIndicator.FadeTo(Convert.ToSingle(opacity.NewValue), 200));

            settings?.BindWith(MaimaiRulesetSettings.DiffBasedRingColor, diffBasedColor);
            diffBasedColor.BindValueChanged(enabled =>
            {
                if (enabled.NewValue)
                {
                    if (difficultyRating is null)
                        difficultyRating = DifficultyRating.Normal;

                    this.FadeColour(colours.ForDifficultyRating(difficultyRating.Value, true), 200);
                }
                else
                {
                    this.FadeColour(Color4.White, 200);
                }
            });
        }
        protected override void LoadComplete()
        {
            ring.FadeIn(1000, Easing.OutElasticQuarter).ScaleTo(1, 1000, Easing.OutElasticQuarter);
            noteStartIndicators.TriggerChange();
            diffBasedColor.TriggerChange();
        }
    }
}
