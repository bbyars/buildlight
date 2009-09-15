using System.Collections.Generic;

namespace buildlight
{
    public class CompositeColor : IColor
    {
        private readonly List<IColor> colors;

        public CompositeColor(params IColor[] colors)
        {
            this.colors = new List<IColor>(colors);
        }

        public void Off()
        {
            colors.ForEach(color => color.Off());
        }

        public void On()
        {
            colors.ForEach(color => color.On());
        }

        public void Flash()
        {
            colors.ForEach(color => color.Flash());
        }
    }
}
