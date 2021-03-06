using Paillave.Etl;
using Paillave.Etl.Extensions;
using Paillave.Etl.Core.Streams;
using System.Linq;

namespace ExamplesQuickStart.Jobs
{
    public class CrossApplyActionJobs
    {
        public static void DefineProcess(ISingleStream<object> rootStream)
        {
            rootStream
                .CrossApplyEnumerable("create some values", (input) => Enumerable.Range(0, 10).Select(i => new { Id = i, Value = (i % 3 == 0) ? i : (int?)null }))
                .Select("set null value to the previous not null value", 0, (i, ctx, setCtx) =>
                {
                    var v = i.Value ?? ctx;
                    setCtx(v);
                    return new { i.Id, Value = v };
                });
        }
    }
}
