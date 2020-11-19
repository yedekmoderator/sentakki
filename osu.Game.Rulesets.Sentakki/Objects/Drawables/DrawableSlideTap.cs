﻿using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Sentakki.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Sentakki.Objects.Drawables
{
    public class DrawableSlideTap : DrawableTap
    {
        private DrawableSlide slide;

        public DrawableSlideTap() : this(null, null) { }

        public DrawableSlideTap(SlideTap hitObject, DrawableSlide slide)
            : base(hitObject)
        {
            this.slide = slide;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void OnParentReceived(DrawableHitObject parent)
        {
            base.OnParentReceived(parent);
            slide = (DrawableSlide)parent;
        }

        protected override Drawable CreateTapRepresentation() => new SlideTapPiece();

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            var note = TapVisual as SlideTapPiece;

            if (HitObject.NestedHitObjects.Count > 2)
                note.SecondStar.Alpha = 1;
            else
                note.SecondStar.Alpha = 0;


            double spinDuration = ((Slide)slide.HitObject).SlideInfoList.FirstOrDefault().Duration;
            note.Stars.Spin(spinDuration, RotationDirection.CounterClockwise, 0).Loop();
        }
    }
}
