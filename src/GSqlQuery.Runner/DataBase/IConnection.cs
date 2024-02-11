﻿using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.Runner
{
    public interface IConnection : IDisposable
    {
        ConnectionState State { get; }

        object GetDbCommand();

        void Close();

        Task CloseAsync(CancellationToken cancellationToken = default);

        ITransaction BeginTransaction();

        ITransaction BeginTransaction(IsolationLevel isolationLevel);

        Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);

        void RemoveTransaction(ITransaction transaction);
    }

    public interface IConnection<TITransaccion,TDbCommand> : IConnection, IDisposable
        where TITransaccion: ITransaction
        where TDbCommand : DbCommand
    {
        new TDbCommand GetDbCommand();

        new TITransaccion BeginTransaction();

        new TITransaccion BeginTransaction(IsolationLevel isolationLevel);

        new Task<TITransaccion> BeginTransactionAsync(CancellationToken cancellationToken = default);

        new Task<TITransaccion> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);

        void RemoveTransaction(TITransaccion transaction);
    }
}