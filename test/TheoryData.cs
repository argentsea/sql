using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ArgentSea.Sql.Test
{
    public abstract class TheoryData : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>();

        protected void AddRow(params object[] values)
        {
            data.Add(values);
        }
        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
