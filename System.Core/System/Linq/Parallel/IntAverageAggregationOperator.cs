﻿namespace System.Linq.Parallel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal sealed class IntAverageAggregationOperator : InlinedAggregationOperator<int, Pair<long, long>, double>
    {
        internal IntAverageAggregationOperator(IEnumerable<int> child) : base(child)
        {
        }

        protected override QueryOperatorEnumerator<Pair<long, long>, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<int, TKey> source, object sharedData, CancellationToken cancellationToken)
        {
            return new IntAverageAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
        }

        protected override double InternalAggregate(ref Exception singularExceptionToThrow)
        {
            using (IEnumerator<Pair<long, long>> enumerator = this.GetEnumerator(3, true))
            {
                if (!enumerator.MoveNext())
                {
                    singularExceptionToThrow = new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
                    return 0.0;
                }
                Pair<long, long> current = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    Pair<long, long> pair2 = enumerator.Current;
                    current.First += pair2.First;
                    Pair<long, long> pair3 = enumerator.Current;
                    current.Second += pair3.Second;
                }
                return (((double) current.First) / ((double) current.Second));
            }
        }

        private class IntAverageAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Pair<long, long>>
        {
            private QueryOperatorEnumerator<int, TKey> m_source;

            internal IntAverageAggregationOperatorEnumerator(QueryOperatorEnumerator<int, TKey> source, int partitionIndex, CancellationToken cancellationToken) : base(partitionIndex, cancellationToken)
            {
                this.m_source = source;
            }

            protected override void Dispose(bool disposing)
            {
                this.m_source.Dispose();
            }

            protected override bool MoveNextCore(ref Pair<long, long> currentElement)
            {
                long first = 0L;
                long second = 0L;
                QueryOperatorEnumerator<int, TKey> source = this.m_source;
                int num3 = 0;
                TKey currentKey = default(TKey);
                if (!source.MoveNext(ref num3, ref currentKey))
                {
                    return false;
                }
                int num4 = 0;
                do
                {
                    if ((num4++ & 0x3f) == 0)
                    {
                        CancellationState.ThrowIfCanceled(base.m_cancellationToken);
                    }
                    first += num3;
                    second += 1L;
                }
                while (source.MoveNext(ref num3, ref currentKey));
                currentElement = new Pair<long, long>(first, second);
                return true;
            }
        }
    }
}

