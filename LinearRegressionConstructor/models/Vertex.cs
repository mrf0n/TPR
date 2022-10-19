using System.Collections.Generic;
using System.Linq;

namespace LinearRegressionConstructor.models
{
    internal class Vertex
    {
        private int num;
        private List<int> vertices;

        public int Num
        {
            get => num;
            set => num = value;
        }
        public Vertex(int num)
        {
            this.num = num;
            vertices = new List<int>();
        }
        public void AddVertex(int v) => vertices.Add(v);
        public bool IsRelated(int v) => vertices.Where(x => x == v).Count() != 0;
    }
}
