using System.Collections.Generic;

namespace RobustHaven.IntegrationTests.Framework
{
    public class Sequence : Composite
    {
        private readonly Component _left;
        private readonly Component _right;

        public Sequence(Component left, Component right)
        {
            _left = left;
            _right = right;
        }


        public override List<Component> Children
        {
            get { return new List<Component> {_left, _right}; }
        }
    }
}