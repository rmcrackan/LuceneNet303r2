using System;
using System.Collections.Generic;
using FluentAssertions;

namespace LuceneNet303r2Tests
{
	public class SearchResults
	{
		public int QtyHits { get; }
		public List<ResultHit> Hits { get; }

		public SearchResults(int qtyHits, List<ResultHit> hits)
		{
			QtyHits = qtyHits;
			Hits = hits;
		}
	}

	public class ResultHit
	{
		public float Score { get; }

		public List<(string name, string value)> HitFields { get; }

		public ResultHit(float score, List<(string name, string value)> hitFields)
		{
			Score = score;
			HitFields = hitFields;
		}
	}

	public static class ResultsCompare
	{
		public static void AssertEquiv(this SearchResults expected, SearchResults actual)
		{
			actual.QtyHits.Should().Be(expected.QtyHits);
			actual.Hits.Count.Should().Be(expected.Hits.Count);

			for (var i = 0; i < actual.Hits.Count; i++)
			{
				actual.Hits[i].Score.Should().Be(expected.Hits[i].Score);

				actual.Hits[i].HitFields.Count.Should().Be(expected.Hits[i].HitFields.Count);

				for (var j = 0; j < actual.Hits[i].HitFields.Count; j++)
				{
					actual.Hits[i].HitFields[j].name.Should().Be(expected.Hits[i].HitFields[j].name);
					actual.Hits[i].HitFields[j].value.Should().Be(expected.Hits[i].HitFields[j].value);
				}
			}
		}
	}
}
