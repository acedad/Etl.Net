﻿using Paillave.Etl;
using Paillave.Etl.Extensions;
using Paillave.Etl.Core.Streams;
using Paillave.Etl.SqlServer.StreamNodes;
using Paillave.Etl.SqlServer.ValuesProviders;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Paillave.Etl.SqlServer.Extensions
{
    public static class SqlServerEx
    {
        public static IStream<TOut> CrossApplySqlServerQuery<TIn, TOut>(this IStream<TIn> stream, string name, ISingleStream<SqlConnection> sqlConnection, string sqlQuery, bool noParallelisation = false)
        {
            var valuesProvider = new SqlCommandValueProvider<TIn, TOut>(sqlQuery);
            return stream.CrossApply<TIn, SqlConnection, TOut>(name, sqlConnection, valuesProvider.PushValues, noParallelisation);
        }
        public static IStream<TIn> ThroughSqlServer<TIn>(this IStream<TIn> stream, string name, ISingleStream<SqlConnection> sqlConnectionStream, string sqlQuery)
            where TIn : class
        {
            return new ThroughSqlCommandStreamNode<TIn, IStream<TIn>>(name, new ThroughSqlCommandArgs<TIn, IStream<TIn>>
            {
                SourceStream = stream,
                SqlConnectionStream = sqlConnectionStream,
                SqlQuery = sqlQuery
            }).Output;
        }
    }
}
